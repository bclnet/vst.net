using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core.Test
{
    [ComImport, Guid(Interfaces.ITestW), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITest
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.U1)]
        Boolean Setup();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.U1)]
        Boolean Run(
            [MarshalAs(UnmanagedType.Interface), In] ITestResult testResult);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.U1)]
        Boolean Teardown();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        String GetDescription();
    }

    static partial class Interfaces
    {
        public const string ITestW = "FE64FC19-9568-4F53-AAA7-8DC87228338E";
        public const string ITestA = "9E2E608B-64C6-4CF8-8390-59BDA194032D";
    }
}
