using Steinberg.Vst3;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
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

            for (var type = MediaType.Audio; type < MediaType.NumMediaTypes; type++)
            {
                var numInputs = vstPlug.GetBusCount(type, BusDirection.Input);
                var numOutputs = vstPlug.GetBusCount(type, BusDirection.Output);

                numTotalBusses += numInputs + numOutputs;

                for (var i = 0; i < numInputs + numOutputs; ++i)
                {
                    var busDirection = i < numInputs ? BusDirection.Input : BusDirection.Output;
                    var busIndex = busDirection == BusDirection.Input ? i : i - numInputs;

                    if (vstPlug.GetBusInfo(type, busDirection, busIndex, out var busInfo) != kResultTrue)
                    {
                        testResult.AddErrorMessage("IComponent::getBusInfo (..) failed.");
                        return false;
                    }

                    testResult.AddMessage(string.Format("   Bus Activation: {0} {1} Bus ({2}) ({3})",
                        busDirection == BusDirection.Input ? "Input" : "Output",
                        type == MediaType.Audio ? "Audio" : "Event", busIndex,
                        busInfo.BusType == BusType.Main ? "kMain" : "kAux"));

                    if ((busInfo.Flags & BusFlags.DefaultActive) == 0)
                    {
                        if (vstPlug.ActivateBus(type, busDirection, busIndex, true) != kResultOk) numFailedActivations++;
                        if (vstPlug.ActivateBus(type, busDirection, busIndex, false) != kResultOk) numFailedActivations++;
                    }
                    else if ((busInfo.Flags & BusFlags.DefaultActive) != 0)
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
