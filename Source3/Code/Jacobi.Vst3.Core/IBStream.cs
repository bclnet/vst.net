using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Base class for streams.
    /// - read/write binary data from/to stream
    /// - get/set stream read-write position(read and write position is the same)
    /// </summary>
    [ComImport, Guid(Interfaces.IBStream), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IBStream
    {
        /// <summary>
        /// Reads binary data from stream.
        /// </summary>
        /// <param name="buffer">destination buffer</param>
        /// <param name="numBytes">amount of bytes to be read</param>
        /// <param name="numBytesRead">result - how many bytes have been read from stream (set to 0 if this is of no interest)</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Read(
            [MarshalAs(UnmanagedType.SysInt), In] IntPtr buffer,
            [MarshalAs(UnmanagedType.I4), In] Int32 numBytes,
            [MarshalAs(UnmanagedType.I4), Out] out Int32 numBytesRead);

        /// <summary>
        /// Writes binary data to stream.
        /// </summary>
        /// <param name="buffer">source buffer</param>
        /// <param name="numBytes">amount of bytes to write</param>
        /// <param name="numBytesWritten">result - how many bytes have been written to stream (set to 0 if this is of no interest)</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Write(
            [MarshalAs(UnmanagedType.SysInt), In] IntPtr buffer,
            [MarshalAs(UnmanagedType.I4), In] Int32 numBytes,
            [MarshalAs(UnmanagedType.I4), Out] out Int32 numBytesWritten);

        /// <summary>
        /// Sets stream read-write position.
        /// </summary>
        /// <param name="pos">new stream position (dependent on mode)</param>
        /// <param name="mode">value of enum IStreamSeekMode</param>
        /// <param name="result">new seek position (set to 0 if this is of no interest)</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Seek(
            [MarshalAs(UnmanagedType.I8), In] Int64 pos,
            [MarshalAs(UnmanagedType.I4), In] StreamSeekMode mode,
            [MarshalAs(UnmanagedType.I8), In, Out] ref Int64 result);

        /// <summary>
        /// Gets current stream read-write position. 
        /// </summary>
        /// <param name="pos">is assigned the current position if function succeeds</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Tell(
            [MarshalAs(UnmanagedType.I8), Out] out Int64 pos);
    }

    static partial class Interfaces
    {
        public const string IBStream = "C3BF6EA2-3099-4752-9B6B-F9901EE33E9B";
    }

    public enum StreamSeekMode
    {
        SeekSet = 0, // set absolute seek position
        SeekCur,     // set seek position relative to current position
        SeekEnd      // set seek position relative to stream end
    }
}
