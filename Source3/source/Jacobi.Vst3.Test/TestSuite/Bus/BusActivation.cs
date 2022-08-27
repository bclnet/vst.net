using Jacobi.Vst3;
using static Jacobi.Vst3.TResult;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Bus Activation.
    /// </summary>
    public class BusActivationTest : TestBase
    {
        public override string Name => "Bus Activation";

        public BusActivationTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            var numTotalBusses = 0;
            var numFailedActivations = 0;

            for (var type = MediaTypes.Audio; type < MediaTypes.NumMediaTypes; type++)
            {
                var numInputs = vstPlug.GetBusCount(type, BusDirections.Input);
                var numOutputs = vstPlug.GetBusCount(type, BusDirections.Output);

                numTotalBusses += numInputs + numOutputs;

                for (var i = 0; i < numInputs + numOutputs; ++i)
                {
                    var busDirection = i < numInputs ? BusDirections.Input : BusDirections.Output;
                    var busIndex = busDirection == BusDirections.Input ? i : i - numInputs;

                    if (vstPlug.GetBusInfo(type, busDirection, busIndex, out var busInfo) != kResultTrue)
                    {
                        testResult.AddErrorMessage("IComponent::getBusInfo (..) failed.");
                        return false;
                    }

                    testResult.AddMessage(string.Format("   Bus Activation: {0} {1} Bus ({2}) ({3})",
                        busDirection == BusDirections.Input ? "Input" : "Output",
                        type == MediaTypes.Audio ? "Audio" : "Event", busIndex,
                        busInfo.BusType == BusTypes.Main ? "kMain" : "kAux"));

                    if ((busInfo.Flags & BusInfo.BusFlags.DefaultActive) == 0)
                    {
                        if (vstPlug.ActivateBus(type, busDirection, busIndex, true) != kResultOk) numFailedActivations++;
                        if (vstPlug.ActivateBus(type, busDirection, busIndex, false) != kResultOk) numFailedActivations++;
                    }
                    else if ((busInfo.Flags & BusInfo.BusFlags.DefaultActive) != 0)
                    {
                        if (vstPlug.ActivateBus(type, busDirection, busIndex, false) != kResultOk) numFailedActivations++;
                        if (vstPlug.ActivateBus(type, busDirection, busIndex, true) != kResultOk) numFailedActivations++;
                    }
                }
            }

            if (numFailedActivations > 0) testResult.AddErrorMessage("Bus activation failed.");
            return numFailedActivations == 0;
        }
    }
}
