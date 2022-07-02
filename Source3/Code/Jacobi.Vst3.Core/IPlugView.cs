using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IPlugView), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPlugView
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int IsPlatformTypeSupported(
            [MarshalAs(UnmanagedType.LPStr), In] string type);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int Attached(
            [MarshalAs(UnmanagedType.SysInt), In] IntPtr parent,
            [MarshalAs(UnmanagedType.LPStr), In] string type);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int Removed();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int OnWheel(
            [MarshalAs(UnmanagedType.R4), In] float distance);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int OnKeyDown(
            [MarshalAs(UnmanagedType.U2), In] char key,
            [MarshalAs(UnmanagedType.I2), In] short keyCode,
            [MarshalAs(UnmanagedType.I2), In] short modifiers);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int OnKeyUp(
            [MarshalAs(UnmanagedType.U2), In] char key,
            [MarshalAs(UnmanagedType.I2), In] short keyCode,
            [MarshalAs(UnmanagedType.I2), In] short modifiers);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetSize(
            [MarshalAs(UnmanagedType.Struct), In, Out] ref ViewRect size);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int OnSize(
            [MarshalAs(UnmanagedType.Struct), In] ref ViewRect newSize);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int OnFocus(
            [MarshalAs(UnmanagedType.U1), In] bool state);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetFrame(
            [MarshalAs(UnmanagedType.Interface), In] IPlugFrame frame);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int CanResize();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int CheckSizeConstraint(
            [MarshalAs(UnmanagedType.Struct), In] ref ViewRect rect);
    }

    static partial class Interfaces
    {
        public const string IPlugView = "5BC32507-D060-49EA-A615-1B522B755B29";
    }
}
