using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;
using System.Collections.Generic;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test MIDI Mapping.
    /// </summary>
    public class MidiMappingTest : TestBase
    {
        public override string Name => "MIDI Mapping";

        public MidiMappingTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            if (controller == null)
            {
                testResult.AddMessage("No Edit Controller supplied!");
                return true;
            }

            if (controller is not IMidiMapping midiMapping)
            {
                testResult.AddMessage("No MIDI Mapping interface supplied!");
                return true;
            }

            var numParameters = controller.GetParameterCount();
            var eventBusCount = vstPlug.GetBusCount(MediaTypes.Event, BusDirections.Input);
            var interruptProcess = false;

            var parameterIds = new HashSet<uint>();
            for (var i = 0; i < numParameters; ++i)
            {
                if (controller.GetParameterInfo(i, out var parameterInfo) == TResult.S_True) parameterIds.Add(parameterInfo.Id);
            }
            for (var bus = 0; bus < eventBusCount + 1; bus++)
            {
                if (interruptProcess) break;

                if (vstPlug.GetBusInfo(MediaTypes.Event, BusDirections.Input, bus, out var info) == TResult.S_True)
                {
                    if (bus >= eventBusCount)
                    {
                        testResult.AddMessage("getBusInfo supplied for an unknown event bus");
                        break;
                    }
                }
                else break;

                for (short channel = 0; channel < info.ChannelCount; channel++)
                {
                    if (interruptProcess) break;

                    var foundCount = 0;
                    // test with the cc outside the valid range too (>=kCountCtrlNumber)
                    for (ControllerNumbers cc = 0; cc < ControllerNumbers.CountCtrlNumber + 1; cc++)
                    {
                        var tag = 0U;
                        if (midiMapping.GetMidiControllerAssignment(bus, channel, cc, ref tag) == TResult.S_True)
                        {
                            if (bus >= eventBusCount)
                            {
                                testResult.AddMessage("MIDI Mapping supplied for an unknown event bus");
                                interruptProcess = true;
                                break;
                            }
                            if (cc >= ControllerNumbers.CountCtrlNumber)
                            {
                                testResult.AddMessage("MIDI Mapping supplied for a wrong ControllerNumbers value (bigger than the max)");
                                break;
                            }
                            if (!parameterIds.Contains(tag))
                            {
                                testResult.AddErrorMessage($"Unknown ParamID [{tag}] returned for MIDI Mapping");
                                return false;
                            }
                            foundCount++;
                        }
                        else
                        {
                            if (bus >= eventBusCount) interruptProcess = true;
                        }
                    }
                    if (foundCount == 0 && bus < eventBusCount)
                        testResult.AddMessage($"MIDI Mapping getMidiControllerAssignment ({bus}, {channel}) : no assignment available!");
                }
            }

            return true;
        }
    }
}
