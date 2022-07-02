using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IDependent), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDependent
    {
        void Update(
            [MarshalAs(UnmanagedType.IUnknown), In] Object changedUnknown,
            [MarshalAs(UnmanagedType.I4), In] Int32 message);
    }

    internal static partial class Interfaces
    {
        public const string IDependent = "F52B7AAE-DE72-416d-8AF1-8ACE9DD7BD5E";
    }

    // IDependent.Update messages
    public enum ChangeMessages
    {
        WillChange,
        Changed,
        Destroyed,
        WillDestroy,

        StdChangeMessageLast = WillDestroy
    }
}
