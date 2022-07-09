using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Extended host callback interface Vst::IComponentHandler3 for an edit controller. 
    /// A plug-in can ask the host to create a context menu for a given exported parameter ID or a generic context menu.\n
    /// 
    /// The host may pre-fill this context menu with specific items regarding the parameter ID like "Show automation for parameter",
    /// "MIDI learn" etc...\n
    /// </summary>
    [ComImport, Guid(Interfaces.IComponentHandler3), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponentHandler3
    {
        /// <summary>
        /// Creates a host context menu for a plug-in:
		/// - If paramID is zero, the host may create a generic context menu.
		/// - The IPlugView object must be valid.
		/// - The return IContextMenu object needs to be released afterwards by the plug-in.
        /// </summary>
        /// <param name="plugView"></param>
        /// <param name="paramID"></param>
        /// <returns></returns>
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
