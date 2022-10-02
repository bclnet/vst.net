using Steinberg.Vst3;
using System;

namespace Steinberg.Vst3.TestSuite
{
    /// <summary>
    /// Test Input Overwriting
    /// </summary>
    public unsafe class ProcessInputOverwritingTest : ProcessTest
    {
        bool noNeedtoProcess;

        public override string Name => "Process Input Overwriting";

        public ProcessInputOverwritingTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            var ret = base.Run(testResult);
            return ret;
        }

        protected override bool PreProcess(ITestResult testResult)
        {
            var min = processData._.NumInputs < processData._.NumOutputs ? processData._.NumInputs : processData._.NumOutputs;
            noNeedtoProcess = true;

            for (var i = 0; i < min; i++)
            {
                if (!noNeedtoProcess) break;

                var minChannel = processData._.Inputs[i].NumChannels < processData._.Outputs[i].NumChannels ? processData._.Inputs[i].NumChannels : processData._.Outputs[i].NumChannels;

                var ptrIn = processData._.Inputs[i].ChannelBuffers32;
                var ptrOut = processData._.Outputs[i].ChannelBuffers32;
                for (var j = 0; j < minChannel; j++)
                    if (ptrIn[j] != ptrOut[j]) { noNeedtoProcess = false; break; }
            }
            if (noNeedtoProcess) return true;

            for (var i = 0; i < processData._.NumInputs; i++)
            {
                if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
                {
                    var ptr = processData._.Inputs[i].ChannelBuffers32;
                    if (ptr != null)
                    {
                        var inc = 1f / (processData._.NumSamples - 1);
                        for (var c = 0; c < processData._.Inputs[i].NumChannels; c++)
                        {
                            var chaBuf = ptr[c];
                            for (var j = 0; j < processData._.NumSamples; j++)
                            {
                                *chaBuf = inc * j;
                                chaBuf++;
                            }
                        }
                    }
                }
                else if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                {
                    var ptr = processData._.Inputs[i].ChannelBuffers64;
                    if (ptr != null)
                    {
                        var inc = 1f / (processData._.NumSamples - 1);
                        for (var c = 0; c < processData._.Inputs[i].NumChannels; c++)
                        {
                            var chaBuf = ptr[c];
                            for (var j = 0; j < processData._.NumSamples; j++)
                            {
                                *chaBuf = inc * j;
                                chaBuf++;
                            }
                        }
                    }
                }
            }
            return true;
        }

        protected override bool PostProcess(ITestResult testResult)
        {
            if (noNeedtoProcess) return true;

            for (var i = 0; i < processData._.NumInputs; i++)
            {
                if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
                {
                    var ptr = processData._.Inputs[i].ChannelBuffers32;
                    if (ptr != null)
                    {
                        var inc = 1f / (processData._.NumSamples - 1);
                        for (var c = 0; c < processData._.Inputs[i].NumChannels; c++)
                        {
                            var chaBuf = ptr[c];
                            for (var j = 0; j < processData._.NumSamples; j++)
                            {
                                if (*chaBuf != inc * j)
                                {
                                    testResult.AddErrorMessage("IAudioProcessor::process overwrites input buffer (..with kSample32..)!");
                                    return false;
                                }
                                chaBuf++;
                            }
                        }
                    }
                }
                else if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                {
                    var ptr = processData._.Inputs[i].ChannelBuffers64;
                    if (ptr != null)
                    {
                        var inc = 1f / (processData._.NumSamples - 1);
                        for (var c = 0; c < processData._.Inputs[i].NumChannels; c++)
                        {
                            var chaBuf = ptr[c];
                            for (var j = 0; j < processData._.NumSamples; j++)
                            {
                                if (*chaBuf != inc * j)
                                {
                                    testResult.AddErrorMessage("IAudioProcessor::process overwrites input buffer (..with kSample64..)!");
                                    return false;
                                }
                                chaBuf++;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
