using Steinberg.Vst3;
using Steinberg.Vst3.Hosting;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
{
    /// <summary>
    /// Test SideChain Arrangement.
    /// </summary>
    public class SideChainArrangementTest : TestBase
    {
        public override string Name => "SideChain Arrangement";

        public SideChainArrangementTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            var failed = false;

            if (vstPlug is not IAudioProcessor audioEffect) return failed;

            // get the side chain arrangements
            // set Main/first Input and output to Mono
            // get the current arrangement and compare

            // check if Audio sideChain is supported
            var hasInputSideChain = false;
            var numInBusses = vstPlug.GetBusCount(MediaType.Audio, BusDirection.Input);
            if (numInBusses < 2)
                return true;

            for (var busIndex = 0; busIndex < numInBusses; busIndex++)
            {
                if (vstPlug.GetBusInfo(MediaType.Audio, BusDirection.Input, busIndex, out var info) != kResultTrue)
                {
                    testResult.AddErrorMessage("IComponent::getBusInfo (..) failed.");
                    continue;
                }
                if (info.BusType == BusType.Aux) hasInputSideChain = true;
            }
            if (!hasInputSideChain) return true;

            var inputArrArray = new SpeakerArrangement[numInBusses];
            for (var busIndex = 0; busIndex < numInBusses; busIndex++)
                if (audioEffect.GetBusArrangement(BusDirection.Input, busIndex, out inputArrArray[busIndex]) != kResultTrue)
                    testResult.AddErrorMessage("IComponent::getBusArrangement (..) failed.");

            var numOutBusses = vstPlug.GetBusCount(MediaType.Audio, BusDirection.Output);
            SpeakerArrangement[] outputArrArray = null;
            if (numOutBusses > 0)
            {
                outputArrArray = new SpeakerArrangement[numOutBusses];
                for (var busIndex = 0; busIndex < numOutBusses; busIndex++)
                    if (audioEffect.GetBusArrangement(BusDirection.Output, busIndex, out outputArrArray[busIndex]) != kResultTrue)
                        testResult.AddErrorMessage("IComponent::getBusArrangement (..) failed.");
                outputArrArray[0] = SpeakerArrangement.kMono;
            }
            inputArrArray[0] = SpeakerArrangement.kMono;

            if (audioEffect.SetBusArrangements(inputArrArray, numInBusses, outputArrArray, numOutBusses) == kResultTrue)
            {
                for (var busIndex = 0; busIndex < numInBusses; busIndex++)
                {
                    if (audioEffect.GetBusArrangement(BusDirection.Input, busIndex, out var tmp) == kResultTrue)
                        if (tmp != inputArrArray[busIndex])
                        {
                            testResult.AddErrorMessage($"Input {busIndex}: setBusArrangements was returning kResultTrue but getBusArrangement returns different arrangement!");
                            failed = true;
                        }
                }
                for (var busIndex = 0; busIndex < numOutBusses; busIndex++)
                {
                    if (audioEffect.GetBusArrangement(BusDirection.Output, busIndex, out var tmp) != kResultTrue)
                        if (tmp != outputArrArray[busIndex])
                        {
                            testResult.AddErrorMessage($"Output {busIndex}: setBusArrangements was returning kResultTrue but getBusArrangement returns different arrangement!");
                            failed = true;
                        }
                }
            }

            return !failed;
        }
    }
}
