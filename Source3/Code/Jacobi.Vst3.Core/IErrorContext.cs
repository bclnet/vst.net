using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IErrorContext), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IErrorContext
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        void DisableErrorUI(
            [MarshalAs(UnmanagedType.U1), In] Boolean state);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 ErrorMessageShown();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 getErrorMessage(
            [MarshalAs(UnmanagedType.Interface), In] IString message);
    }

    internal static partial class Interfaces
    {
        public const string IErrorContext = "12BCD07B-7C69-4336-B7DA-77C3444A0CD0";
    }
}
