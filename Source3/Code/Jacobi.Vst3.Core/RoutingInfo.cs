using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Routing Information:
    /// When the plug-in supports multiple I/O busses, a host may want to know how the busses are related. The
    /// relation of an event-input-channel to an audio-output-bus in particular is of interest to the host
    /// (in order to relate MIDI-tracks to audio-channels)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = Platform.StructurePack)]
    public struct RoutingInfo
    {
        public static readonly int Size = Marshal.SizeOf<RoutingInfo>();

        [MarshalAs(UnmanagedType.I4)] public MediaTypes MediaType;	// media type see \ref MediaTypes
        [MarshalAs(UnmanagedType.I4)] public Int32 BusIndex;		// bus index
        [MarshalAs(UnmanagedType.I4)] public Int32 Channel;			// channel (-1 for all channels)
    }
}
