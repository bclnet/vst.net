using Jacobi.Vst3.Core;
using Jacobi.Vst3.Hosting;
using static Jacobi.Vst3.Core.TResult;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Keyswitch.
    /// </summary>
    public class KeyswitchTest : TestBase
    {
        public override string Name => "Keyswitch";

        public KeyswitchTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            if (controller == null)
            {
                testResult.AddMessage("No Edit Controller supplied!");
                return true;
            }

            if (controller is not IKeyswitchController keyswitch)
            {
                testResult.AddMessage("No Keyswitch interface supplied!");
                return true;
            }

            var eventBusCount = vstPlug.GetBusCount(MediaTypes.Event, BusDirections.Input);
            for (var bus = 0; bus < eventBusCount; bus++)
            {
                vstPlug.GetBusInfo(MediaTypes.Event, BusDirections.Input, bus, out var busInfo);
                for (short channel = 0; channel < busInfo.ChannelCount; channel++)
                {
                    var count = keyswitch.GetKeyswitchCount(bus, channel);
                    if (count > 0) testResult.AddMessage($"Keyswitch support bus[{bus}], channel[{channel}]: {count}");

                    for (var i = 0; i < count; ++i)
                    {
                        KeyswitchInfo info = new();
                        if (keyswitch.GetKeyswitchInfo(bus, channel, i, ref info) == kResultTrue) { }
                        else
                        {
                            testResult.AddErrorMessage($"Keyswitch getKeyswitchInfo ({bus}, {channel}, {i}) return kResultFalse!");
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
