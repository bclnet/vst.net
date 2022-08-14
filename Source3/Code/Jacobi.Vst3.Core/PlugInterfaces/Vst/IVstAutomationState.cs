using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Extended plug-in interface IEditController: Vst::IAutomationState
    /// Hosts can inform the plug-in about its current automation state (Read/Write/Nothing).
    /// </summary>
    [ComImport, Guid(Interfaces.IAutomationState), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAutomationState
    {
        /// <summary>
        /// Sets the current Automation state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetAutomationState(
            [MarshalAs(UnmanagedType.I4), In] AutomationStates state);

        public enum AutomationStates : int
        {
            NoAutomation = 0,      // Not Read and not Write
            ReadState = 1 << 0,    // Read state
            WriteState = 1 << 1,   // Write state

            ReadWriteState = ReadState | WriteState, // Read and Write enable
        }
    }

    static partial class Interfaces
    {
        public const string IAutomationState = "B4E8287F-1BB3-46AA-83A4-666768937BAB";
    }
}
