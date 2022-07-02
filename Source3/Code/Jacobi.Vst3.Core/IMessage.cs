using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Messages are sent from a VST controller component to a VST editor component and vice versa
    /// </summary>
    [ComImport, Guid(Interfaces.IMessage), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMessage
    {
        /// <summary>
        /// Returns the message ID (for example "TextMessage").
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        String GetMessageID();

        /// <summary>
        /// Sets a message ID (for example "TextMessage").
        /// </summary>
        /// <param name="id"></param>
        [PreserveSig]
        void SetMessageID(
            [MarshalAs(UnmanagedType.LPWStr), In] String id);

        /// <summary>
        /// Returns the attribute list associated to the message.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        IAttributeList GetAttributes();
    }

    internal static partial class Interfaces
    {
        public const string IMessage = "936F033B-C6C0-47DB-BB08-82F813C1E613";
    }
}
