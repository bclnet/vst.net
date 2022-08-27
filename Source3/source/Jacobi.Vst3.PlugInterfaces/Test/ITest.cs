using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3
{
    static partial class Constants
    {
        public const string kTestClass = "Test Class";
    }

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

    partial class Interfaces
    {
        public const string ITestW = "FE64FC19-9568-4F53-AAA7-8DC87228338E";
        public const string ITestA = "9E2E608B-64C6-4CF8-8390-59BDA194032D";
    }

    [ComImport, Guid(Interfaces.ITestResultW), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITestResult
    {
        [PreserveSig]
        void AddErrorMessage([MarshalAs(UnmanagedType.LPWStr), In] String msg);

        [PreserveSig]
        void AddMessage([MarshalAs(UnmanagedType.LPWStr), In] String msg);
    }

    partial class Interfaces
    {
        public const string ITestResultW = "69796279-F651-418B-B24D-79B7D7C527F4";
        public const string ITestResultA = "CE13B461-5334-451D-B394-3E997446885B";
    }

    [ComImport, Guid(Interfaces.ITestSuiteW), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITestSuite
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult AddTest(
            [MarshalAs(UnmanagedType.LPWStr), In] String name,
            [MarshalAs(UnmanagedType.Interface), In] ITest test);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult AddTestSuite(
            [MarshalAs(UnmanagedType.LPWStr), In] String name,
            [MarshalAs(UnmanagedType.Interface), In] ITestSuite testSuite);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetEnvironment(
            [MarshalAs(UnmanagedType.Interface), In] ITest environment);
    }

    partial class Interfaces
    {
        public const string ITestSuiteW = "5CA7106F-9878-4AA5-B4D3-0D712F5F1498";
        public const string ITestSuiteA = "81724C94-E9F6-4F65-ACB1-04E9CC702253";
    }

    [ComImport, Guid(Interfaces.ITestFactoryW), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITestFactory
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult CreateTests(
            [MarshalAs(UnmanagedType.IUnknown), In] Object context,
            [MarshalAs(UnmanagedType.Interface), In] ITestSuite parentSuite);
    }

    partial class Interfaces
    {
        public const string ITestFactoryW = "AB483D3A-1526-4650-BF86-EEF69A327A93";
        public const string ITestFactoryA = "E77EA913-58AA-4838-986A-462053579080";
    }
}
