using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
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
        Int32 GetNoteExpressionInfo(
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
        Int32 GetNoteExpressionStringByValue(
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
        Int32 GetNoteExpressionValueByString(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.U4), In] UInt32 noteExpressionTypeID,
            [MarshalAs(UnmanagedType.LPWStr), In] String str,
            [MarshalAs(UnmanagedType.R8), In, Out] ref Double valueNormalized);
    }

    internal static partial class Interfaces
    {
        public const string INoteExpressionController = "B7F8F859-4123-4872-9116-95814F3721A3";
    }
}
