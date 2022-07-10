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
        Int32 GetScreenSize(
            [MarshalAs(UnmanagedType.Struct), In, Out] ref ViewRect size,
            [MarshalAs(UnmanagedType.R4), Out] out Single scale);

        /// <summary>
        /// get status of connection
        /// </summary>
        /// <returns>kResultTrue if an Inter-App Audio connection is established</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 ConnectedToHost();

        /// <summary>
        /// switch to the host.
        /// </summary>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SwitchToHost();

        /// <summary>
        /// send a remote control event to the host
        /// </summary>
        /// <param name="evnt">event type, see AudioUnitRemoteControlEvent in the iOS SDK documentation for possible types</param>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SendRemoteControlEvent(
            [MarshalAs(UnmanagedType.U4), In] UInt32 evnt);

        /// <summary>
        /// ask for the host icon.
        /// </summary>
        /// <param name="icon">pointer to a CGImageRef</param>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetHostIcon(
            [MarshalAs(UnmanagedType.LPArray), In] IntPtr icon);

        /// <summary>
        /// schedule an event from the user interface thread
        /// </summary>
        /// <param name="evnt">the event to schedule</param>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 ScheduleEventFromUI(
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
        Int32 ShowSettingsView();
    }

    static partial class Interfaces
    {
        public const string IInterAppAudioHost = "0CE5743D-68DF-415E-AE28-5BD4E2CDC8FD";
    }
}
