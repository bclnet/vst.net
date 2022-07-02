using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// All parameter changes of a processing block: Vst::IParameterChanges
    /// 
    /// This interface is used to transmit any changes to be applied to parameters
    /// in the current processing block.A change can be caused by GUI interaction as
    /// well as automation.They are transmitted as a list of queues (\ref IParamValueQueue)
    /// containing only queues for parameters that actually did change.
    /// </summary>
    [ComImport, Guid(Interfaces.IParameterChanges), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IParameterChanges
    {
        /// <summary>
        /// Returns count of Parameter changes in the list.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetParameterCount();

        /// <summary>
        /// Returns the queue at a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [PreserveSig]
        //[return: MarshalAs(UnmanagedType.Interface)]
        //IParamValueQueue GetParameterData(
        //    [MarshalAs(UnmanagedType.I4), In] Int32 index);
        [return: MarshalAs(UnmanagedType.SysInt)]
        IntPtr GetParameterData(
            [MarshalAs(UnmanagedType.I4), In] Int32 index);

        /// <summary>
        /// Adds a new parameter queue with a given ID at the end of the list,
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <returns>returns it and its index in the parameter changes list.</returns>
        [PreserveSig]
        //[return: MarshalAs(UnmanagedType.Interface)]
        //IParamValueQueue AddParameterData(
        //    [MarshalAs(UnmanagedType.U4), In] UInt32 id,
        //    [MarshalAs(UnmanagedType.I4), In, Out] ref Int32 index);
        [return: MarshalAs(UnmanagedType.SysInt)]
        IntPtr AddParameterData(
            [MarshalAs(UnmanagedType.U4), In] UInt32 id,
            [MarshalAs(UnmanagedType.I4), In, Out] ref Int32 index);
    }

    internal static partial class Interfaces
    {
        public const string IParameterChanges = "A4779663-0BB6-4A56-B443-84A8466FEB9D";
    }
}
