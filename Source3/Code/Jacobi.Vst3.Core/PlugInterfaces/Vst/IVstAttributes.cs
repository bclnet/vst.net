﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Attribute list used in IMessage and IStreamAttributes: Vst::IAttributeList
    /// An attribute list associates values with a key (id: some predefined keys can be found in \ref presetAttributes).
    /// </summary>
    [ComImport, Guid(Interfaces.IAttributeList), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAttributeList
    {
        /// <summary>
        /// Sets integer value.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetInt(
            [MarshalAs(UnmanagedType.LPStr), In] String id,
            [MarshalAs(UnmanagedType.I8), In] Int64 value);

        /// <summary>
        /// Gets integer value.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetInt(
            [MarshalAs(UnmanagedType.LPStr), In] String id,
            [MarshalAs(UnmanagedType.I8), Out] out Int64 value);

        /// <summary>
        /// Sets float value.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetFloat(
            [MarshalAs(UnmanagedType.LPStr), In] String id,
            [MarshalAs(UnmanagedType.R8), In] Double value);

        /// <summary>
        /// Gets float value.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetFloat(
            [MarshalAs(UnmanagedType.LPStr), In] String id,
            [MarshalAs(UnmanagedType.R8), Out] out Double value);

        /// <summary>
        /// Sets string value (UTF16) (must be null-terminated!).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetString(
            [MarshalAs(UnmanagedType.LPStr), In] String id,
            [MarshalAs(UnmanagedType.LPWStr), In] String str);

        /// <summary>
        /// Gets string value (UTF16). Note that Size is in Byte, not the string Length!
		/// Do not forget to multiply the length by sizeof (TChar)!
        /// </summary>
        /// <param name="id"></param>
        /// <param name="str"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetString(
            [MarshalAs(UnmanagedType.LPStr), In] String id,
            [MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 2), In] StringBuilder str,
            [MarshalAs(UnmanagedType.U4), In, Out] ref UInt32 size);

        /// <summary>
        /// Sets binary data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetBinary(
            [MarshalAs(UnmanagedType.LPStr), In] String id,
            [MarshalAs(UnmanagedType.SysInt), In] IntPtr data,
            [MarshalAs(UnmanagedType.U4), In] UInt32 size);

        /// <summary>
        /// Gets binary data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetBinary(
            [MarshalAs(UnmanagedType.LPStr), In] String id,
            [MarshalAs(UnmanagedType.SysInt), In, Out] IntPtr data,
            [MarshalAs(UnmanagedType.U4), In, Out] ref UInt32 size);
    }

    static partial class Interfaces
    {
        public const string IAttributeList = "1E5F0AEB-CC7F-4533-A254-401138AD5EE4";
    }

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
        TResult GetFileName(
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
