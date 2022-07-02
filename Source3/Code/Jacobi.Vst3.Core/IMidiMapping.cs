using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IMidiMapping), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMidiMapping
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetMidiControllerAssignment(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.I2), In] ControllerNumbers midiControllerNumber,
            [MarshalAs(UnmanagedType.U4), In, Out] ref UInt32 paramId);
    }

    internal static partial class Interfaces
    {
        public const string IMidiMapping = "DF0FF9F7-49B7-4669-B63A-B7327ADBF5E5";
    }
}
