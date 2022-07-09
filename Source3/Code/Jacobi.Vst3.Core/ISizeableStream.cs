using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Stream with a size.
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
        Int32 GetStreamSize(
            [MarshalAs(UnmanagedType.I8), Out] out Int64 size);

        /// <summary>
        /// Set the steam size. File streams can only be resized if they are write enabled.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetStreamSize(
            [MarshalAs(UnmanagedType.I8), In] Int64 size);
    }

    static partial class Interfaces
    {
        public const string ISizeableStream = "04F9549E-E02F-4E6E-87E8-6A8747F4E17F";
    }
}
