using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Data event specific data. Used in \ref Event (union)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct DataEvent
    {
        public static readonly int TypeSize = Marshal.SizeOf<DataEvent>();

        [MarshalAs(UnmanagedType.U4)] public UInt32 Size;			// size of the bytes
        [MarshalAs(UnmanagedType.U4)] public DataTypes Type;		// type of this data block (see \ref DataTypes)
        [MarshalAs(UnmanagedType.SysInt)] public IntPtr Bytes;		// pointer to the data block

        /// <summary>
        /// Value for DataEvent::type
        /// </summary>
        public enum DataTypes
        {
            MidiSysEx = 0		// for MIDI system exclusive message
        }
    }
}
