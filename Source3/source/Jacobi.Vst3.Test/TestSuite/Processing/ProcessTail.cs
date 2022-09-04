using Steinberg.Vst3;
using System;
using System.Runtime.InteropServices;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
{
    /// <summary>
    /// Test ProcesTail.
    /// </summary>
    public unsafe class ProcessTailTest : ProcessTest
    {
        uint _tailSamples;
        int _inTail;

        float* dataPtrFloat;
        double* dataPtrDouble;
        bool _inSilenceInput;
        bool _dontTest;

        public override string Name => "Check Tail processing";

        public ProcessTailTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }
        ~ProcessTailTest()
        {
            if (dataPtrFloat != null)
            {
                Marshal.FreeHGlobal((IntPtr)dataPtrFloat);
                dataPtrFloat = null;
            }
            if (dataPtrDouble != null)
            {
                Marshal.FreeHGlobal((IntPtr)dataPtrDouble);
                dataPtrDouble = null;
            }
        }

        public override bool Setup()
        {
            var result = base.Setup();
            if (result)
            {
                _tailSamples = audioEffect.GetTailSamples();

                StringResult subCat = new();
                plugProvider.GetSubCategories(subCat);
                if (subCat.Get().Contains("Generator") || subCat.Get().Contains("Instrument")) _dontTest = true;
            }
            return result;
        }

        protected override bool PreProcess(ITestResult testResult)
        {
            if (!_inSilenceInput)
            {
                var rand = new Random((int)DateTime.Now.Ticks);
                if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
                {
                    if (dataPtrFloat == null) dataPtrFloat = (float*)Marshal.AllocHGlobal(processData._.NumSamples * sizeof(float));
                    var ptr = dataPtrFloat;
                    for (var i = 0; i < processData._.NumSamples; ++i) ptr[i] = (float)(2 * rand.Next() / 32767.0 - 1);
                }
                else
                {
                    if (dataPtrDouble == null) dataPtrDouble = (double*)Marshal.AllocHGlobal(processData._.NumSamples * sizeof(double));
                    var ptr = dataPtrDouble;
                    for (var i = 0; i < processData._.NumSamples; ++i) ptr[i] = (double)(2 * rand.Next() / 32767.0 - 1);
                }
                for (var i = 0; i < processData._.NumOutputs; ++i)
                    for (var c = 0; c < processData._.OutputsX[i].NumChannels; ++c)
                        if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
                            Platform.Memset((IntPtr)processData._.OutputsX[i].ChannelBuffers32X[c], 0, processData._.NumSamples * sizeof(float));
                        else
                            Platform.Memset((IntPtr)processData._.OutputsX[i].ChannelBuffers64X[c], 0, processData._.NumSamples * sizeof(double));
                for (var i = 0; i < processData._.NumInputs; ++i)
                    for (var c = 0; c < processData._.InputsX[i].NumChannels; ++c)
                        if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
                            Platform.Memcpy((IntPtr)processData._.InputsX[i].ChannelBuffers32X[c], (IntPtr)dataPtrFloat, processData._.NumSamples * sizeof(float));
                        else
                            Platform.Memcpy((IntPtr)processData._.InputsX[i].ChannelBuffers64X[c], (IntPtr)dataPtrDouble, processData._.NumSamples * sizeof(double));
            }
            else
            {
                // process with silent buffers
                for (var i = 0; i < processData._.NumOutputs; ++i)
                    for (var c = 0; c < processData._.OutputsX[i].NumChannels; ++c)
                        if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
                            Platform.Memset((IntPtr)processData._.OutputsX[i].ChannelBuffers32X[c], 0, processData._.NumSamples * sizeof(float));
                        else
                            Platform.Memset((IntPtr)processData._.OutputsX[i].ChannelBuffers64X[c], 0, processData._.NumSamples * sizeof(double));
                for (var i = 0; i < processData._.NumInputs; ++i)
                    for (var c = 0; c < processData._.InputsX[i].NumChannels; ++c)
                        if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
                            Platform.Memset((IntPtr)processData._.InputsX[i].ChannelBuffers32X[c], 0, processData._.NumSamples * sizeof(float));
                        else
                            Platform.Memset((IntPtr)processData._.InputsX[i].ChannelBuffers64X[c], 0, processData._.NumSamples * sizeof(double));
            }

            return true;
        }

        protected override bool PostProcess(ITestResult testResult)
        {
            if (_inSilenceInput)
            {
                // should be silence
                if (_tailSamples < _inTail + processData._.NumSamples)
                {
                    var start = _tailSamples > _inTail ? _tailSamples - _inTail : 0;
                    var end = processData._.NumSamples;

                    for (var i = 0; i < processData._.NumOutputs; ++i)
                        for (var c = 0; c < processData._.OutputsX[i].NumChannels; ++c)
                        {
                            if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
                            {
                                for (var s = start; s < end; ++s)
                                    if (Math.Abs(processData._.OutputsX[i].ChannelBuffers32X[c][s]) >= 1e-7)
                                    {
                                        testResult.AddErrorMessage($"IAudioProcessor::process (..) generates non silent output for silent input for tail above {_tailSamples} samples.");
                                        return false;
                                    }
                            }
                            else
                            {
                                for (var s = start; s < end; ++s)
                                    if (Math.Abs(processData._.OutputsX[i].ChannelBuffers64X[c][s]) >= 1e-7)
                                    {
                                        testResult.AddErrorMessage($"IAudioProcessor::process (..) generates non silent output for silent input for tail above {_tailSamples} samples.");
                                        return false;
                                    }
                            }
                        }
                }
                _inTail += processData._.NumSamples;
            }
            return true;
        }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || audioEffect == null) return false;
            if (processSetup.SymbolicSampleSize != processData._.SymbolicSampleSize) return false;
            if (!CanProcessSampleSize(testResult)) return true;
            if (_dontTest) return true;

            testResult.AddMessage($"==={Name} == Tail={_tailSamples} ======================");

            audioEffect.SetProcessing(true);

            // process with signal (noise) and silence
            for (var i = 0; i < 20 * TestDefaults.Instance.NumAudioBlocksToProcess; ++i)
            {
                _inSilenceInput = i > 10;

                if (!PreProcess(testResult)) return false;
                var result = audioEffect.Process(processData._);
                if (result != kResultOk)
                {
                    testResult.AddErrorMessage("IAudioProcessor::process (..) failed.");

                    audioEffect.SetProcessing(false);
                    return false;
                }
                if (!PostProcess(testResult))
                {
                    audioEffect.SetProcessing(false);
                    return false;
                }
            }

            audioEffect.SetProcessing(false);
            return true;
        }
    }
}
