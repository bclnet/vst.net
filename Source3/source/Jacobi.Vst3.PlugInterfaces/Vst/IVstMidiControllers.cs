namespace Jacobi.Vst3
{
    /// <summary>
    /// Controller Numbers (MIDI)
    /// </summary>
    public enum ControllerNumbers : short
    {
		CtrlBankSelectMSB = 0,		// Bank Select MSB
		CtrlModWheel = 1,			// Modulation Wheel
		CtrlBreath = 2,				// Breath controller

		CtrlFoot = 4,				// Foot Controller
		CtrlPortaTime = 5,			// Portamento Time
		CtrlDataEntryMSB = 6,		// Data Entry MSB
		CtrlVolume = 7,				// Channel Volume (formerly Main Volume)
		CtrlBalance = 8,			// Balance

		CtrlPan = 10,				// Pan
		CtrlExpression = 11,		// Expression
		CtrlEffect1 = 12,			// Effect Control 1
		CtrlEffect2 = 13,			// Effect Control 2

		//---General Purpose Controllers #1 to #4---
		CtrlGPC1 = 16,				// General Purpose Controller #1
		CtrlGPC2 = 17,				// General Purpose Controller #2
		CtrlGPC3 = 18,				// General Purpose Controller #3
		CtrlGPC4 = 19,				// General Purpose Controller #4

		CtrlBankSelectLSB = 32,		// Bank Select LSB

		CtrlDataEntryLSB = 38,		// Data Entry LSB

		CtrlSustainOnOff = 64,		// Damper Pedal On/Off (Sustain)
		CtrlPortaOnOff = 65,		// Portamento On/Off
		CtrlSustenutoOnOff = 66,	// Sustenuto On/Off
		CtrlSoftPedalOnOff = 67,	// Soft Pedal On/Off
		CtrlLegatoFootSwOnOff = 68, // Legato Footswitch On/Off
		CtrlHold2OnOff = 69,		// Hold 2 On/Off

		//---Sound Controllers #1 to #10---
		CtrlSoundVariation = 70,	// Sound Variation
		CtrlFilterCutoff = 71,		// Filter Cutoff (Timbre/Harmonic Intensity)
		CtrlReleaseTime = 72,		// Release Time
		CtrlAttackTime = 73,		// Attack Time
		CtrlFilterResonance = 74,	// Filter Resonance (Brightness)
		CtrlDecayTime = 75,			// Decay Time
		CtrlVibratoRate = 76,		// Vibrato Rate
		CtrlVibratoDepth = 77,		// Vibrato Depth
		CtrlVibratoDelay = 78,		// Vibrato Delay
		CtrlSoundCtrler10 = 79,		// undefined

		//---General Purpose Controllers #5 to #8---
		CtrlGPC5 = 80,				// General Purpose Controller #5
		CtrlGPC6 = 81,				// General Purpose Controller #6
		CtrlGPC7 = 82,				// General Purpose Controller #7
		CtrlGPC8 = 83,				// General Purpose Controller #8

		CtrlPortaControl = 84,		// Portamento Control

		//---Effect Controllers---
		CtrlEff1Depth = 91,			// Effect 1 Depth (Reverb Send Level)
		CtrlEff2Depth = 92,			// Effect 2 Depth (Tremolo Level)
		CtrlEff3Depth = 93,			// Effect 3 Depth (Chorus Send Level)
		CtrlEff4Depth = 94,			// Effect 4 Depth (Delay/Variation/Detune Level)
		CtrlEff5Depth = 95,			// Effect 5 Depth (Phaser Level)

		CtrlDataIncrement = 96,		// Data Increment (+1)
		CtrlDataDecrement = 97,		// Data Decrement (-1)
		CtrlNRPNSelectLSB = 98,		// NRPN Select LSB
		CtrlNRPNSelectMSB = 99,		// NRPN Select MSB
		CtrlRPNSelectLSB = 100,		// RPN Select LSB
		CtrlRPNSelectMSB = 101,		// RPN Select MSB

		//---Other Channel Mode Messages---
		CtrlAllSoundsOff = 120,		// All Sounds Off
		CtrlResetAllCtrlers = 121,	// Reset All Controllers
		CtrlLocalCtrlOnOff = 122,	// Local Control On/Off
		CtrlAllNotesOff = 123,		// All Notes Off
		CtrlOmniModeOff = 124,		// Omni Mode Off + All Notes Off
		CtrlOmniModeOn = 125,		// Omni Mode On  + All Notes Off
		CtrlPolyModeOnOff = 126,	// Poly Mode On/Off + All Sounds Off
		CtrlPolyModeOn = 127,		// Poly Mode On

		//---Extra--------------------------
		AfterTouch = 128,          // After Touch (associated to Channel Pressure)
		PitchBend = 129,           // Pitch Bend Change

		CountCtrlNumber,           // Count of Controller Number

		//---Extra for kLegacyMIDICCOutEvent-
		CtrlProgramChange = 130,	// Program Change (use LegacyMIDICCOutEvent.value only)
		CtrlPolyPressure = 131,		// Polyphonic Key Pressure (use LegacyMIDICCOutEvent.value for pitch and LegacyMIDICCOutEvent.value2 for pressure)
		CtrlQuarterFrame = 132		// Quarter Frame ((use LegacyMIDICCOutEvent.value only)
	}
}
