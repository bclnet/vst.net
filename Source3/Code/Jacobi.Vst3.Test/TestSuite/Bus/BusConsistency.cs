using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Hosting;
using System;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Bus Consistency.
    /// </summary>
    public class BusConsistencyTest : TestBase
    {
        public override string Name => "Bus Consistency";

        public BusConsistencyTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            var failed = false;
            var numFalseDescQueries = 0;

            var rand = new Random((int)DateTime.Now.Ticks);
            for (var mediaType = MediaTypes.Audio; mediaType < MediaTypes.NumMediaTypes; mediaType++)
                for (var dir = BusDirections.Input; dir <= BusDirections.Output; dir++)
                {
                    var numBusses = vstPlug.GetBusCount(mediaType, dir);
                    if (numBusses > 0)
                    {
                        var busArray = new BusInfo[numBusses];
                        if (busArray != null)
                        {
                            // get all bus descriptions and save them in an array
                            int busIndex;
                            for (busIndex = 0; busIndex < numBusses; busIndex++)
                            {
                                busArray[busIndex].Clear();
                                vstPlug.GetBusInfo(mediaType, dir, busIndex, out busArray[busIndex]);
                            }

                            // test by getting descriptions randomly and comparing with saved ones
                            BusInfo info = new();
                            int randIndex;

                            for (busIndex = 0; busIndex <= numBusses * TestDefaults.Instance.NumIterations; busIndex++)
                            {
                                randIndex = rand.Next() % numBusses;

                                info.Clear();
                                vstPlug.GetBusInfo(mediaType, dir, randIndex, out info);
                                if (busArray[randIndex] != info)
                                {
                                    failed |= true;
                                    numFalseDescQueries++;
                                }
                            }
                            busArray = null;
                        }
                    }
                }

            if (numFalseDescQueries > 0)
                testResult.AddErrorMessage($"The component returned {numFalseDescQueries} inconsistent buses! (getBusInfo () returns sometime different info for the same bus!");
            return !failed;
        }
    }
}
