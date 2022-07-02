using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IPlugFrame), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPlugFrame
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 ResizeView(
            [MarshalAs(UnmanagedType.Interface), In] IPlugView view,
            [MarshalAs(UnmanagedType.Struct), In] ref ViewRect newSize);
    }

    internal static partial class Interfaces
    {
        public const string IPlugFrame = "367FAF01-AFA9-4693-8D4D-A2A0ED0882A3";
    }
}
