using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Description of a chord.
    /// A chord is described with a key note, a root note and the
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct Chord
    {
        public static readonly int Size = Marshal.SizeOf<Chord>();

        [MarshalAs(UnmanagedType.U1)] public Byte KeyNote;		// key note in chord
        [MarshalAs(UnmanagedType.U1)] public Byte RootNote;     // lowest note in chord

        /// <summary>
        /// Bitmask of a chord.
        /// 1st bit set: minor second; 2nd bit set: major second, and so on.
        /// There is \b no bit for the keynote (root of the chord) because it is inherently always present.
        /// Examples:
        /// - XXXX 0000 0100 1000 (= 0x0048) -> major chord\n
        /// - XXXX 0000 0100 0100 (= 0x0044) -> minor chord\n
        /// - XXXX 0010 0100 0100 (= 0x0244) -> minor chord with minor seventh
        /// </summary>
        [MarshalAs(UnmanagedType.I2)] public Int16 ChordMask;

        public enum Masks
        {
            ChordMask = 0x0FFF,	    // mask for chordMask 
            ReservedMask = 0xF000	// reserved for future use
        }
    }
}
