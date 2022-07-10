using System;
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
        /// <summary>
        /// Fill a PFactoryInfo structure with information about the plug-in vendor.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetFactoryInfo(
            [MarshalAs(UnmanagedType.Struct), Out] out PFactoryInfo info);

        /// <summary>
        /// Returns the number of exported classes by this factory. If you are using the CPluginFactory
        /// implementation provided by the SDK, it returns the number of classes you registered with
        /// CPluginFactory::registerClass.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 CountClasses();

        /// <summary>
        /// Fill a PClassInfo structure with information about the class at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetClassInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out PClassInfo info);

        /// <summary>
        /// Create a new class instance.
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="interfaceId"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 CreateInstance(
            [MarshalAs(UnmanagedType.Struct), In] ref Guid classId,
            [MarshalAs(UnmanagedType.Struct), In] ref Guid interfaceId,
            [MarshalAs(UnmanagedType.SysInt, IidParameterIndex = 1), Out] out IntPtr instance);
    }

    static partial class Interfaces
    {
        public const string IPluginFactory = "7A4D811C-5211-4A1F-AED9-D2EE0B43BF9F";
    }

    /// <summary>
    /// Version 2 of class factory supporting PClassInfo2: IPluginFactory2
    /// </summary>
    [ComImport, Guid(Interfaces.IPluginFactory2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginFactory2 : IPluginFactory
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 GetFactoryInfo(
            [MarshalAs(UnmanagedType.Struct), Out] out PFactoryInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        new Int32 CountClasses();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 GetClassInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out PClassInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 CreateInstance(
            [MarshalAs(UnmanagedType.Struct), In] ref Guid classId,
            [MarshalAs(UnmanagedType.Struct), In] ref Guid interfaceId,
            [MarshalAs(UnmanagedType.SysInt, IidParameterIndex = 1), Out] out IntPtr instance);

        //---------------------------------------------------------------------

        /// <summary>
        /// Returns the class info (version 2) for a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetClassInfo2(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out PClassInfo2 info);
    }

    static partial class Interfaces
    {
        public const string IPluginFactory2 = "0007B650-F24B-4C0B-A464-EDB9F00B2ABB";
    }

    /// <summary>
    /// Version 3 of class factory supporting PClassInfoW: IPluginFactory3
    /// </summary>
    [ComImport, Guid(Interfaces.IPluginFactory3), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginFactory3 : IPluginFactory2
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 GetFactoryInfo(
            [MarshalAs(UnmanagedType.Struct), Out] out PFactoryInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        new Int32 CountClasses();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 GetClassInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out PClassInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 CreateInstance(
            [MarshalAs(UnmanagedType.Struct), In] ref Guid classId,
            [MarshalAs(UnmanagedType.Struct), In] ref Guid interfaceId,
            [MarshalAs(UnmanagedType.SysInt, IidParameterIndex = 1), Out] out IntPtr instance);

        //---------------------------------------------------------------------

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 GetClassInfo2(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out PClassInfo2 info);

        //---------------------------------------------------------------------

        /// <summary>
        /// Returns the unicode class info for a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetClassInfoUnicode(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out PClassInfoW info);

        /// <summary>
        /// Receives information about host
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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
