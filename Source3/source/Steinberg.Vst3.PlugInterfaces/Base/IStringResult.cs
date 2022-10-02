using System;
using System.Runtime.InteropServices;

namespace Steinberg.Vst3
{
    /// <summary>
    /// Interface to return an ascii string of variable size. 
    /// In order to manage memory allocation and deallocation properly, 
    /// this interface is used to transfer a string as result parameter of
	/// a method requires a string of unknown size. 
    /// </summary>
    [ComImport, Guid(Interfaces.IStringResult), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStringResult
    {
        [PreserveSig]
        void SetText(
            [MarshalAs(UnmanagedType.LPStr), In] String text);
    }

    partial class Interfaces
    {
        public const string IStringResult = "550798BC-8720-49DB-8492-0A153B50B7A8";
    }

    /// <summary>
    /// Interface to a string of variable size and encoding.
    /// </summary>
    [ComImport, Guid(Interfaces.IString), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IString
    {
        /// <summary>
        /// Assign ASCII string
        /// </summary>
        /// <param name="text"></param>
        [PreserveSig]
        void SetText8(
            [MarshalAs(UnmanagedType.LPStr), In] String text);

        /// <summary>
        /// Assign unicode string
        /// </summary>
        /// <param name="text"></param>
        [PreserveSig]
        void SetText16(
            [MarshalAs(UnmanagedType.LPWStr), In] String text);

        /// <summary>
        /// Return ASCII string. If the string is unicode so far, it will be converted.
	    /// So you need to be careful, because the conversion can result in data loss. 
		/// It is save though to call getText8 if isWideString() returns false
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.LPStr)]
        String GetText8();

        /// <summary>
        /// Return unicode string. If the string is ASCII so far, it will be converted.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        String GetText16();

        /// <summary>
        /// !Do not use this method! Early implementations take the given pointer as 
        /// internal string and this will cause problems because 'free' will be used to delete the passed memory.
        /// Later implementations will redirect 'take' to setText8 and setText16
        /// </summary>
        /// <param name="s"></param>
        /// <param name="isWide"></param>
        [PreserveSig]
        void Take(
            [MarshalAs(UnmanagedType.SysInt), In] IntPtr s,
            [MarshalAs(UnmanagedType.U1), In] Boolean isWide);

        /// <summary>
        /// Returns true if the string is in unicode format, returns false if the string is ASCII
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.U1)]
        Boolean IsWideString();
    }

    partial class Interfaces
    {
        public const string IString = "F99DB7A3-0FC1-4821-800B-0CF98E348EDF";
    }
}
