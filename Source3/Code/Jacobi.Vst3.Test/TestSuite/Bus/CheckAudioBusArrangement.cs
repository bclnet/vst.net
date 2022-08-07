using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Hosting;
using Jacobi.Vst3.Plugin;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Check Audio Bus Arrangement.
    /// </summary>
    public class CheckAudioBusArrangementTest : TestBase
    {
        public override string Name => "Check Audio Bus Arrangement";

        public CheckAudioBusArrangementTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            var numInputs = vstPlug.GetBusCount(MediaTypes.Audio, BusDirections.Input);
            var numOutputs = vstPlug.GetBusCount(MediaTypes.Audio, BusDirections.Output);
            var arrangementMismatchs = 0;

            if (vstPlug is IAudioProcessor audioEffect)
                for (var i = 0; i < numInputs + numOutputs; ++i)
                {
                    var dir = i < numInputs ? BusDirections.Input : BusDirections.Output;
                    var busIndex = dir == BusDirections.Input ? i : i - numInputs;

                    testResult.AddMessage(string.Format("   Check {0} Audio Bus Arrangement ({1})",
                        dir == BusDirections.Input ? "Input" : "Output", busIndex));

                    if (vstPlug.GetBusInfo(MediaTypes.Audio, dir, busIndex, out var busInfo) == TResult.S_True)
                    {
                        if (audioEffect.GetBusArrangement(dir, busIndex, out var arrangement) == TResult.S_True)
                        {
                            if (busInfo.ChannelCount != arrangement.GetChannelCount())
                            {
                                arrangementMismatchs++;
                                testResult.AddErrorMessage("channelCount is inconsistent!");
                            }
                        }
                        else
                        {
                            testResult.AddErrorMessage("IAudioProcessor::getBusArrangement (..) failed!");
                            return false;
                        }
                    }
                    else
                    {
                        testResult.AddErrorMessage("IComponent::getBusInfo (..) failed!");
                        return false;
                    }
                }
            return arrangementMismatchs == 0;
        }
    }
}
