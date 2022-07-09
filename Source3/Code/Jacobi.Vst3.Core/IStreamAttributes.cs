using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Meta attributes of a stream: Vst::IStreamAttributes
    /// Interface to access preset meta information from stream, used, for example, in setState in order to inform the plug-in about
    /// the current context in which the preset loading occurs (Project context or Preset load (see \ref StateType))
    /// or used to get the full file path of the loaded preset (if available).
    /// </summary>
    [ComImport, Guid(Interfaces.IStreamAttributes), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStreamAttributes
    {
        /// <summary>
        /// Gets filename (without file extension) of the stream.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetFileName(
            [MarshalAs(UnmanagedType.LPStr), In, Out] String name);

        /// <summary>
        /// Gets meta information list.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        IAttributeList GetAttributes();
    }

    static partial class Interfaces
    {
        public const string IStreamAttributes = "D6CE2FFC-EFAF-4B8C-9E74-F1BB12DA44B4";
    }
}
