using Steinberg.Vst3;
using Steinberg.Vst3.Hosting;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
{
    /// <summary>
    /// Test Scan Buses.
    /// </summary>
    public class ScanBussesTest : TestBase
    {
        public override string Name => "Scan Buses";

        public ScanBussesTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            var numBusses = 0;

            for (var mediaType = MediaType.Audio; mediaType < MediaType.NumMediaTypes; mediaType++)
            {
                var numInputs = vstPlug.GetBusCount(mediaType, BusDirection.Input);
                var numOutputs = vstPlug.GetBusCount(mediaType, BusDirection.Output);

                numBusses += numInputs + numOutputs;

                if (mediaType == (MediaType.NumMediaTypes - 1) && numBusses == 0)
                {
                    testResult.AddErrorMessage("This component does not export any buses!!!");
                    return false;
                }

                testResult.AddMessage(string.Format("=> {0} Buses: [{1} In(s) => {2} Out(s)]",
                    mediaType == MediaType.Audio ? "Audio" : "Event", numInputs, numOutputs));

                for (var i = 0; i < numInputs + numOutputs; ++i)
                {
                    var busDirection = i < numInputs ? BusDirection.Input : BusDirection.Output;
                    var busIndex = busDirection == BusDirection.Input ? i : i - numInputs;

                    if (vstPlug.GetBusInfo(mediaType, busDirection, busIndex, out var busInfo) == kResultTrue)
                    {
                        var busName = busInfo.Name;
                        if (string.IsNullOrWhiteSpace(busName))
                        {
                            testResult.AddErrorMessage($"Bus {busIndex} has no name!!!");
                            return false;
                        }
                        testResult.AddMessage(string.Format("     {0}[{1}]: \"{2}\" ({3}-{4}) ", busDirection == BusDirection.Input ? "In " : "Out",
                            busIndex, busName, busInfo.BusType == BusType.Main ? "Main" : "Aux",
                            (busInfo.Flags & BusFlags.DefaultActive) != 0 ? "Default Active" : "Default Inactive"));
                    }
                    else return false;
                }
            }
            return true;
        }
    }
}
