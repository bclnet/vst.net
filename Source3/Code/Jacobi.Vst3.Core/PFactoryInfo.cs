using Jacobi.Vst3.Common;
using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    // Note CharSet is not the same as the platform global value.
    /// <summary>
    /// Basic Information about the class factory of the plug-in.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = Platform.StructurePack)]
    public struct PFactoryInfo
    {
        public static readonly int Size = Marshal.SizeOf<PFactoryInfo>();

        public const FactoryFlags DefaultFactoryFlags = FactoryFlags.Unicode;	                    // no programs are used in the unit.

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kNameSize)] public String Vendor; // e.g. "Steinberg Media Technologies"
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kURLSize)] public String Url;     // e.g. "http://www.steinberg.de"
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeConst.kEmailSize)] public String Email; // e.g. "info@steinberg.de"
        [MarshalAs(UnmanagedType.I4)] public FactoryFlags Flags;             // (see FactoryFlags above)

        public void SetVendor(string vendor) { Guard.ThrowIfTooLong(nameof(vendor), vendor, 0, SizeConst.kNameSize); Vendor = vendor; }
        public void SetEmail(string email) { Guard.ThrowIfTooLong(nameof(email), email, 0, SizeConst.kEmailSize); Email = email; }
        public void SetUrl(string url) { Guard.ThrowIfTooLong(nameof(url), url, 0, SizeConst.kURLSize); Url = url; }

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
    }
}
