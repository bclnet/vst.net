using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IPluginBase), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginBase
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Initialize(
            [MarshalAs(UnmanagedType.IUnknown), In] Object context);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Terminate();
    }

    static partial class Interfaces
    {
        public const string IPluginBase = "22888DDB-156E-45AE-8358-B34808190625";
    }
}
