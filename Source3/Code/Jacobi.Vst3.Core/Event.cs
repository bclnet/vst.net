using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
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
    /// Event
    /// Structure representing a single Event of different types associated to a specific event (\ref kEvent) bus.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct Event
    {
        public static readonly int Size = Marshal.SizeOf<Event>();

        [FieldOffset(FieldOffset_BusIndex), MarshalAs(UnmanagedType.I4)] public Int32 BusIndex;			// event bus index
        [FieldOffset(FieldOffset_SampleOffset), MarshalAs(UnmanagedType.I4)] public Int32 SampleOffset;	// sample frames related to the current block start sample position
        [FieldOffset(FieldOffset_PpqPosition), MarshalAs(UnmanagedType.R8)] public Double PpqPosition;	// position in project
        [FieldOffset(FieldOffset_Flags), MarshalAs(UnmanagedType.I4)] public EventFlags Flags;		    // combination of \ref EventFlags
        [FieldOffset(FieldOffset_Type), MarshalAs(UnmanagedType.I4)] public EventTypes Type;			// a value from \ref EventTypes

        // union
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public NoteOnEvent NoteOn;                            // type == NoteOnEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public NoteOffEvent NoteOff;							// type == NoteOffEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public DataEvent Data;								// type == DataEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public PolyPressureEvent PolyPressure;				// type == PolyPressureEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public NoteExpressionValueEvent NoteExpressionValue;	// type == NoteExpressionValueEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public NoteExpressionTextEvent NoteExpressionText;	// type == NoteExpressionTextEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public ChordEvent Chord; /*unbound*/                  // type == ChordEvent
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.Struct)] public ScaleEvent Scale; /*unbound*/	                // type == ScaleEvent
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
        internal const int FieldOffset_BusIndex = 0;
        internal const int FieldOffset_SampleOffset = 4;
        internal const int FieldOffset_PpqPosition = 8;
        internal const int FieldOffset_Flags = 16;
        internal const int FieldOffset_Type = 20;
        internal const int FieldOffset_Union = 24;
#endif
#if X64
        internal const int FieldOffset_BusIndex = 0;
        internal const int FieldOffset_SampleOffset = 8;
        internal const int FieldOffset_PpqPosition = 16;
        internal const int FieldOffset_Flags = 24;
        internal const int FieldOffset_Type = 32;
        internal const int FieldOffset_Union = 40;
#endif

    }
}
