using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Legacy MIDI CC Out event specific data. Used in \ref Event (union)
    /// This kind of event is reserved for generating MIDI CC as output event for kEvent Bus during the process call.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct LegacyMIDICCOutEvent
    {
        public static readonly int TypeSize = Marshal.SizeOf<DataEvent>();

        [MarshalAs(UnmanagedType.U1)] public Byte ControlNumber;	// see enum ControllerNumbers [0, 255]
        [MarshalAs(UnmanagedType.I1)] public SByte Channel;		    // channel index in event bus [0, 15]
        [MarshalAs(UnmanagedType.I1)] public SByte value;		    // value of Controller [0, 127]
        [MarshalAs(UnmanagedType.I1)] public SByte value2;		    // [0, 127] used for pitch bend (kPitchBend) and polyPressure (kCtrlPolyPressure)
    }
}
