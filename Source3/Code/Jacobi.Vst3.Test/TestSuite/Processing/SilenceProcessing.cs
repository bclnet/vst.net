using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using System;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Silence Processing.
    /// </summary>
    public unsafe class SilenceProcessingTest : ProcessTest
    {
        public override string Name => "Silence Processing";

        public SilenceProcessingTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        protected bool IsBufferSilent(void* buffer, int numSamples, SymbolicSampleSizes sampl)
        {
            if (sampl == SymbolicSampleSizes.Sample32)
            {
                const float kSilenceThreshold = 0.000132184039f;

                var floatBuffer = (float*)buffer;
                while (numSamples-- != 0)
                {
                    if (Math.Abs(*floatBuffer) > kSilenceThreshold) return false;
                    floatBuffer++;
                }
            }
            else if (sampl == SymbolicSampleSizes.Sample64)
            {
                const double kSilenceThreshold = 0.000132184039;

                var floatBuffer = (double*)buffer;
                while (numSamples-- != 0)
                {
                    if (Math.Abs(*floatBuffer) > kSilenceThreshold) return false;
                    floatBuffer++;
                }
            }
            return true;
        }

        public override bool Run(ITestResult testResult)
        {
            if (vstPlug == null || testResult == null || audioEffect == null) return false;

            if (!CanProcessSampleSize(testResult)) return true;

            PrintTestHeader(testResult);

            if (processData._.Inputs != IntPtr.Zero)
            {
                // process 20s before checking flags
                var numPasses = (int)(20 * processSetup.SampleRate / processData._.NumSamples + 0.5);

                audioEffect.SetProcessing(true);
                for (var pass = 0; pass < numPasses; pass++)
                {
                    for (var busIndex = 0; busIndex < processData._.NumInputs; busIndex++)
                    {
                        ((AudioBusBuffers*)processData._.Inputs)[busIndex].SilenceFlags = 0;
                        for (var channelIndex = 0; channelIndex < ((AudioBusBuffers*)processData._.Inputs)[busIndex].NumChannels; channelIndex++)
                        {
                            ((AudioBusBuffers*)processData._.Inputs)[busIndex].SilenceFlags |= 1UL << channelIndex;
                            if (processData._.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
                                Platform.Memset((IntPtr)((AudioBusBuffers*)processData._.Inputs)[busIndex].ChannelBuffers32_[channelIndex], 0, sizeof(float) * processData._.NumSamples);
                            else if (processData._.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                                Platform.Memset((IntPtr)((AudioBusBuffers*)processData._.Inputs)[busIndex].ChannelBuffers32_[channelIndex], 0, sizeof(double) * processData._.NumSamples);
                        }
                    }

                    for (var busIndex = 0; busIndex < processData._.NumOutputs; busIndex++)
                    {
                        if (processData._.NumInputs > busIndex) ((AudioBusBuffers*)processData._.Outputs)[busIndex].SilenceFlags = ((AudioBusBuffers*)processData._.Inputs)[busIndex].SilenceFlags;
                        else
                        {
                            ((AudioBusBuffers*)processData._.Outputs)[busIndex].SilenceFlags = 0;
                            for (var channelIndex = 0; channelIndex < ((AudioBusBuffers*)processData._.Outputs)[busIndex].NumChannels; channelIndex++)
                                ((AudioBusBuffers*)processData._.Outputs)[busIndex].SilenceFlags |= (1UL << channelIndex);
                        }
                    }

                    var result = audioEffect.Process(processData._);
                    if (result != TResult.S_OK)
                    {
                        testResult.AddErrorMessage("The component failed to process!");

                        audioEffect.SetProcessing(false);
                        return false;
                    }
                }

                for (var busIndex = 0; busIndex < processData._.NumOutputs; busIndex++)
                    for (var channelIndex = 0; channelIndex < ((AudioBusBuffers*)processData._.Outputs)[busIndex].NumChannels; channelIndex++)
                    {
                        var channelShouldBeSilent = (((AudioBusBuffers*)processData._.Outputs)[busIndex].SilenceFlags & 1UL << channelIndex) != 0;
                        var channelIsSilent = IsBufferSilent(((AudioBusBuffers*)processData._.Outputs)[busIndex].ChannelBuffers32_[channelIndex], processData._.NumSamples, processData._.SymbolicSampleSize);
                        if (channelShouldBeSilent != channelIsSilent)
                        {
                            var silentText = "The component reported a wrong silent flag for its output buffer! : output is silent but silenceFlags not set !";
                            var nonSilentText = "The component reported a wrong silent flag for its output buffer! : silenceFlags is set to silence but output is not silent";
                            testResult.AddMessage(channelIsSilent ? silentText : nonSilentText);
                            break;
                        }
                    }
            }
            else if (processData._.NumInputs > 0)
            {
                testResult.AddErrorMessage("ProcessData::inputs are 0 but ProcessData::numInputs are nonzero.");
                return false;
            }

            audioEffect.SetProcessing(false);
            return true;
        }
    }
}
