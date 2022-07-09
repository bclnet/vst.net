using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Chord event specific data. Used in \ref Event (union)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct ChordEvent
    {
        public static readonly int TypeSize = Marshal.SizeOf<ChordEvent>();

        [MarshalAs(UnmanagedType.I2)] public Int16 Root;			// range [0, 127] = [C-2, G8] with A3=440Hz
        [MarshalAs(UnmanagedType.I2)] public Int16 BassNote;		// range [0, 127] = [C-2, G8] with A3=440Hz
        [MarshalAs(UnmanagedType.I2)] public Int16 Mask;		    // root is bit 0
        [MarshalAs(UnmanagedType.U4)] public UInt32 TextLen;		// the number of characters (TChar) between the beginning of text and the terminating null character (without including the terminating null character itself)
        [MarshalAs(UnmanagedType.LPWStr)] public String Text;    	// UTF-16, null terminated Hosts Chord Name
    }
}
