using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core.Test
{
    [ComImport, Guid(Interfaces.ITestSuiteW), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITestSuite
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 AddTest(
            [MarshalAs(UnmanagedType.LPWStr), In] String name,
            [MarshalAs(UnmanagedType.Interface), In] ITest test);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 AddTestSuite(
            [MarshalAs(UnmanagedType.LPWStr), In] String name,
            [MarshalAs(UnmanagedType.Interface), In] ITestSuite testSuite);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetEnvironment(
            [MarshalAs(UnmanagedType.Interface), In] ITest environment);
    }

    static partial class Interfaces
    {
        public const string ITestSuiteW = "5CA7106F-9878-4AA5-B4D3-0D712F5F1498";
        public const string ITestSuiteA = "81724C94-E9F6-4F65-ACB1-04E9CC702253";
    }
}
