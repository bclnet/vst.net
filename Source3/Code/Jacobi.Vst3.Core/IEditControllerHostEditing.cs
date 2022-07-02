using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IEditControllerHostEditing), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEditControllerHostEditing
    {
        Int32 BeginEditFromHost(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramID);

        Int32 EndEditFromHost(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramID);
    }

    internal static partial class Interfaces
    {
        public const string IEditControllerHostEditing = "C1271208-7059-4098-B9DD-34B36BB0195E";
    }
}
