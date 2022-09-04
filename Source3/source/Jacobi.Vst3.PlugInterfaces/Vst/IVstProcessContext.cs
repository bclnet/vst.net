using System;
using System.Runtime.InteropServices;

namespace Steinberg.Vst3
{
    /// <summary>
    /// Frame Rate 
    /// A frame rate describes the number of image(frame) displayed per second.
    /// Some examples:
	/// - 23.976 fps     is framesPerSecond: 24 and flags: kPullDownRate
	/// - 24 fps         is framesPerSecond: 24 and flags: 0
	/// - 25 fps         is framesPerSecond: 25 and flags: 0
	/// - 29.97 drop fps is framesPerSecond: 30 and flags: kDropRate|kPullDownRate
	/// - 29.97 fps      is framesPerSecond: 30 and flags: kPullDownRate
	/// - 30 fps         is framesPerSecond: 30 and flags: 0
	/// - 30 drop fps    is framesPerSecond: 30 and flags: kDropRate
	/// - 50 fps         is framesPerSecond: 50 and flags: 0
	/// - 59.94 fps	     is framesPerSecond: 60 and flags: kPullDownRate
	/// - 60 fps         is framesPerSecond: 60 and flags: 0
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct FrameRate
    {
        [MarshalAs(UnmanagedType.U4)] public UInt32 framesPerSecond;	// frame rate
        [MarshalAs(UnmanagedType.U4)] public FrameRateFlags flags;    	// flags #FrameRateFlags

        public enum FrameRateFlags
        {
            PullDownRate = 1 << 0,      // for ex. HDTV: 23.976 fps with 24 as frame rate
            DropRate = 1 << 1	        // for ex. 29.97 fps drop with 30 as frame rate
        }
    }

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

    /// <summary>
    /// Audio processing context.
    /// For each processing block the host provides timing information and musical parameters that can change over time.For a host that supports jumps(like cycle) it is possible to split up a
    /// processing block into multiple parts in order to provide a correct project time inside of every block, but this behavior is not mandatory. Since the timing will be correct at the beginning of the
    /// next block again, a host that is dependent on a fixed processing block size can choose to neglect this problem.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = Platform.StructurePack)]
    public struct ProcessContext
    {
        public static readonly int Size = Marshal.SizeOf<ProcessContext>();

        [MarshalAs(UnmanagedType.U4)] public UInt32 State;                  // a combination of the values from \ref StatesAndFlags

        [MarshalAs(UnmanagedType.R8)] public Double SampleRate;				// current sample rate (always valid)
        [MarshalAs(UnmanagedType.I8)] public Int64 ProjectTimeSamples;      // project time in samples (always valid)

        [MarshalAs(UnmanagedType.I8)] public Int64 SystemTime;				// system time in nanoseconds (optional)
        [MarshalAs(UnmanagedType.I8)] public Int64 ContinousTimeSamples;    // project time, without loop (optional)

        [MarshalAs(UnmanagedType.R8)] public Double ProjectTimeMusic;	    // musical position in quarter notes (1.0 equals 1 quarter note)
        [MarshalAs(UnmanagedType.R8)] public Double BarPositionMusic;	    // last bar start position, in quarter notes
        [MarshalAs(UnmanagedType.R8)] public Double CycleStartMusic;	    // cycle start in quarter notes
        [MarshalAs(UnmanagedType.R8)] public Double CycleEndMusic;          // cycle end in quarter notes

        [MarshalAs(UnmanagedType.R8)] public Double Tempo;					// tempo in BPM (Beats Per Minute)
        [MarshalAs(UnmanagedType.I4)] public Int32 TimeSigNumerator;		// time signature numerator (e.g. 3 for 3/4)
        [MarshalAs(UnmanagedType.I4)] public Int32 TimeSigDenominator;      // time signature denominator (e.g. 4 for 3/4)

        [MarshalAs(UnmanagedType.Struct)] public Chord Chord;               // musical info

        [MarshalAs(UnmanagedType.I4)] public Int32 SmpteOffsetSubframes;	// SMPTE (sync) offset in subframes (1/80 of frame)
        [MarshalAs(UnmanagedType.Struct)] public FrameRate FrameRate;		// frame rate

        [MarshalAs(UnmanagedType.I4)] public Int32 SamplesToNextClock;		// MIDI Clock Resolution (24 Per Quarter Note), can be negative (nearest)

        /// <summary>
        /// Transport state & other flags
        /// </summary>
        public enum StatesAndFlags
        {
            Playing = 1 << 1,		        // currently playing
            CycleActive = 1 << 2,		    // cycle is active
            Recording = 1 << 3,		        // currently recording

            SystemTimeValid = 1 << 8,		// systemTime contains valid information
            ContTimeValid = 1 << 17,	    // continousTimeSamples contains valid information

            ProjectTimeMusicValid = 1 << 9, // projectTimeMusic contains valid information
            BarPositionValid = 1 << 11,	    // barPositionMusic contains valid information
            CycleValid = 1 << 12,	        // cycleStartMusic and barPositionMusic contain valid information

            TempoValid = 1 << 10,	        // tempo contains valid information
            TimeSigValid = 1 << 13,	        // timeSigNumerator and timeSigDenominator contain valid information
            ChordValid = 1 << 18,	        // chord contains valid information

            SmpteValid = 1 << 14,	        // smpteOffset and frameRate contain valid information
            ClockValid = 1 << 15		    // samplesToNextClock valid		
        }
    }
}
