using System;
using System.Runtime.InteropServices;

namespace Steinberg.Vst3
{
    [ComImport, Guid(Interfaces.IPlugViewContentScaleSupport), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPlugViewContentScaleSupport
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetContentScaleFactor(
            [MarshalAs(UnmanagedType.R4), In] float factor);
    }

    partial class Interfaces
    {
        public const string IPlugViewContentScaleSupport = "65ED9690-8AC4-4525-8AAD-EF7A72EA703F";
    }
}
