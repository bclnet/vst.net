using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Extended host callback interface for an edit controller: Vst::IComponentHandler2
    /// </summary>
    [ComImport, Guid(Interfaces.IComponentHandler2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponentHandler2
    {
        /// <summary>
        /// Tells host that the plug-in is dirty (something besides parameters has changed since last save),
        /// if true the host should apply a save before quitting.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetDirty(
            [MarshalAs(UnmanagedType.U1), In] Boolean state);

        /// <summary>
        /// Tells host that it should open the plug-in editor the next time it's possible.
        /// You should use this instead of showing an alert and blocking the program flow (especially on loading projects).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 RequestOpenEditor(
            [MarshalAs(UnmanagedType.LPStr), In] String name);

        /// <summary>
        /// Starts the group editing (call before a \ref IComponentHandler::beginEdit),
        /// the host will keep the current timestamp at this call and will use it for all \ref IComponentHandler::beginEdit
        /// \ref IComponentHandler::performEdit / \ref IComponentHandler::endEdit calls until a \ref finishGroupEdit().
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 StartGroupEdit();

        /// <summary>
        /// Finishes the group editing started by a \ref startGroupEdit (call after a \ref IComponentHandler::endEdit).
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 FinishGroupEdit();
    }

    static partial class Interfaces
    {
        public const string IComponentHandler2 = "F040B4B3-A360-45EC-ABCD-C045B4D5A2CC";
    }
}
