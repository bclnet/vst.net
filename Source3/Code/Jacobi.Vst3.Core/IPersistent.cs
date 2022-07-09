using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Persistent Object Interface.
    /// This interface is used to store/restore attributes of an object.
    /// An IPlugController can implement this interface to handle presets.
    /// </summary>
    [ComImport, Guid(Interfaces.IPersistent), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPersistent
    {
        /// <summary>
        /// The class ID must be a 16 bytes unique id that is used to create the object. 
        /// This ID is also used to identify the preset list when used with presets.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetClassID(
            [MarshalAs(UnmanagedType.LPStr), In, Out] ref String uid);

        /// <summary>
        /// Store all members/data in the passed IAttributes.
        /// </summary>
        /// <param name="attrs"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SaveAttributes(
            [MarshalAs(UnmanagedType.Interface), In] IAttributes attrs);

        /// <summary>
        /// Restore all members/data from the passed IAttributes.
        /// </summary>
        /// <param name="attrs"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 LoadAttributes(
            [MarshalAs(UnmanagedType.Interface), In] IAttributes attrs);
    }

    internal static partial class Interfaces
    {
        public const string IPersistent = "BA1A4637-3C9F-46D0-A65D-BA0EB85DA829";
    }
}
