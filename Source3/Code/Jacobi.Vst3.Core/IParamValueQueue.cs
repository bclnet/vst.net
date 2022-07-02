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
        Int32 GetPoint(
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
        Int32 AddPoint(
            [MarshalAs(UnmanagedType.I4), In] Int32 sampleOffset,
            [MarshalAs(UnmanagedType.R8), In] Double value,
            [MarshalAs(UnmanagedType.I4), In, Out] ref Int32 index);
    }

    internal static partial class Interfaces
    {
        public const string IParamValueQueue = "01263A18-ED07-4F6F-98C9-D3564686F9BA";
    }
}
