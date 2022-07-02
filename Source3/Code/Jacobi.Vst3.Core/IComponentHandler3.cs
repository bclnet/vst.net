using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IComponentHandler3), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponentHandler3
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        IContextMenu CreateContextMenu(
            [MarshalAs(UnmanagedType.Interface), In] IPlugView plugView,
            [MarshalAs(UnmanagedType.U4), In, Out] ref UInt32 paramID);
    }

    static partial class Interfaces
    {
        public const string IComponentHandler3 = "69F11617-D26B-400D-A4B6-B9647B6EBBAB";
    }
}
