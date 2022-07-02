﻿using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IUpdateHandler), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUpdateHandler
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 AddDependent(
            [MarshalAs(UnmanagedType.IUnknown), In] Object obj,
            [MarshalAs(UnmanagedType.Interface), In] IDependent dependent);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 RemoveDependent(
            [MarshalAs(UnmanagedType.IUnknown), In] Object obj,
            [MarshalAs(UnmanagedType.Interface), In] IDependent dependent);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 TriggerUpdates(
            [MarshalAs(UnmanagedType.IUnknown), In] Object obj,
            [MarshalAs(UnmanagedType.I4), In] Int32 message);

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
