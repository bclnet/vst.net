using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Hosting;

namespace Jacobi.Vst3.TestSuite
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
            var numInBusses = vstPlug.GetBusCount(MediaTypes.Audio, BusDirections.Input);
            if (numInBusses < 2)
                return true;

            for (var busIndex = 0; busIndex < numInBusses; busIndex++)
            {
                if (vstPlug.GetBusInfo(MediaTypes.Audio, BusDirections.Input, busIndex, out var info) != TResult.S_True)
                {
                    testResult.AddErrorMessage("IComponent::getBusInfo (..) failed.");
                    continue;
                }
                if (info.BusType == BusTypes.Aux) hasInputSideChain = true;
            }
            if (!hasInputSideChain) return true;

            var inputArrArray = new SpeakerArrangement[numInBusses];
            for (var busIndex = 0; busIndex < numInBusses; busIndex++)
                if (audioEffect.GetBusArrangement(BusDirections.Input, busIndex, out inputArrArray[busIndex]) != TResult.S_True)
                    testResult.AddErrorMessage("IComponent::getBusArrangement (..) failed.");

            var numOutBusses = vstPlug.GetBusCount(MediaTypes.Audio, BusDirections.Output);
            SpeakerArrangement[] outputArrArray = null;
            if (numOutBusses > 0)
            {
                outputArrArray = new SpeakerArrangement[numOutBusses];
                for (var busIndex = 0; busIndex < numOutBusses; busIndex++)
                    if (audioEffect.GetBusArrangement(BusDirections.Output, busIndex, out outputArrArray[busIndex]) != TResult.S_True)
                        testResult.AddErrorMessage("IComponent::getBusArrangement (..) failed.");
                outputArrArray[0] = SpeakerArrangement.kMono;
            }
            inputArrArray[0] = SpeakerArrangement.kMono;

            if (audioEffect.SetBusArrangements(inputArrArray, numInBusses, outputArrArray, numOutBusses) == TResult.S_True)
            {
                for (var busIndex = 0; busIndex < numInBusses; busIndex++)
                {
                    if (audioEffect.GetBusArrangement(BusDirections.Input, busIndex, out var tmp) == TResult.S_True)
                        if (tmp != inputArrArray[busIndex])
                        {
                            testResult.AddErrorMessage($"Input {busIndex}: setBusArrangements was returning kResultTrue but getBusArrangement returns different arrangement!");
                            failed = true;
                        }
                }
                for (var busIndex = 0; busIndex < numOutBusses; busIndex++)
                {
                    if (audioEffect.GetBusArrangement(BusDirections.Output, busIndex, out var tmp) != TResult.S_True)
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
