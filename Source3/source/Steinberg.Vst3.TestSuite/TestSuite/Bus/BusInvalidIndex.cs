using Steinberg.Vst3;
using Steinberg.Vst3.Hosting;
using System;

namespace Steinberg.Vst3.TestSuite
{
    /// <summary>
    /// Test Bus Invalid Index.
    /// </summary>
    public class BusInvalidIndexTest : TestBase
    {
        public override string Name => "Bus Invalid Index";

        public BusInvalidIndexTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            var failed = false;
            var numInvalidDesc = 0;

            var rand = new Random((int)DateTime.Now.Ticks);
            for (var mediaType = MediaType.Audio; mediaType < MediaType.NumMediaTypes; mediaType++)
            {
                var numBusses = vstPlug.GetBusCount(mediaType, BusDirection.Input) + vstPlug.GetBusCount(mediaType, BusDirection.Output);
                for (var dir = BusDirection.Input; dir <= BusDirection.Output; dir++)
                {
                    var descBefore = new BusInfo();
                    var descAfter = new BusInfo();
                    int randIndex;

                    // todo: rand with negative numbers
                    for (var i = 0; i <= numBusses * TestDefaults.Instance.NumIterations; ++i)
                    {
                        randIndex = rand.Next();
                        if (0 > randIndex || randIndex > numBusses)
                        {
                            vstPlug.GetBusInfo(mediaType, dir, randIndex, out descAfter);
                            if (descBefore != descAfter)
                            {
                                failed |= true;
                                numInvalidDesc++;
                            }
                        }
                    }
                }
            }

            if (numInvalidDesc > 0)
                testResult.AddErrorMessage($"The component returned {numInvalidDesc} buses queried with an invalid index!");
            return !failed;
        }
    }
}
