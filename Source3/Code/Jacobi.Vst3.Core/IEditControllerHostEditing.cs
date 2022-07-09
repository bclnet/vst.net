using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Parameter Editing from host: Vst::IEditControllerHostEditing
    /// If this interface is implemented by the edit controller, and when performing edits from outside
    /// the plug-in (host / remote) of a not automatable and not read-only, and not hidden flagged parameter(kind of helper parameter),
    /// the host will start with a beginEditFromHost before calling setParamNormalized and end with an endEditFromHost.
    /// </summary>
    [ComImport, Guid(Interfaces.IEditControllerHostEditing), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEditControllerHostEditing
    {
        /// <summary>
        /// Called before a setParamNormalized sequence, a endEditFromHost will be call at the end of the editing action.
        /// </summary>
        /// <param name="paramID"></param>
        /// <returns></returns>
        Int32 BeginEditFromHost(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramID);

        /// <summary>
        /// Called after a beginEditFromHost and a sequence of setParamNormalized.
        /// </summary>
        /// <param name="paramID"></param>
        /// <returns></returns>
        Int32 EndEditFromHost(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramID);
    }

    internal static partial class Interfaces
    {
        public const string IEditControllerHostEditing = "C1271208-7059-4098-B9DD-34B36BB0195E";
    }
}
