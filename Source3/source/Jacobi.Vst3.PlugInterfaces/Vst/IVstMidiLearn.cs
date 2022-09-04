using System;
using System.Runtime.InteropServices;

namespace Steinberg.Vst3
{
    /// <summary>
    /// MIDI Learn interface: Vst::IMidiLearn
    /// 
    /// If this interface is implemented by the edit controller, the host will call this method whenever
    /// there is live MIDI-CC input for the plug-in. This way, the plug-in can change its MIDI-CC parameter
    /// mapping and inform the host via the IComponentHandler::restartComponent with the
    /// kMidiCCAssignmentChanged flag.
    /// Use this if you want to implement custom MIDI-Learn functionality in your plug-in.
    /// </summary>
    [ComImport, Guid(Interfaces.IMidiLearn), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMidiLearn
    {
        /// <summary>
        /// Called on live input MIDI-CC change associated to a given bus index and MIDI channel
        /// </summary>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <param name="midiControllerNumber"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult OnLiveMIDIControllerInput(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.I2), In] ControllerNumbers midiControllerNumber);
    }

    partial class Interfaces
    {
        public const string IMidiLearn = "6B2449CC-4197-40B5-AB3C-79DAC5FE5C86";
    }
}
