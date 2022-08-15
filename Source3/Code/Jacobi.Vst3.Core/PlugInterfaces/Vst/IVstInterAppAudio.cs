using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Inter-App Audio host Interface.
    /// Implemented by the InterAppAudio Wrapper.
    /// </summary>
    [ComImport, Guid(Interfaces.IInterAppAudioHost), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInterAppAudioHost
    {
        /// <summary>
        /// get the size of the screen
        /// </summary>
        /// <param name="size">size of the screen</param>
        /// <param name="scale">scale of the screen</param>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetScreenSize(
            [MarshalAs(UnmanagedType.Struct), In, Out] ref ViewRect size,
            [MarshalAs(UnmanagedType.R4), Out] out Single scale);

        /// <summary>
        /// get status of connection
        /// </summary>
        /// <returns>kResultTrue if an Inter-App Audio connection is established</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult ConnectedToHost();

        /// <summary>
        /// switch to the host.
        /// </summary>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SwitchToHost();

        /// <summary>
        /// send a remote control event to the host
        /// </summary>
        /// <param name="evnt">event type, see AudioUnitRemoteControlEvent in the iOS SDK documentation for possible types</param>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SendRemoteControlEvent(
            [MarshalAs(UnmanagedType.U4), In] UInt32 evnt);

        /// <summary>
        /// ask for the host icon.
        /// </summary>
        /// <param name="icon">pointer to a CGImageRef</param>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetHostIcon(
            [MarshalAs(UnmanagedType.LPArray), In] IntPtr icon);

        /// <summary>
        /// schedule an event from the user interface thread
        /// </summary>
        /// <param name="evnt">the event to schedule</param>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult ScheduleEventFromUI(
            [MarshalAs(UnmanagedType.Struct), In, Out] ref Event evnt);

        /// <summary>
        /// get the preset manager
        /// </summary>
        /// <param name="cid">class ID to use by the preset manager</param>
        /// <returns>the preset manager. Needs to be released by called.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        IInterAppAudioPresetManager CreatePresetManager(
            [MarshalAs(UnmanagedType.U8), In, Out] ref Guid cid);

        /// <summary>
        /// show the settings view
        /// </summary>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult ShowSettingsView();
    }

    static partial class Interfaces
    {
        public const string IInterAppAudioHost = "0CE5743D-68DF-415E-AE28-5BD4E2CDC8FD";
    }

    /// <summary>
    /// Extended plug-in interface IEditController for Inter-App Audio connection state change notifications
    /// </summary>
    [ComImport, Guid(Interfaces.IInterAppAudioConnectionNotification), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInterAppAudioConnectionNotification
    {
        /// <summary>
        /// called when the Inter-App Audio connection state changes
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        void OnInterAppAudioConnectionStateChange(
            [MarshalAs(UnmanagedType.U1), In] Boolean newState);
    }

    static partial class Interfaces
    {
        public const string IInterAppAudioConnectionNotification = "6020C72D-5FC2-4AA1-B095-0DB5D7D6D5CF";
    }

    /// <summary>
    /// Extended plug-in interface IEditController for Inter-App Audio Preset Management
    /// </summary>
    [ComImport, Guid(Interfaces.IInterAppAudioPresetManager), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInterAppAudioPresetManager
    {
        /// <summary>
        /// Open the Preset Browser in order to load a preset
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 RunLoadPresetBrowser();

        /// <summary>
        /// Open the Preset Browser in order to save a preset
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 RunSavePresetBrowser();

        /// <summary>
        /// Load the next available preset
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 LoadNextPreset();

        /// <summary>
        /// Load the previous available preset
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 LoadPreviousPreset();
    }

    static partial class Interfaces
    {
        public const string IInterAppAudioPresetManager = "ADE6FCC4-46C9-4E1D-B3B4-9A80C93FEFDD";
    }
}
