using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.ISizeableStream), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISizeableStream
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetStreamSize(
            [MarshalAs(UnmanagedType.I8), In, Out] ref Int64 size);

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
