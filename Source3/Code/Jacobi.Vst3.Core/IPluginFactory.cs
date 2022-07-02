﻿using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Class factory that any Plug-in defines for creating class instances.
    /// - [plug imp]
    ///
    /// From the host's point of view a Plug-in module is a factory which can create 
    /// a certain kind of object(s). The interface IPluginFactory provides methods 
    /// to get information about the classes exported by the Plug-in and a 
    /// mechanism to create instances of these classes (that usually define the IPluginBase interface).
    /// </summary>
    [ComImport, Guid(Interfaces.IPluginFactory), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginFactory
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetFactoryInfo(
            [MarshalAs(UnmanagedType.Struct), In, Out] ref PFactoryInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 CountClasses();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetClassInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref PClassInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 CreateInstance(
            [In] ref Guid classId,
            [In] ref Guid interfaceId,
            [MarshalAs(UnmanagedType.SysInt, IidParameterIndex = 1), In, Out] ref IntPtr instance);
    }

    static partial class Interfaces
    {
        public const string IPluginFactory = "7A4D811C-5211-4A1F-AED9-D2EE0B43BF9F";
    }

    [ComImport, Guid(Interfaces.IPluginFactory2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginFactory2
    {

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetFactoryInfo(
            [MarshalAs(UnmanagedType.Struct)] ref PFactoryInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 CountClasses();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetClassInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct)] ref PClassInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 CreateInstance(
            [In] ref Guid classId,
            [In] ref Guid interfaceId,
            [MarshalAs(UnmanagedType.SysInt, IidParameterIndex = 1), In, Out] ref IntPtr instance);


        //---------------------------------------------------------------------


        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetClassInfo2(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct)] ref PClassInfo2 info);
    }

    static partial class Interfaces
    {
        public const string IPluginFactory2 = "0007B650-F24B-4C0B-A464-EDB9F00B2ABB";
    }

    [ComImport, Guid(Interfaces.IPluginFactory3), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginFactory3
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetFactoryInfo(
            [MarshalAs(UnmanagedType.Struct)] ref PFactoryInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 CountClasses();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetClassInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct)] ref PClassInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 CreateInstance(
            [In] ref Guid classId,
            [In] ref Guid interfaceId,
            [MarshalAs(UnmanagedType.SysInt, IidParameterIndex = 1), In, Out] ref IntPtr instance);


        //---------------------------------------------------------------------


        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetClassInfo2(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct)] ref PClassInfo2 info);


        //---------------------------------------------------------------------


        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetClassInfoUnicode(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct)] ref PClassInfoW info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetHostContext(
            [MarshalAs(UnmanagedType.IUnknown), In] Object context);
    }

    static partial class Interfaces
    {
        public const string IPluginFactory3 = "4555A2AB-C123-4E57-9B12-291036878931";
    }
}
