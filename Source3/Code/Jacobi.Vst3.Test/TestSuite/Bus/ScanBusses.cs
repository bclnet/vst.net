using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;

namespace Jacobi.Vst3.TestSuite
{
    public class ScanBussesTest : TestBase
    {
        public override string Name => "Scan Buses";

        public ScanBussesTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            var numBusses = 0;

            for (var mediaType = MediaTypes.Audio; mediaType < MediaTypes.NumMediaTypes; mediaType++)
            {
                var numInputs = vstPlug.GetBusCount(mediaType, BusDirections.Input);
                var numOutputs = vstPlug.GetBusCount(mediaType, BusDirections.Output);

                numBusses += numInputs + numOutputs;

                if (mediaType == (MediaTypes.NumMediaTypes - 1) && numBusses == 0)
                {
                    testResult.AddErrorMessage("This component does not export any buses!!!");
                    return false;
                }

                testResult.AddMessage(string.Format("=> {0} Buses: [{1} In(s) => {2} Out(s)]",
                    mediaType == MediaTypes.Audio ? "Audio" : "Event", numInputs, numOutputs));

                for (var i = 0; i < numInputs + numOutputs; ++i)
                {
                    var busDirection = i < numInputs ? BusDirections.Input : BusDirections.Output;
                    var busIndex = busDirection == BusDirections.Input ? i : i - numInputs;

                    BusInfo busInfo = new();
                    if (vstPlug.GetBusInfo(mediaType, busDirection, busIndex, ref busInfo) == TResult.S_True)
                    {
                        var busName = busInfo.Name;
                        if (string.IsNullOrWhiteSpace(busName))
                        {
                            testResult.AddErrorMessage($"Bus {busIndex} has no name!!!");
                            return false;
                        }
                        testResult.AddMessage(string.Format("     {0}[{1}]: \"{2}\" ({3}-{4}) ", busDirection == BusDirections.Input ? "In " : "Out",
                            busIndex, busName, busInfo.BusType == BusTypes.Main ? "Main" : "Aux",
                            (busInfo.Flags & BusInfo.BusFlags.DefaultActive) != 0 ? "Default Active" : "Default Inactive"));
                    }
                    else return false;
                }
            }
            return true;
        }
    }
}
