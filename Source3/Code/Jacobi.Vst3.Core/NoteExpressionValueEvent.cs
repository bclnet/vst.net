using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// NoteExpressionTypeIDs describes the type of the note expression.
    /// VST predefines some types like volume, pan, tuning by defining their ranges and curves.
    /// Used by NoteExpressionEvent::typeId and NoteExpressionTypeID::typeId
    /// </summary>
    public enum NoteExpressionTypeIDs : uint
    {
        VolumeTypeID = 0,		// Volume, plain range [0 = -oo , 0.25 = 0dB, 0.5 = +6dB, 1 = +12dB]: plain = 20 * log (4 * norm)
        PanTypeID,				// Panning (L-R), plain range [0 = left, 0.5 = center, 1 = right]
        TuningTypeID,           // Tuning, plain range [0 = -120.0 (ten octaves down), 0.5 none, 1 = +120.0 (ten octaves up)]
                                // plain = 240 * (norm - 0.5) and norm = plain / 240 + 0.5
                                // oneOctave is 12.0 / 240.0; oneHalfTune = 1.0 / 240.0;
        VibratoTypeID,			// Vibrato
        ExpressionTypeID,		// Expression
        BrightnessTypeID,		// Brightness
        TextTypeID,			    // See NoteExpressionTextEvent
        PhonemeTypeID,			// TODO:

        CustomStart = 100000,	// start of custom note expression type ids
        CustomEnd = 200000,	    // end of custom note expression type ids

        InvalidTypeID = 0xffffffff // indicates an invalid note expression type
    }

    /// <summary>
    /// Note Expression Value event. Used in \ref Event (union)
    /// A note expression event affects one single playing note(referring its noteId).
    /// This kind of event is send from host to the plug-in like other events (NoteOnEvent, NoteOffEvent,...) in \ref ProcessData during the process call.
    /// Note expression events for a specific noteId can only occur after a NoteOnEvent. The host must take care that the event list (\ref IEventList) is properly sorted.
    /// Expression events are always absolute normalized values [0.0, 1.0].
    /// The predefined types have a predefined mapping of the normalized values (see \ref NoteExpressionTypeIDs)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct NoteExpressionValueEvent
    {
        public static readonly int Size = Marshal.SizeOf<NoteExpressionValueEvent>();

        [MarshalAs(UnmanagedType.U4)] public UInt32 TypeId;		    // see \ref NoteExpressionTypeID
        [MarshalAs(UnmanagedType.I4)] public Int32 NoteId;			// associated note identifier to apply the change	
        [MarshalAs(UnmanagedType.R8)] public Double Value;			// normalized value [0.0, 1.0].
    }
}