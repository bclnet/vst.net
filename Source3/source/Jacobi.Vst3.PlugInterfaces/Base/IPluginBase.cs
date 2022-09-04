using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Steinberg.Vst3
{
    /// <summary>
    /// Basic interface to a plug-in component: IPluginBase
    /// - initialize/terminate the plug-in component
    /// 
    /// The host uses this interface to initialize and to terminate the plug-in component.
    /// The context that is passed to the initialize method contains any interface to the
    /// host that the plug-in will need to work. These interfaces can vary from category to category.
    /// A list of supported host context interfaces should be included in the documentation
    /// of a specific category.
    /// </summary>
    [ComImport, Guid(Interfaces.IPluginBase), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginBase
    {
        /// <summary>
        /// The host passes a number of interfaces as context to initialize the plug-in class.
        /// \param context, passed by the host, is mandatory and should implement IHostApplication
        /// @note Extensive memory allocations etc. should be performed in this method rather than in
        /// the class' constructor! If the method does NOT return kResultOk, the object is released
        /// immediately. In this case terminate is not called!
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult Initialize(
            [MarshalAs(UnmanagedType.IUnknown), In] Object context);

        /// <summary>
        /// This function is called before the plug-in is unloaded and can be used for
	    /// cleanups. You have to release all references to any host application interfaces.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult Terminate();
    }

    partial class Interfaces
    {
        public const string IPluginBase = "22888DDB-156E-45AE-8358-B34808190625";
    }

    // Note CharSet is not the same as the platform global value.
    /// <summary>
    /// Basic Information about the class factory of the plug-in.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = Platform.StructurePack)]
    public struct PFactoryInfo
    {
        [Flags]
        public enum FactoryFlags : int
        {
            /// <summary>Nothing</summary>
            NoFlags = 0,

            /// <summary>
            /// The number of exported classes can change each time the Module is loaded. 
            /// If this flag is set, the host does not cache class information. 
            /// This leads to a longer startup time because the host always has to load the Module to get the current class information.
            /// </summary>
            ClassesDiscardable = 1 << 0,

            /// <summary>
            /// Class IDs of components are interpreted as Syncrosoft-License (LICENCE_UID). 
            /// Loaded in a Steinberg host, the module will not be loaded when the license is not valid.
            /// </summary>
            LicenseCheck = 1 << 1,

            /// <summary>Component won't be unloaded until process exit.</summary>
            ComponentNonDiscardable = 1 << 3,

            /// <summary>Components have entirely unicode encoded strings. (True for VST 3 Plug-ins so far)</summary>
            Unicode = 1 << 4
        }

        public static class SizeConst
        {
            public const int kURLSize = 256;
            public const int kEmailSize = 128;
            public const int kNameSize = 64;
        }

        public static readonly int Size = Marshal.SizeOf<PFactoryInfo>();

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kNameSize)] public String Vendor; // e.g. "Steinberg Media Technologies"
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kURLSize)] public String Url;     // e.g. "http://www.steinberg.de"
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kEmailSize)] public String Email; // e.g. "info@steinberg.de"
        [MarshalAs(UnmanagedType.I4)] public FactoryFlags Flags;                                    // (see FactoryFlags above)

        public void SetVendor(string vendor) { Guard.ThrowIfTooLong(nameof(vendor), vendor, 0, SizeConst.kNameSize); Vendor = vendor; }
        public void SetEmail(string email) { Guard.ThrowIfTooLong(nameof(email), email, 0, SizeConst.kEmailSize); Email = email; }
        public void SetUrl(string url) { Guard.ThrowIfTooLong(nameof(url), url, 0, SizeConst.kURLSize); Url = url; }
    }

    /// <summary>
    /// Basic Information about a class provided by the plug-in.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = Platform.StructurePack), NativeCppClass]
    public struct PClassInfo
    {
        public enum ClassCardinality : int
        {
            ManyInstances = 0x7FFFFFFF
        }

        public static class SizeConst
        {
            public const int kCategorySize = 32;
            public const int kNameSize = 64;
        }

        public static readonly int Size = Marshal.SizeOf<PClassInfo>();

        [MarshalAs(UnmanagedType.Struct)] public Guid Cid;                                                      // Class ID 16 Byte class GUID
        [MarshalAs(UnmanagedType.I4)] public ClassCardinality Cardinality;                                                 // Cardinality of the class, set to kManyInstances (see \ref PClassInfo::ClassCardinality)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kCategorySize)] public String Category;       // Class category, host uses this to categorize interfaces
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kNameSize)] public String Name;               // Class name, visible to the user

        public void SetCategory(string category) { Guard.ThrowIfTooLong(nameof(category), category, 0, SizeConst.kCategorySize); Category = category; }
        public void SetName(string name) { Guard.ThrowIfTooLong(nameof(name), name, 0, SizeConst.kNameSize); Name = name; }
    }

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
        TResult GetFactoryInfo(
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
        TResult GetClassInfo(
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
        TResult CreateInstance(
            [MarshalAs(UnmanagedType.Struct), In] ref Guid classId,
            [MarshalAs(UnmanagedType.Struct), In] ref Guid interfaceId,
            [MarshalAs(UnmanagedType.SysInt, IidParameterIndex = 1), Out] out IntPtr instance);
    }

    partial class Interfaces
    {
        public const string IPluginFactory = "7A4D811C-5211-4A1F-AED9-D2EE0B43BF9F";
    }

    /// <summary>
    /// Version 2 of Basic Information about a class provided by the plug-in.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = Platform.StructurePack), NativeCppClass]
    public struct PClassInfo2
    {
        public static readonly int Size = Marshal.SizeOf<PClassInfo2>();

        [MarshalAs(UnmanagedType.Struct)] public Guid Cid;                                                      // Class ID 16 Byte class GUID
        [MarshalAs(UnmanagedType.I4)] public PClassInfo.ClassCardinality Cardinality;                           // Cardinality of the class, set to kManyInstances (see \ref PClassInfo::ClassCardinality)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PClassInfo.SizeConst.kCategorySize)] public String Category; // Class category, host uses this to categorize interfaces
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PClassInfo.SizeConst.kNameSize)] public String Name;    // Class name, visible to the user

        // --------------------------------------------------------------------

        public static class SizeConst
        {
            public const int kVendorSize = 64;
            public const int kVersionSize = 64;
            public const int kSubCategoriesSize = 128;
        }

        [MarshalAs(UnmanagedType.U4)] public ComponentFlags ClassFlags;                                         // flags used for a specific category, must be defined where category is defined
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kSubCategoriesSize)] public String SubCategories; // module specific subcategories, can be more than one, logically added by the OR operator
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kVendorSize)] public String Vendor;           // overwrite vendor information from factory info
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kVersionSize)] public String Version;         // Version string (e.g. "1.0.0.512" with Major.Minor.Subversion.Build)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kVersionSize)] public String SdkVersion;      // SDK version used to build this class (e.g. "VST 3.0")
    }

    /// <summary>
    /// Version 2 of class factory supporting PClassInfo2: IPluginFactory2
    /// </summary>
    [ComImport, Guid(Interfaces.IPluginFactory2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginFactory2 : IPluginFactory
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new TResult GetFactoryInfo(
            [MarshalAs(UnmanagedType.Struct), Out] out PFactoryInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        new Int32 CountClasses();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new TResult GetClassInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out PClassInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new TResult CreateInstance(
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
        TResult GetClassInfo2(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out PClassInfo2 info);
    }

    partial class Interfaces
    {
        public const string IPluginFactory2 = "0007B650-F24B-4C0B-A464-EDB9F00B2ABB";
    }

    /// <summary>
    /// Unicode Version of Basic Information about a class provided by the plug-in
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = Platform.StructurePack), NativeCppClass]
    public struct PClassInfoW
    {
        public static readonly int Size = Marshal.SizeOf<PClassInfoW>();

        [MarshalAs(UnmanagedType.Struct)] public Guid Cid;                      // see \ref PClassInfo
        [MarshalAs(UnmanagedType.I4)] public PClassInfo.ClassCardinality Cardinality; // see \ref PClassInfo
        public AnsiCategory Category;                                           // see \ref PClassInfo
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PClassInfo.SizeConst.kNameSize)] public String Name;

        // --------------------------------------------------------------------

        public static class SizeConst
        {
            public const int kVendorSize = 64;
            public const int kVersionSize = 64;
            public const int kSubCategoriesSize = 128;
        }

        [MarshalAs(UnmanagedType.U4)] public ComponentFlags ClassFlags;                                         // flags used for a specific category, must be defined where category is defined
        public AnsiSubCategories SubCategories;                                                                 // module specific subcategories, can be more than one, logically added by the OR operator
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kVendorSize)] public String Vendor;           // overwrite vendor information from factory info
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kVersionSize)] public String Version;         // Version string (e.g. "1.0.0.512" with Major.Minor.Subversion.Build)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kVersionSize)] public String SdkVersion;      // SDK version used to build this class (e.g. "VST 3.0")

        //---------------------------------------------------------------------
        // need extra structs to solve mixed Ansi/Unicode strings

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = Platform.StructurePack), NativeCppClass]
        public struct AnsiCategory
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PClassInfo.SizeConst.kCategorySize)] public String Value;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = Platform.StructurePack), NativeCppClass]
        public struct AnsiSubCategories
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kSubCategoriesSize)] public String Value;
        }
    }

    /// <summary>
    /// Version 3 of class factory supporting PClassInfoW: IPluginFactory3
    /// </summary>
    [ComImport, Guid(Interfaces.IPluginFactory3), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginFactory3 : IPluginFactory2
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new TResult GetFactoryInfo(
            [MarshalAs(UnmanagedType.Struct), Out] out PFactoryInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        new Int32 CountClasses();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new TResult GetClassInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out PClassInfo info);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new TResult CreateInstance(
            [MarshalAs(UnmanagedType.Struct), In] ref Guid classId,
            [MarshalAs(UnmanagedType.Struct), In] ref Guid interfaceId,
            [MarshalAs(UnmanagedType.SysInt, IidParameterIndex = 1), Out] out IntPtr instance);

        //---------------------------------------------------------------------

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new TResult GetClassInfo2(
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
        TResult GetClassInfoUnicode(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out PClassInfoW info);

        /// <summary>
        /// Receives information about host
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetHostContext(
            [MarshalAs(UnmanagedType.IUnknown), In] Object context);
    }

    partial class Interfaces
    {
        public const string IPluginFactory3 = "4555A2AB-C123-4E57-9B12-291036878931";
    }
}
