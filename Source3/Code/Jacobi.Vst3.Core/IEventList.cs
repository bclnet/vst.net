using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IEventList), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEventList
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetEventCount();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetEvent(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref Event e);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 AddEvent(
            [MarshalAs(UnmanagedType.Struct), In] ref Event e);
    }

    internal static partial class Interfaces
    {
        public const string IEventList = "3A2C4214-3463-49FE-B2C4-F397B9695A44";
    }
}
