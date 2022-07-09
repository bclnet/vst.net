using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// A component can support unit preset data via this interface or program list data(IProgramListData).
    /// </summary>
    [ComImport, Guid(Interfaces.IProgress), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IProgress
    {
        /// <summary>
        /// Start a new progress of a given type and optional Description. outID is as ID created by the
        /// host to identify this newly created progress (for update and finish method)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="optionalDescription"></param>
        /// <param name="outID"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Start(
            [MarshalAs(UnmanagedType.U4), In] ProgressType type,
            [MarshalAs(UnmanagedType.LPWStr), In] string optionalDescription,
            [MarshalAs(UnmanagedType.U8), Out] out UInt64 outID);

        /// <summary>
        /// Update the progress value (normValue between [0, 1]) associated to the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="normValue"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Update(
            [MarshalAs(UnmanagedType.U8), In] UInt64 id,
            [MarshalAs(UnmanagedType.R8), In] Double normValue);

        /// <summary>
        /// Finish the progress associated to the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Finish(
            [MarshalAs(UnmanagedType.U8), In] UInt64 id);

        public enum ProgressType : uint
        {
            AsyncStateRestoration = 0,  ///< plug-in state is restored async (in a background Thread)
            UIBackgroundTask            ///< a plug-in task triggered by a UI action 
        }
    }

    static partial class Interfaces
    {
        public const string IProgress = "00C9DC5B-9D90-4254-91A3-88C8B4E91B69";
    }
}
