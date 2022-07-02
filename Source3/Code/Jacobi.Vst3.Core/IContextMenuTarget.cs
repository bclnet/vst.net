using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IContextMenuTarget), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IContextMenuTarget
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 ExecuteMenuItem(
            [MarshalAs(UnmanagedType.I4), In] Int32 tag);
    }

    static partial class Interfaces
    {
        public const string IContextMenuTarget = "3CDF2E75-85D3-4144-BF86-D36BD7C4894D";
    }
}
