using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IComponentHandlerBusActivation), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponentHandlerBusActivation
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 RequestBusActivation(
            [MarshalAs(UnmanagedType.I4), In] MediaTypes type,
            [MarshalAs(UnmanagedType.I4), In] BusDirections dir,
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.I4), In] Boolean state);
    }

    static partial class Interfaces
    {
        public const string IComponentHandlerBusActivation = "067D02C1-5B4E-274D-A92D-90FD6EAF7240";
    }
}
