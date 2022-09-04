using Steinberg.Vst3;
using Steinberg.Vst3.Hosting;
using System;
using System.Runtime.InteropServices;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
{
    /// <summary>
    /// Test Process Test.
    /// </summary>
    public unsafe class ProcessTest : TestEnh
    {
        protected HostProcessData processData = new();

        public override string Name => "Process Test";

        public ProcessTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl)
        {
            processData._.NumSamples = TestDefaults.Instance.DefaultBlockSize;
            processData._.SymbolicSampleSize = sampl;

            processSetup.ProcessMode = ProcessModes.Realtime;
            processSetup.SymbolicSampleSize = sampl;
            processSetup.MaxSamplesPerBlock = TestDefaults.Instance.MaxBlockSize;
            processSetup.SampleRate = TestDefaults.Instance.DefaultSampleRate;
        }

        public override bool Setup()
        {
            if (!base.Setup()) return false;
            if (vstPlug == null || audioEffect == null) return false;
            if (processSetup.SymbolicSampleSize != processData._.SymbolicSampleSize) return false;
            if (audioEffect.CanProcessSampleSize(processSetup.SymbolicSampleSize) != kResultOk) return true; // this fails in run (..)

            PrepareProcessing();

            return vstPlug.SetActive(true) == kResultTrue;
        }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            if (processSetup.SymbolicSampleSize != processData._.SymbolicSampleSize) return false;
            if (!CanProcessSampleSize(testResult)) return true;

            audioEffect.SetProcessing(true);

            for (var i = 0; i < TestDefaults.Instance.NumAudioBlocksToProcess; ++i)
            {
                if (!PreProcess(testResult)) return false;
                var result = audioEffect.Process(ref processData._);
                if (result != kResultOk)
                {
                    testResult.AddErrorMessage(processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32
                        ? "IAudioProcessor::process (..with kSample32..) failed."
                        : "IAudioProcessor::process (..with kSample64..) failed.");
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

        protected virtual bool PreProcess(ITestResult testResult) => true;

        protected virtual bool PostProcess(ITestResult testResult) => true;

        protected bool CanProcessSampleSize(ITestResult testResult)
        {
            if (testResult == null || audioEffect == null) return false;
            if (processSetup.SymbolicSampleSize != processData._.SymbolicSampleSize) return false;
            if (audioEffect.CanProcessSampleSize(processSetup.SymbolicSampleSize) != kResultOk)
            {
                testResult.AddMessage(processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32
                    ? "32bit Audio Processing not supported."
                    : "64bit Audio Processing not supported.");
                return false;
            }
            return true;
        }

        public override bool Teardown()
        {
            UnprepareProcessing();
            if (vstPlug == null || vstPlug.SetActive(false) != kResultOk) return false;
            return base.Teardown();
        }

        protected virtual bool PrepareProcessing()
        {
            if (vstPlug == null || audioEffect == null) return false;

            if (audioEffect.SetupProcessing(processSetup) == kResultOk)
            {
                processData.Prepare(vstPlug, 0, processSetup.SymbolicSampleSize);

                for (var dir = BusDirection.Input; dir <= BusDirection.Output; dir++)
                {
                    var numBusses = vstPlug.GetBusCount(MediaType.Audio, dir);
                    var audioBuffers = dir == BusDirection.Input ? processData._.Inputs : processData._.Outputs;
                    if (!SetupBuffers(numBusses, audioBuffers, dir)) return false;

                    if (dir == BusDirection.Input)
                    {
                        processData._.NumInputs = numBusses;
                        processData._.Inputs = audioBuffers;
                    }
                    else
                    {
                        processData._.NumOutputs = numBusses;
                        processData._.Outputs = audioBuffers;
                    }
                }
                return true;
            }
            return false;
        }

        protected bool SetupBuffers(int numBusses, IntPtr audioBuffers, BusDirection dir)
        {
            if ((numBusses > 0 && audioBuffers == IntPtr.Zero) || vstPlug == null) return false;

            var audioBuffers2 = (AudioBusBuffers*)audioBuffers;
            for (var busIndex = 0; busIndex < numBusses; busIndex++) // buses
            {
                if (vstPlug.GetBusInfo(MediaType.Audio, dir, busIndex, out var busInfo) == kResultTrue)
                {
                    if (!SetupBuffers(ref audioBuffers2[busIndex])) return false;

                    if ((busInfo.Flags & BusFlags.DefaultActive) != 0)
                        for (var chIdx = 0; chIdx < busInfo.ChannelCount; chIdx++) // channels per bus
                            audioBuffers2[busIndex].SilenceFlags |= TestDefaults.Instance.ChannelIsSilent << chIdx;
                }
                else return false;
            }
            return true;
        }

        protected bool SetupBuffers(ref AudioBusBuffers audioBuffers)
        {
            if (processSetup.SymbolicSampleSize != processData._.SymbolicSampleSize) return false;
            audioBuffers.SilenceFlags = 0;
            for (var chIdx = 0; chIdx < audioBuffers.NumChannels; chIdx++)
                if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
                {
                    if (audioBuffers.ChannelBuffers32X != null)
                    {
                        var channelBuffers32 = audioBuffers.ChannelBuffers32X;
                        channelBuffers32[chIdx] = (Single*)Marshal.AllocHGlobal(sizeof(Single) * processSetup.MaxSamplesPerBlock);
                        if (channelBuffers32[chIdx] != null) Platform.Memset((IntPtr)channelBuffers32[chIdx], 0, (IntPtr)(processSetup.MaxSamplesPerBlock * sizeof(Single)));
                        else return false;
                    }
                    else return false;
                }
                else if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                {
                    if (audioBuffers.ChannelBuffers64X != null)
                    {
                        var channelBuffers64 = audioBuffers.ChannelBuffers64X;
                        channelBuffers64[chIdx] = (Double*)Marshal.AllocHGlobal(sizeof(Double) * processSetup.MaxSamplesPerBlock);
                        if (channelBuffers64[chIdx] != null) Platform.Memset((IntPtr)channelBuffers64[chIdx], 0, (IntPtr)(processSetup.MaxSamplesPerBlock * sizeof(Double)));
                        else return false;
                    }
                    else return false;
                }
                else return false;
            return true;
        }

        protected virtual bool UnprepareProcessing()
        {
            var ret = true;
            ret &= FreeBuffers(processData._.NumInputs, processData._.Inputs);
            ret &= FreeBuffers(processData._.NumOutputs, processData._.Outputs);
            processData.Unprepare();
            return ret;
        }

        protected bool FreeBuffers(int numBuses, IntPtr buses)
        {
            if (processSetup.SymbolicSampleSize != processData._.SymbolicSampleSize) return false;
            var buses2 = (AudioBusBuffers*)buses;
            for (var busIndex = 0; busIndex < numBuses; busIndex++)
                for (var chIdx = 0; chIdx < buses2[busIndex].NumChannels; chIdx++)
                {
                    if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32) Marshal.FreeHGlobal((IntPtr)buses2[busIndex].ChannelBuffers32X[chIdx]);
                    else if (processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample64) Marshal.FreeHGlobal((IntPtr)buses2[busIndex].ChannelBuffers64X[chIdx]);
                    else return false;
                }
            return true;
        }
    }
}
