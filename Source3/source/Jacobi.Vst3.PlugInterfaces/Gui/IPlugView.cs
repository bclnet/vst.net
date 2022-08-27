using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3
{
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct ViewRect
    {
        public static readonly int Size = Marshal.SizeOf<ViewRect>();

        [MarshalAs(UnmanagedType.I4)] public Int32 Left;
        [MarshalAs(UnmanagedType.I4)] public Int32 Top;
        [MarshalAs(UnmanagedType.I4)] public Int32 Right;
        [MarshalAs(UnmanagedType.I4)] public Int32 Bottom;
    }

    //WHERE: kPlatformTypeHWND -> ??
    //WHERE: kPlatformTypeHIView -> ??
    //WHERE: kPlatformTypeNSView -> ??
    //WHERE: kPlatformTypeUIView -> ??
    //WHERE: kPlatformTypeX11EmbedWindowID -> ??

    [ComImport, Guid(Interfaces.IPlugView), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPlugView
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult IsPlatformTypeSupported(
            [MarshalAs(UnmanagedType.LPStr), In] string type);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult Attached(
            [MarshalAs(UnmanagedType.SysInt), In] IntPtr parent,
            [MarshalAs(UnmanagedType.LPStr), In] string type);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult Removed();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult OnWheel(
            [MarshalAs(UnmanagedType.R4), In] float distance);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult OnKeyDown(
            [MarshalAs(UnmanagedType.U2), In] char key,
            [MarshalAs(UnmanagedType.I2), In] short keyCode,
            [MarshalAs(UnmanagedType.I2), In] short modifiers);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult OnKeyUp(
            [MarshalAs(UnmanagedType.U2), In] char key,
            [MarshalAs(UnmanagedType.I2), In] short keyCode,
            [MarshalAs(UnmanagedType.I2), In] short modifiers);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetSize(
            [MarshalAs(UnmanagedType.Struct), In, Out] ref ViewRect size);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult OnSize(
            [MarshalAs(UnmanagedType.Struct), In] ref ViewRect newSize);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult OnFocus(
            [MarshalAs(UnmanagedType.U1), In] bool state);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetFrame(
            [MarshalAs(UnmanagedType.Interface), In] IPlugFrame frame);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult CanResize();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult CheckSizeConstraint(
            [MarshalAs(UnmanagedType.Struct), In] ref ViewRect rect);
    }

    partial class Interfaces
    {
        public const string IPlugView = "5BC32507-D060-49EA-A615-1B522B755B29";
    }

    [ComImport, Guid(Interfaces.IPlugFrame), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPlugFrame
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult ResizeView(
            [MarshalAs(UnmanagedType.Interface), In] IPlugView view,
            [MarshalAs(UnmanagedType.Struct), In] ref ViewRect newSize);
    }

    partial class Interfaces
    {
        public const string IPlugFrame = "367FAF01-AFA9-4693-8D4D-A2A0ED0882A3";
    }
}
