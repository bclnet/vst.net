using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core.Test
{
    [ComImport, Guid(Interfaces.ITestResultW), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITestResult
    {
        [PreserveSig]
        void AddErrorMessage([MarshalAs(UnmanagedType.LPWStr), In] String msg);

        [PreserveSig]
        void AddMessage([MarshalAs(UnmanagedType.LPWStr), In] String msg);
    }

    static partial class Interfaces
    {
        public const string ITestResultW = "69796279-F651-418B-B24D-79B7D7C527F4";
        public const string ITestResultA = "CE13B461-5334-451D-B394-3E997446885B";
    }
}
