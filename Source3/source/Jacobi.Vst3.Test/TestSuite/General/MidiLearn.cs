using Jacobi.Vst3;
using Jacobi.Vst3.Hosting;
using static Jacobi.Vst3.TResult;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test MIDI Learn.
    /// </summary>
    public class MidiLearnTest : TestBase
    {
        public override string Name => "MIDI Learn";

        public MidiLearnTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            if (controller == null)
            {
                testResult.AddMessage("No Edit Controller supplied!");
                return true;
            }

            if (controller is not IMidiLearn midiLearn)
            {
                testResult.AddMessage("No MIDI Learn interface supplied!");
                return true;
            }

            if (midiLearn.OnLiveMIDIControllerInput(0, 0, ControllerNumbers.CtrlPan) != kResultTrue)
                testResult.AddMessage("onLiveMIDIControllerInput do not return kResultTrue!");
            if (midiLearn.OnLiveMIDIControllerInput(0, 0, ControllerNumbers.CtrlVibratoDelay) != kResultTrue)
                testResult.AddMessage("onLiveMIDIControllerInput do not return kResultTrue!");

            return true;
        }
    }
}
