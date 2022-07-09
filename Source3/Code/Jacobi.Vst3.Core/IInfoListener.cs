using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Channel context interface: Vst::IInfoListener
    /// Allows the host to inform the plug-in about the context in which the plug-in is instantiated,
    /// mainly channel based info(color, name, index,...). Index can be defined inside a namespace
    /// (for example, index start from 1 to N for Type Input/Output Channel(Index namespace) and index
    /// start from 1 to M for Type Audio Channel).
    /// As soon as the plug-in provides this IInfoListener interface, the host will call setChannelContextInfos 
    /// for each change occurring to this channel (new name, new color, new indexation,...)
    /// </summary>
    [ComImport, Guid(Interfaces.IInfoListener), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInfoListener
    {
        /// <summary>
        /// Receive the channel context infos from host.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetChannelContextInfos(
            [MarshalAs(UnmanagedType.Interface), In] IAttributeList list);
    }

    internal static partial class Interfaces
    {
        public const string IInfoListener = "0F194781-8D98-4ADA-BBA0-C1EFC011D8D0";
    }
}
