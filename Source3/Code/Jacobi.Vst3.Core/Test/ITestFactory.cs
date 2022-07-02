using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core.Test
{
    [ComImport, Guid(Interfaces.ITestFactoryW), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITestFactory
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 CreateTests(
            [MarshalAs(UnmanagedType.IUnknown), In] Object context,
            [MarshalAs(UnmanagedType.Interface), In] ITestSuite parentSuite);
    }

    static partial class Interfaces
    {
        public const string ITestFactoryW = "AB483D3A-1526-4650-BF86-EEF69A327A93";
        public const string ITestFactoryA = "E77EA913-58AA-4838-986A-462053579080";
    }
}
