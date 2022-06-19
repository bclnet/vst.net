using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.INoteExpressionPhysicalUIMapping), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface INoteExpressionPhysicalUIMapping
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetPhysicalUIMapping(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref PhysicalUIMapList list);
    }
}
