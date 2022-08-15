using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    public enum StreamSeekMode
    {
        SeekSet = 0, // set absolute seek position
        SeekCur,     // set seek position relative to current position
        SeekEnd      // set seek position relative to stream end
    }

    /// <summary>
    /// Base class for streams.
    /// pluginBase:
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
        TResult Read(
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
        TResult Write(
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
        TResult Seek(
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
        TResult Tell(
            [MarshalAs(UnmanagedType.I8), Out] out Int64 pos);
    }

    static partial class Interfaces
    {
        public const string IBStream = "C3BF6EA2-3099-4752-9B6B-F9901EE33E9B";
    }

    /// <summary>
    /// Stream with a size.
    /// pluginBase:
    /// [extends IBStream] when stream type supports it (like file and memory stream)
    /// </summary>
    [ComImport, Guid(Interfaces.ISizeableStream), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISizeableStream
    {
        /// <summary>
        /// Return the stream size
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetStreamSize(
            [MarshalAs(UnmanagedType.I8), Out] out Int64 size);

        /// <summary>
        /// Set the steam size. File streams can only be resized if they are write enabled.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetStreamSize(
            [MarshalAs(UnmanagedType.I8), In] Int64 size);
    }

    static partial class Interfaces
    {
        public const string ISizeableStream = "04F9549E-E02F-4E6E-87E8-6A8747F4E17F";
    }
}
