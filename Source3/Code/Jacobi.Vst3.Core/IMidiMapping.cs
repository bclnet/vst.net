using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// MIDI Mapping interface: Vst::IMidiMapping
    /// MIDI controllers are not transmitted directly to a VST component. MIDI as hardware protocol has
    /// restrictions that can be avoided in software.Controller data in particular come along with unclear
    /// and often ignored semantics.On top of this they can interfere with regular parameter automation and
    /// the host is unaware of what happens in the plug-in when passing MIDI controllers directly.
    /// </summary>
    [ComImport, Guid(Interfaces.IMidiMapping), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMidiMapping
    {
        /// <summary>
        /// Gets an (preferred) associated ParamID for a given Input Event Bus index, channel and MIDI Controller.
        /// </summary>
        /// <param name="busIndex">index of Input Event Bus</param>
        /// <param name="channel">channel of the bus</param>
        /// <param name="midiControllerNumber">see \ref ControllerNumbers for expected values (could be bigger than 127)</param>
        /// <param name="id">return the associated ParamID to the given midiControllerNumber</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetMidiControllerAssignment(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.I2), In] ControllerNumbers midiControllerNumber,
            [MarshalAs(UnmanagedType.U4), In, Out] ref UInt32 id);
    }

    internal static partial class Interfaces
    {
        public const string IMidiMapping = "DF0FF9F7-49B7-4669-B63A-B7327ADBF5E5";
    }
}
