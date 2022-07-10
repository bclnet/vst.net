using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
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
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetEventCount();

        /// <summary>
        /// Gets parameter by index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetEvent(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out Event e);

        /// <summary>
        /// Adds a new event.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 AddEvent(
            [MarshalAs(UnmanagedType.Struct), In] ref Event e);
    }

    internal static partial class Interfaces
    {
        public const string IEventList = "3A2C4214-3463-49FE-B2C4-F397B9695A44";
    }
}
