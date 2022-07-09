using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Context Menu Item Target interface: Vst::IContextMenuTarget
    /// A receiver of a menu item should implement this interface, which will be called after the user has selected
    /// this menu item.
    /// </summary>
    [ComImport, Guid(Interfaces.IContextMenuTarget), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IContextMenuTarget
    {
        /// <summary>
        /// Called when an menu item was executed.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 ExecuteMenuItem(
            [MarshalAs(UnmanagedType.I4), In] Int32 tag);
    }

    static partial class Interfaces
    {
        public const string IContextMenuTarget = "3CDF2E75-85D3-4144-BF86-D36BD7C4894D";
    }
}
