using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Processing buffers of an audio bus.
    /// This structure contains the processing buffer for each channel of an audio bus.
    /// - The number of channels (numChannels) must always match the current bus arrangement.
    ///   It could be set to value '0' when the host wants to flush the parameters (when the plug-in is not processed).
    /// - The size of the channel buffer array must always match the number of channels. So the host
    ///   must always supply an array for the channel buffers, regardless if the
    ///   bus is active or not. However, if an audio bus is currently inactive, the actual sample
    ///   buffer addresses are safe to be null.
    /// - The silence flag is set when every sample of the according buffer has the value '0'. It is
    ///   intended to be used as help for optimizations allowing a plug-in to reduce processing activities.
    ///   But even if this flag is set for a channel, the channel buffers must still point to valid memory!
    ///   This flag is optional. A host is free to support it or not.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct AudioBusBuffers
    {
        public static readonly int Size = Marshal.SizeOf<AudioBusBuffers>();

        [FieldOffset(FieldOffset_NumChannels), MarshalAs(UnmanagedType.I4)] public Int32 NumChannels;		// number of audio channels in bus
        [FieldOffset(FieldOffset_SilenceFlags), MarshalAs(UnmanagedType.U8)] public UInt64 SilenceFlags;	// Bitset of silence state per channel

        // Single** pointer to an array Single[NumChannels][NumSamples]
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.SysInt)] public IntPtr ChannelBuffers32; // sample buffers to process with 32-bit precision
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.SysInt)] public unsafe Single** ChannelBuffers32_; // sample buffers to process with 32-bit precision (unsafe)

        // Double** pointer to an array Double[NumChannels][NumSamples]
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.SysInt)] public IntPtr ChannelBuffers64; // sample buffers to process with 64-bit precision
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.SysInt)] public unsafe Double** ChannelBuffers64_; // sample buffers to process with 64-bit precision (unsafe)

#if X86
        internal const int FieldOffset_NumChannels = 0;
        internal const int FieldOffset_SilenceFlags = 4;
        internal const int FieldOffset_Union = 12;
#endif
#if X64
        internal const int FieldOffset_NumChannels = 0;
        internal const int FieldOffset_SilenceFlags = 8;
        internal const int FieldOffset_Union = 16;
#endif
    }
}
