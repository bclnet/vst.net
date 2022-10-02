using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
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

            var numInputs = vstPlug.GetBusCount(MediaType.Audio, BusDirection.Input);
            var numOutputs = vstPlug.GetBusCount(MediaType.Audio, BusDirection.Output);
            var arrangementMismatchs = 0;

            if (vstPlug is IAudioProcessor audioEffect)
                for (var i = 0; i < numInputs + numOutputs; ++i)
                {
                    var dir = i < numInputs ? BusDirection.Input : BusDirection.Output;
                    var busIndex = dir == BusDirection.Input ? i : i - numInputs;

                    testResult.AddMessage(string.Format("   Check {0} Audio Bus Arrangement ({1})",
                        dir == BusDirection.Input ? "Input" : "Output", busIndex));

                    if (vstPlug.GetBusInfo(MediaType.Audio, dir, busIndex, out var busInfo) == kResultTrue)
                    {
                        if (audioEffect.GetBusArrangement(dir, busIndex, out var arrangement) == kResultTrue)
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
