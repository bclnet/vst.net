using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Connect a component with another one: Vst::IConnectionPoint
    /// 
    /// This interface is used for the communication of separate components.
    /// Note that some hosts will place a proxy object between the components so that they are not directly connected.
    /// </summary>
    [ComImport, Guid(Interfaces.IConnectionPoint), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IConnectionPoint
    {
        /// <summary>
        /// Connects this instance with another connection point.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Connect(
            [MarshalAs(UnmanagedType.Interface), In] IConnectionPoint other);

        /// <summary>
        /// Disconnects a given connection point from this.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Disconnect(
            [MarshalAs(UnmanagedType.Interface), In] IConnectionPoint other);

        /// <summary>
        /// Called when a message has been sent from the connection point to this.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Notify(
            [MarshalAs(UnmanagedType.Interface), In] IMessage message);
    }

    static partial class Interfaces
    {
        public const string IConnectionPoint = "70A4156F-6E6E-4026-9891-48BFAA60D8D1";
    }
}
