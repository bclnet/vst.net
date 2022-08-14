using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Scale event specific data. Used in \ref Event (union)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct ScaleEvent
    {
        public static readonly int TypeSize = Marshal.SizeOf<ScaleEvent>();

        [MarshalAs(UnmanagedType.I2)] public Int16 Root;			// range [0, 127] = root Note/Transpose Factor
        [MarshalAs(UnmanagedType.I2)] public Int16 Mask;		    // Bit 0 =  C,  Bit 1 = C#, ... (0x5ab5 = Major Scale)
        [MarshalAs(UnmanagedType.U4)] public UInt32 TextLen;		// the number of characters (TChar) between the beginning of text and the terminating null character (without including the terminating null character itself)
        [MarshalAs(UnmanagedType.LPWStr)] public IntPtr Text;    	// UTF-16, null terminated, Hosts Scale Name
        public String TextX => Marshal.PtrToStringUni(Text, (int)TextLen);
    }
}
