using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Host callback interface for an edit controller: Vst::IComponentHandler
    /// Allow transfer of parameter editing to component (processor) via host and support automation.
    /// Cause the host to react on configuration changes(restartComponent).
    /// </summary>
    [ComImport, Guid(Interfaces.IComponentHandler), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponentHandler
    {
        /// <summary>
        /// To be called before calling a performEdit (e.g. on mouse-click-down event).
        /// This must be called in the UI-Thread context!
        /// </summary>
        /// <param name="paramId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 BeginEdit(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId);

        /// <summary>
        /// Called between beginEdit and endEdit to inform the handler that a given parameter has a new
        /// value.This must be called in the UI-Thread context!
        /// </summary>
        /// <param name="paramId"></param>
        /// <param name="valueNormalized"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 PerformEdit(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId,
            [MarshalAs(UnmanagedType.R8), In] Double valueNormalized);

        /// <summary>
        /// To be called after calling a performEdit (e.g. on mouse-click-up event).
        /// This must be called in the UI-Thread context!
        /// </summary>
        /// <param name="paramId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 EndEdit(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId);

        /// <summary>
        /// Instructs host to restart the component. This must be called in the UI-Thread context!
        /// </summary>
        /// <param name="flags">flags is a combination of RestartFlags</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 RestartComponent(
            [MarshalAs(UnmanagedType.I4), In] RestartFlags flags);
    }

    static partial class Interfaces
    {
        public const string IComponentHandler = "93A0BEA3-0BD0-45DB-8E89-0B0CC1E46AC6";
    }
}
