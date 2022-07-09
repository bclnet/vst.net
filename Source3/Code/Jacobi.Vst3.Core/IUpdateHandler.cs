﻿using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Host implements dependency handling for plugins.
    /// Can be used between host-objects and the Plug-In or 
    /// inside the Plug-In to handle internal updates!
    /// </summary>
    [ComImport, Guid(Interfaces.IUpdateHandler), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUpdateHandler
    {
        /// <summary>
        /// Install update notification for given object. It is essential to
	    /// remove all dependencies again using 'removeDependent'! Dependencies
		/// are not removed automatically when the 'object' is released!
        /// </summary>
        /// <param name="obj">interface to object that sends change notifications</param>
        /// <param name="dependent">interface through which the update is passed</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 AddDependent(
            [MarshalAs(UnmanagedType.IUnknown), In] Object obj,
            [MarshalAs(UnmanagedType.Interface), In] IDependent dependent);

        /// <summary>
        /// Remove a previously installed dependency.
        /// </summary>
        /// <param name="obj">is the object that has changed</param>
        /// <param name="dependent">is a value of enum IDependent::ChangeMessage, usually  IDependent::kChanged - can be a private message as well(only known to sender and dependent)</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 RemoveDependent(
            [MarshalAs(UnmanagedType.IUnknown), In] Object obj,
            [MarshalAs(UnmanagedType.Interface), In] IDependent dependent);

        /// <summary>
        /// Inform all dependents, that object has changed.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 TriggerUpdates(
            [MarshalAs(UnmanagedType.IUnknown), In] Object obj,
            [MarshalAs(UnmanagedType.I4), In] Int32 message);

        /// <summary>
        /// Same as triggerUpdates, but delivered in idle (usefull to collect updates).
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 DeferUpdates(
            [MarshalAs(UnmanagedType.IUnknown), In] Object obj,
            [MarshalAs(UnmanagedType.I4), In] Int32 message);
    }

    static partial class Interfaces
    {
        public const string IUpdateHandler = "F5246D56-8654-4d60-B026-AFB57B697B37";
    }
}
