using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Extended host callback interface for an edit controller: Vst::IComponentHandlerBusActivation
    /// Allows the plug-in to request the host to activate or deactivate a specific bus. 
    /// If the host accepts this request, it will call later on \ref IComponent::activateBus.
    /// This is particularly useful for instruments with more than 1 outputs, where the user could request
    /// from the plug-in UI a given output bus activation.
    /// </summary>
    [ComImport, Guid(Interfaces.IComponentHandlerBusActivation), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponentHandlerBusActivation
    {
        /// <summary>
        /// request the host to activate or deactivate a specific bus.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dir"></param>
        /// <param name="index"></param>
        /// <param name="state"></param>
        /// <returns></returns>
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
