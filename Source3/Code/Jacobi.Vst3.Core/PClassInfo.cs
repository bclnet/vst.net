using Jacobi.Vst3.Common;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Basic Information about a class provided by the plug-in.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = Platform.StructurePack), NativeCppClass]
    public struct PClassInfo
    {
        public static readonly int Size = Marshal.SizeOf<PClassInfo>();

        [MarshalAs(UnmanagedType.Struct)] public Guid Cid;                                                      // Class ID 16 Byte class GUID
        [MarshalAs(UnmanagedType.I4)] public ClassCardinality Cardinality;                                                 // Cardinality of the class, set to kManyInstances (see \ref PClassInfo::ClassCardinality)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kCategorySize)] public String Category;       // Class category, host uses this to categorize interfaces
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kNameSize)] public String Name;               // Class name, visible to the user

        public void SetCategory(string category) { Guard.ThrowIfTooLong(nameof(category), category, 0, SizeConst.kCategorySize); Category = category; }
        public void SetName(string name) { Guard.ThrowIfTooLong(nameof(name), name, 0, SizeConst.kNameSize); Name = name; }

        public enum ClassCardinality : int
        {
            ManyInstances = 0x7FFFFFFF
        }

        public static class SizeConst
        {
            public const int kCategorySize = 32;
            public const int kNameSize = 64;
        }
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

        [MarshalAs(UnmanagedType.U4)] public ComponentFlags ClassFlags;                                         // flags used for a specific category, must be defined where category is defined
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kSubCategoriesSize)] public String SubCategories; // module specific subcategories, can be more than one, logically added by the OR operator
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kVendorSize)] public String Vendor;           // overwrite vendor information from factory info
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kVersionSize)] public String Version;         // Version string (e.g. "1.0.0.512" with Major.Minor.Subversion.Build)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kVersionSize)] public String SdkVersion;      // SDK version used to build this class (e.g. "VST 3.0")

        public static class SizeConst
        {
            public const int kVendorSize = 64;
            public const int kVersionSize = 64;
            public const int kSubCategoriesSize = 128;
        }
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

        public static class SizeConst
        {
            public const int kVendorSize = 64;
            public const int kVersionSize = 64;
            public const int kSubCategoriesSize = 128;
        }
    }
}
