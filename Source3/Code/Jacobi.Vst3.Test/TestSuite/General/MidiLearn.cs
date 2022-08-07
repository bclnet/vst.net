﻿using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Hosting;

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

            if (midiLearn.OnLiveMIDIControllerInput(0, 0, ControllerNumbers.CtrlPan) != TResult.S_True)
                testResult.AddMessage("onLiveMIDIControllerInput do not return kResultTrue!");
            if (midiLearn.OnLiveMIDIControllerInput(0, 0, ControllerNumbers.CtrlVibratoDelay) != TResult.S_True)
                testResult.AddMessage("onLiveMIDIControllerInput do not return kResultTrue!");

            return true;
        }
    }
}
