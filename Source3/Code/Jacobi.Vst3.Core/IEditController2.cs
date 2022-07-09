using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Edit controller component interface extension: Vst::IEditController2
    /// Extension to allow the host to inform the plug-in about the host Knob Mode,
    /// and to open the plug-in about box or help documentation.
    /// </summary>
    [ComImport, Guid(Interfaces.IEditController2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEditController2
    {
        /// <summary>
        /// Host could set the Knob Mode for the plug-in.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns>kResultFalse means not supported mode.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetKnobMode(
            [MarshalAs(UnmanagedType.I4), In] KnobMode mode);

        /// <summary>
        /// Host could ask to open the plug-in help (could be: opening a PDF document or link to a web page).
	    /// The host could call it with onlyCheck set to true for testing support of open Help.
        /// </summary>
        /// <param name="onlyCheck"></param>
        /// <returns>kResultFalse means not supported function.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 OpenHelp(
            [MarshalAs(UnmanagedType.U1), In] Boolean onlyCheck);

        /// <summary>
        /// Host could ask to open the plug-in about box.
	    /// The host could call it with onlyCheck set to true for testing support of open AboutBox.
        /// </summary>
        /// <param name="onlyCheck"></param>
        /// <returns>kResultFalse means not supported function.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 OpenAboutBox(
            [MarshalAs(UnmanagedType.U1), In] Boolean onlyCheck);
    }

    internal static partial class Interfaces
    {
        public const string IEditController2 = "7F4EFE59-F320-4967-AC27-A3AEAFB63038";
    }
}
