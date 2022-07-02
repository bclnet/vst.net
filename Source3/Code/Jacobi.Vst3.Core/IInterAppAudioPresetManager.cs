using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
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
