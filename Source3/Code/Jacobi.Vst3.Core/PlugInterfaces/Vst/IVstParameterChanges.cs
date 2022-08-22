using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Queue of changes for a specific parameter: Vst::IParamValueQueue
    /// 
    /// The change queue can be interpreted as segment of an automation curve. For each
    /// processing block, a segment with the size of the block is transmitted to the processor.
    /// The curve is expressed as sampling points of a linear approximation of
    /// the original automation curve.If the original already is a linear curve, it can
    /// be transmitted precisely.A non-linear curve has to be converted to a linear
    /// approximation by the host.Every point of the value queue defines a linear
    /// section of the curve as a straight line from the previous point of a block to
    /// the new one.So the plug-in can calculate the value of the curve for any sample
    /// position in the block.
    /// </summary>
    [ComImport, Guid(Interfaces.IParamValueQueue), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IParamValueQueue
    {
        /// <summary>
        /// Returns its associated ID.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.U4)]
        UInt32 GetParameterId();

        /// <summary>
        /// Returns count of points in the queue.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetPointCount();

        /// <summary>
        /// Gets the value and offset at a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="sampleOffset"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetPoint(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.I4), In, Out] ref Int32 sampleOffset,
            [MarshalAs(UnmanagedType.R8), In, Out] ref Double value);

        /// <summary>
        /// Adds a new value at the end of the queue, its index is returned.
        /// </summary>
        /// <param name="sampleOffset"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult AddPoint(
            [MarshalAs(UnmanagedType.I4), In] Int32 sampleOffset,
            [MarshalAs(UnmanagedType.R8), In] Double value,
            [MarshalAs(UnmanagedType.I4), In, Out] ref Int32 index);
    }

    static partial class Interfaces
    {
        public const string IParamValueQueue = "01263A18-ED07-4F6F-98C9-D3564686F9BA";
    }

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
        [return: MarshalAs(UnmanagedType.Interface)]
        IParamValueQueue GetParameterData(
            [MarshalAs(UnmanagedType.I4), In] Int32 index);
        //[return: MarshalAs(UnmanagedType.SysInt)]
        //IntPtr GetParameterData(
        //    [MarshalAs(UnmanagedType.I4), In] Int32 index);

        /// <summary>
        /// Adds a new parameter queue with a given ID at the end of the list,
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <returns>returns it and its index in the parameter changes list.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        IParamValueQueue AddParameterData(
            [MarshalAs(UnmanagedType.U4), In] UInt32 id,
            [MarshalAs(UnmanagedType.I4), In, Out] ref Int32 index);
        //[return: MarshalAs(UnmanagedType.SysInt)]
        //IntPtr AddParameterData(
        //    [MarshalAs(UnmanagedType.U4), In] UInt32 id,
        //    [MarshalAs(UnmanagedType.I4), In, Out] ref Int32 index);
    }

    static partial class Interfaces
    {
        public const string IParameterChanges = "A4779663-0BB6-4A56-B443-84A8466FEB9D";
    }
}
