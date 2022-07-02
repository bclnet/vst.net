using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Extended plug-in interface IEditController for key switches support: Vst::IKeyswitchController
    /// 
    /// When a (instrument) plug-in supports such interface, the host could get from the plug-in the current set
    /// of used key switches(megatrig/articulation) for a given channel of a event bus and then automatically use them(like in Cubase 6) to
    /// create VST Expression Map(allowing to associated symbol to a given articulation / key switch).
    /// </summary>
    [ComImport, Guid(Interfaces.IKeyswitchController), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IKeyswitchController
    {
        /// <summary>
        /// Returns number of supported key switches for event bus index and channel.
        /// </summary>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetKeyswitchCount(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel);

        /// <summary>
        /// Returns key switch info.
        /// </summary>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <param name="keySwitchIndex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetKeyswitchInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.I4), In] Int32 keySwitchIndex,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref KeyswitchInfo info);
    }

    internal static partial class Interfaces
    {
        public const string IKeyswitchController = "1F2F76D3-BFFB-4B96-B995-27A55EBCCEF4";
    }
}
