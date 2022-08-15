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
    /// Description of a Note Expression Type
    /// This structure is part of the NoteExpressionTypeInfo structure, it describes for given NoteExpressionTypeID its default value
    /// (for example 0.5 for a kTuningTypeID(kIsBipolar: centered)), its minimum and maximum(for predefined NoteExpressionTypeID the full range is predefined too)
    /// and a stepCount when the given NoteExpressionTypeID is limited to discrete values(like on/off state).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct NoteExpressionValueDescription
    {
        public static readonly int Size = Marshal.SizeOf<NoteExpressionValueDescription>();

        [MarshalAs(UnmanagedType.R8)] public Double DefaultValue;		// default normalized value [0,1]
        [MarshalAs(UnmanagedType.R8)] public Double Minimum;			// minimum normalized value [0,1]
        [MarshalAs(UnmanagedType.R8)] public Double Maximum;			// maximum normalized value [0,1]
        [MarshalAs(UnmanagedType.I4)] public Int32 StepCount;			// number of discrete steps (0: continuous, 1: toggle, discrete value otherwise - see \ref vst3parameterIntro)
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

    /// <summary>
    /// Note Expression Text event. Used in Event (union)
    /// A Expression event affects one single playing note. \sa INoteExpressionController
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct NoteExpressionTextEvent
    {
        public static readonly int TypeSize = Marshal.SizeOf<NoteExpressionTextEvent>();

        [MarshalAs(UnmanagedType.U4)] public UInt32 TypeId;	                	// see \ref NoteExpressionTypeID (kTextTypeID or kPhoneticTypeID)
        [MarshalAs(UnmanagedType.I4)] public Int32 NoteId;						// associated note identifier to apply the change
        [MarshalAs(UnmanagedType.U4)] public UInt32 TextLen;					// number of bytes in text (includes null byte)
        [MarshalAs(UnmanagedType.LPWStr)] public IntPtr Text;    				// UTF-16, null terminated
        public String TextX => Marshal.PtrToStringUni(Text, (int)TextLen);
    }

    /// <summary>
    /// NoteExpressionTypeInfo is the structure describing a note expression supported by the plug-in.
    /// This structure is used by the method \ref INoteExpressionController::getNoteExpressionInfo.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct NoteExpressionTypeInfo
    {
        public static readonly int Size = Marshal.SizeOf<NoteExpressionTypeInfo>();

        [MarshalAs(UnmanagedType.U4)] public UInt32 TypeId;			            // unique identifier of this note Expression type
        [MarshalAs(UnmanagedType.LPWStr)] public String Title;					// note Expression type title (e.g. "Volume")
        [MarshalAs(UnmanagedType.LPWStr)] public String ShortTitle;				// note Expression type short title (e.g. "Vol")
        [MarshalAs(UnmanagedType.LPWStr)] public String Units;					// note Expression type unit (e.g. "dB")
        [MarshalAs(UnmanagedType.I4)] public Int32 UnitId;						// id of unit this NoteExpression belongs to (see \ref vst3UnitsIntro), in order to sort the note expression, it is possible to use unitId like for parameters. -1 means no unit used.
        [MarshalAs(UnmanagedType.Struct)] public NoteExpressionValueDescription ValueDesc;	// value description see \ref NoteExpressionValueDescription
        [MarshalAs(UnmanagedType.U4)] public UInt32 AssociatedParameterId;		// optional associated parameter ID (for mapping from note expression to global (using the parameter automation for example) and back). Only used when kAssociatedParameterIDValid is set in flags.
        [MarshalAs(UnmanagedType.I4)] public NoteExpressionTypeFlags Flags;		// NoteExpressionTypeFlags (see below)

        [Flags]
        public enum NoteExpressionTypeFlags
        {
            IsBipolar = 1 << 0,			            // event is bipolar (centered), otherwise unipolar
            IsOneShot = 1 << 1,			            // event occurs only one time for its associated note (at begin of the noteOn)
            IsAbsolute = 1 << 2,			        // This note expression will apply an absolute change to the sound (not relative (offset))
            AssociatedParameterIDValid = 1 << 3,    // indicates that the associatedParameterID is valid and could be used
        }
    }

    /// <summary>
    /// Extended plug-in interface IEditController for note expression event support: Vst::INoteExpressionController
    /// With this plug-in interface, the host can retrieve all necessary note expression information supported by the plug-in.
    /// Note expression information (\ref NoteExpressionTypeInfo) are specific for given channel and event bus.
    /// Note that there is only one NoteExpressionTypeID per given channel of an event bus.
    /// The method getNoteExpressionStringByValue allows conversion from a normalized value to a string representation
    /// and the getNoteExpressionValueByString method from a string to a normalized value.
    /// When the note expression state changes (for example when switching presets) the plug-in needs
    /// to inform the host about it via \ref IComponentHandler::restartComponent (kNoteExpressionChanged).
    /// </summary>
    [ComImport, Guid(Interfaces.INoteExpressionController), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface INoteExpressionController
    {
        /// <summary>
        /// Returns number of supported note change types for event bus index and channel.
        /// </summary>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetNoteExpressionCount(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel);

        /// <summary>
        /// Returns note change type info.
        /// </summary>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <param name="noteExpressionIndex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetNoteExpressionInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.I4), In] Int32 noteExpressionIndex,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref NoteExpressionTypeInfo info);

        /// <summary>
        /// Gets a user readable representation of the normalized note change value.
        /// </summary>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <param name="noteExpressionTypeId"></param>
        /// <param name="valueNormalized"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetNoteExpressionStringByValue(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.U4), In] UInt32 noteExpressionTypeId,
            [MarshalAs(UnmanagedType.R8), In] Double valueNormalized,
            [MarshalAs(UnmanagedType.LPWStr), In, Out] ref String str);

        /// <summary>
        /// Converts the user readable representation to the normalized note change value.
        /// </summary>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <param name="noteExpressionTypeID"></param>
        /// <param name="str"></param>
        /// <param name="valueNormalized"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetNoteExpressionValueByString(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.U4), In] UInt32 noteExpressionTypeID,
            [MarshalAs(UnmanagedType.LPWStr), In] String str,
            [MarshalAs(UnmanagedType.R8), In, Out] ref Double valueNormalized);
    }

    static partial class Interfaces
    {
        public const string INoteExpressionController = "B7F8F859-4123-4872-9116-95814F3721A3";
    }

    /// <summary>
    /// KeyswitchTypeIDs describes the type of a key switch
    /// </summary>
    public enum KeyswitchTypeIDs
    {
        NoteOnKeyswitchTypeID = 0,              // press before noteOn is played
        OnTheFlyKeyswitchTypeID,                // press while noteOn is played
        OnReleaseKeyswitchTypeID,               // press before entering release
        KeyRangeTypeID                          // key should be maintained pressed for playing
    }

    /// <summary>
    /// KeyswitchInfo is the structure describing a key switch
    /// This structure is used by the method \ref IKeyswitchController::getKeyswitchInfo.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct KeyswitchInfo
    {
        public static readonly int Size = Marshal.SizeOf<KeyswitchInfo>();

        [MarshalAs(UnmanagedType.U4)] public KeyswitchTypeIDs TypeId;				// see KeyswitchTypeID
        [MarshalAs(UnmanagedType.LPWStr)] public String Title;						// name of key switch (e.g. "Accentuation")
        [MarshalAs(UnmanagedType.LPWStr)] public String ShortTitle;					// short title (e.g. "Acc")
        [MarshalAs(UnmanagedType.I4)] public Int32 KeyswitchMin;					// associated main key switch min (value between [0, 127])
        [MarshalAs(UnmanagedType.I4)] public Int32 KeyswitchMax;					// associated main key switch max (value between [0, 127])
        [MarshalAs(UnmanagedType.I4)] public Int32 KeyRemapped;						// optional remapped key switch (default -1), the Plug-in could provide one remapped key for a key switch (allowing better location on the keyboard of the key switches)
        [MarshalAs(UnmanagedType.I4)] public Int32 UnitId;							// id of unit this key switch belongs to (see \ref vst3UnitsIntro), -1 means no unit used.
        [MarshalAs(UnmanagedType.I4)] public KeyswitchFlags Flags;                  // not yet used (set to 0)

        public enum KeyswitchFlags
        {
            None = 0
        }
    }

    /// <summary>
    /// Extended plug-in interface IEditController for key switches support: Vst::IKeyswitchController
    /// 
    /// When a (instrument) plug-in supports such interface, the host could get from the plug-in the current set
    /// of used key switches(megatrig/articulation) for a given channel of a event bus and then automatically use them(like in Cubase 6) to
    /// create VST Expression Map(allowing to associated symbol to a given articulation / key switch).
    /// </summary>
    [ComImport, Guid(Interfaces.IKeyswitchController), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IKeyswitchController
    {
        /// <summary>
        /// Returns number of supported key switches for event bus index and channel.
        /// </summary>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetKeyswitchCount(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel);

        /// <summary>
        /// Returns key switch info.
        /// </summary>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <param name="keySwitchIndex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetKeyswitchInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.I4), In] Int32 keySwitchIndex,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref KeyswitchInfo info);
    }

    static partial class Interfaces
    {
        public const string IKeyswitchController = "1F2F76D3-BFFB-4B96-B995-27A55EBCCEF4";
    }
}