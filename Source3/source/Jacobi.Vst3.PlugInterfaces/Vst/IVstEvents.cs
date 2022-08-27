using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3
{
    /// <summary>
    /// Reserved note identifier (noteId) range for a plug-in. Guaranteed not used by the host. */
    /// </summary>
    public enum NoteIDUserRange
    {
        NoteIDUserRangeLowerBound = -10000,
        NoteIDUserRangeUpperBound = -1000,
    }

    /// <summary>
    /// Note-on event specific data. Used in \ref Event (union)
    /// Pitch uses the twelve-tone equal temperament tuning (12-TET).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct NoteOnEvent
    {
        public static readonly int Size = Marshal.SizeOf<NoteOnEvent>();

        [MarshalAs(UnmanagedType.I2)] public Int16 Channel;			// channel index in event bus
        [MarshalAs(UnmanagedType.I2)] public Int16 Pitch;			// range [0, 127] = [C-2, G8] with A3=440Hz
        [MarshalAs(UnmanagedType.R4)] public Single Tuning;			// 1.f = +1 cent, -1.f = -1 cent 
        [MarshalAs(UnmanagedType.R4)] public Single Velocity;		// range [0.0, 1.0]
        [MarshalAs(UnmanagedType.I4)] public Int32 Length;          // in sample frames (optional, Note Off has to follow in any case!)
        [MarshalAs(UnmanagedType.I4)] public Int32 NoteId;			// note identifier (if not available then -1)
    }

    /// <summary>
    /// Note-off event specific data. Used in \ref Event (union)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct NoteOffEvent
    {
        public static readonly int Size = Marshal.SizeOf<NoteOffEvent>();

        [MarshalAs(UnmanagedType.I2)] public Int16 Channel;			// channel index in event bus
        [MarshalAs(UnmanagedType.I2)] public Int16 Pitch;			// range [0, 127] = [C-2, G8] with A3=440Hz
        [MarshalAs(UnmanagedType.R4)] public Single Velocity;		// range [0.0, 1.0]
        [MarshalAs(UnmanagedType.I4)] public Int32 NoteId;			// associated noteOn identifier (if not available then -1)
        [MarshalAs(UnmanagedType.R4)] public Single Tuning;			// 1.f = +1 cent, -1.f = -1 cent 
    }

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

    /// <summary>
    /// PolyPressure event specific data. Used in \ref Event (union)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct PolyPressureEvent
    {
        public static readonly int Size = Marshal.SizeOf<PolyPressureEvent>();

        [MarshalAs(UnmanagedType.I2)] public Int16 Channel;			// channel index in event bus
        [MarshalAs(UnmanagedType.I2)] public Int16 Pitch;			// range [0, 127] = [C-2, G8] with A3=440Hz
        [MarshalAs(UnmanagedType.R4)] public Single Pressure;		// range [0.0, 1.0]
        [MarshalAs(UnmanagedType.I4)] public Int32 NoteId;		    // event should be applied to the noteId (if not -1)
    }

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
        [MarshalAs(UnmanagedType.LPWStr)] public IntPtr Text;    	// UTF-16, null terminated Hosts Chord Name
        public String TextX => Marshal.PtrToStringUni(Text, (int)TextLen);
    }

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

    /// <summary>
    /// Event
    /// Structure representing a single Event of different types associated to a specific event (\ref kEvent) bus.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct Event
    {
        public static readonly int Size = Marshal.SizeOf<Event>();

        [FieldOffset(FieldOffset_BusIndex), MarshalAs(UnmanagedType.I4)] public int BusIndex;			// event bus index
        [FieldOffset(FieldOffset_SampleOffset), MarshalAs(UnmanagedType.I4)] public int SampleOffset;	// sample frames related to the current block start sample position
        [FieldOffset(FieldOffset_PpqPosition), MarshalAs(UnmanagedType.R8)] public double PpqPosition;	// position in project
        [FieldOffset(FieldOffset_Flags), MarshalAs(UnmanagedType.I4)] public EventFlags Flags;		    // combination of \ref EventFlags
        [FieldOffset(FieldOffset_Type), MarshalAs(UnmanagedType.I4)] public EventTypes Type;			// a value from \ref EventTypes

        // union
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public NoteOnEvent NoteOn;                            // type == NoteOnEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public NoteOffEvent NoteOff;							// type == NoteOffEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public DataEvent Data;								// type == DataEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public PolyPressureEvent PolyPressure;				// type == PolyPressureEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public NoteExpressionValueEvent NoteExpressionValue;	// type == NoteExpressionValueEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public NoteExpressionTextEvent NoteExpressionText; /*unbound*/ // type == NoteExpressionTextEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public ChordEvent Chord; /*unbound*/                // type == ChordEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public ScaleEvent Scale; /*unbound*/	            // type == ScaleEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public LegacyMIDICCOutEvent MidiCCOut;	            // type == LegacyMIDICCOutEvent

        public enum EventFlags
        {
            IsLive = 1 << 0,			    // indicates that the event is played live (directly from keyboard)

            UserReserved1 = 1 << 14,	    // reserved for user (for internal use)
            UserReserved2 = 1 << 15	        // reserved for user (for internal use)
        }

        public enum EventTypes
        {
            NoteOnEvent = 0,                // is \ref NoteOnEvent
            NoteOffEvent = 1,               // is \ref NoteOffEvent
            DataEvent = 2,                  // is \ref DataEvent
            PolyPressureEvent = 3,          // is \ref PolyPressureEvent
            NoteExpressionValueEvent = 4,   // is \ref NoteExpressionValueEvent
            NoteExpressionTextEvent = 5,    // is \ref NoteExpressionTextEvent
            ChordEvent = 6,                 // is \ref ChordEvent
            ScaleEvent = 7,                 // is \ref NoteExpressionTextEvent
            LegacyMIDICCOutEvent = 65535    // is \ref NoteExpressionTextEvent
        }

#if X86
        const int FieldOffset_BusIndex = 0;
        const int FieldOffset_SampleOffset = 4;
        const int FieldOffset_PpqPosition = 8;
        const int FieldOffset_Flags = 16;
        const int FieldOffset_Type = 20;
        const int FieldOffset_Union = 24;
#endif
#if X64
        const int FieldOffset_BusIndex = 0;
        const int FieldOffset_SampleOffset = 8;
        const int FieldOffset_PpqPosition = 16;
        const int FieldOffset_Flags = 24;
        const int FieldOffset_Type = 32;
        const int FieldOffset_Union = 40;
#endif

    }

    /// <summary>
    /// List of events to process: Vst::IEventList
    /// </summary>
    [ComImport, Guid(Interfaces.IEventList), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEventList
    {
        /// <summary>
        /// Returns the count of events.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        Int32 GetEventCount();

        /// <summary>
        /// Gets parameter by index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetEvent(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out Event e);

        /// <summary>
        /// Adds a new event.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult AddEvent(
            [MarshalAs(UnmanagedType.Struct), In] ref Event e);
    }

    partial class Interfaces
    {
        public const string IEventList = "3A2C4214-3463-49FE-B2C4-F397B9695A44";
    }
}
