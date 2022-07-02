using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Host callback interface for an edit controller: Vst::IPlugInterfaceSupport
    /// Allows a plug-in to ask the host if a given plug-in interface is supported/used by the host.
    /// It is implemented by the hostContext given when the component is initialized.
    /// </summary>
    [ComImport, Guid(Interfaces.IPlugInterfaceSupport), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPlugInterfaceSupport
    {
        /// <summary>
        /// Returns kResultTrue if the associated interface to the given _iid is supported/used by the host.
        /// </summary>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        public Int32 IsPlugInterfaceSupported(
            [MarshalAs(UnmanagedType.U8), Out] out Guid uid);
    }

    static partial class Interfaces
    {
        public const string IPlugInterfaceSupport = "4FB58B9E-9EAA-4E0F-AB36-1C1CCCB56FEA";
    }
}
