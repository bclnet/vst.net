using System;
using System.Runtime.InteropServices;
using ColorSpec = System.UInt32;
using ColorComponent = System.Byte;
using System.Runtime.CompilerServices;

namespace Jacobi.Vst3
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
        TResult SetChannelContextInfos(
            [MarshalAs(UnmanagedType.Interface), In] IAttributeList list);
    }

    partial class Interfaces
    {
        public const string IInfoListener = "0F194781-8D98-4ADA-BBA0-C1EFC011D8D0";
    }

    /// <summary>
    /// Values used for kChannelPluginLocationKey
    /// </summary>
    public enum ChannelPluginLocation
    {
        PreVolumeFader = 0,
        PostVolumeFader,
        UsedAsPanner
    }

    public static class ColorSpecX
    {
        /// <summary>
        /// Returns the Blue part of the given ColorSpec 
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorComponent GetBlue(ColorSpec cs) => (ColorComponent)(cs & 0x000000FF);

        /// <summary>
        /// Returns the Green part of the given ColorSpec
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorComponent GetGreen(ColorSpec cs) => (ColorComponent)((cs >> 8) & 0x000000FF);

        /// <summary>
        /// Returns the Red part of the given ColorSpec
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorComponent GetRed(ColorSpec cs) => (ColorComponent)((cs >> 16) & 0x000000FF);

        /// <summary>
        /// Returns the Alpha part of the given ColorSpec
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorComponent GetAlpha(ColorSpec cs) => (ColorComponent)((cs >> 24) & 0x000000FF);
    }

    /// <summary>
    /// Keys used as AttrID (Attribute ID) in the return IAttributeList of IInfoListener::setChannelContextInfos
    /// </summary>
    public static class ChannelContextKey
    {
        /// <summary>
        /// string (TChar) [optional]: unique id string used to identify a channel
        /// </summary>
        public const string kChannelUIDKey = "channel uid";

        /// <summary>
        /// integer (int64) [optional]: number of characters in kChannelUIDKey
        /// </summary>
        public const string kChannelUIDLengthKey = "channel uid length";

        /// <summary>
        /// string (TChar) [optional]: name of the channel like displayed in the mixer
        /// </summary>
        public const string kChannelNameKey = "channel name";

        /// <summary>
        /// integer (int64) [optional]: number of characters in kChannelNameKey
        /// </summary>
        public const string kChannelNameLengthKey = "channel name length";

        /// <summary>
        /// color (ColorSpec) [optional]: used color for the channel in mixer or track
        /// </summary>
        public const string kChannelColorKey = "channel color";

        /// <summary>
        /// integer (int64) [optional]: index of the channel in a channel index namespace, start with 1 not * 0!
        /// </summary>
        public const string kChannelIndexKey = "channel index";

        /// <summary>
        /// integer (int64) [optional]: define the order of the current used index namespace, start with 1 not 0!
        /// For example:
        /// index namespace is "Input"   -> order 1,
        /// index namespace is "Channel" -> order 2,
        /// index namespace is "Output"  -> order 3
        /// </summary>
        public const string kChannelIndexNamespaceOrderKey = "channel index namespace order";

        /// <summary>
        /// string (TChar) [optional]: name of the channel index namespace for example "Input", "Output", "Channel", ...
        /// </summary>
        public const string kChannelIndexNamespaceKey = "channel index namespace";

        /// <summary>
        /// integer (int64) [optional]: number of characters in kChannelIndexNamespaceKey
        /// </summary>
        public const string kChannelIndexNamespaceLengthKey = "channel index namespace length";

        /// <summary>
        /// PNG image representation as binary [optional]
        /// </summary>
        public const string kChannelImageKey = "channel image";

        /// <summary>
        /// integer (int64) [optional]: routing position of the plug-in in the channel (see ChannelPluginLocation)
        /// </summary>
        public const string kChannelPluginLocationKey = "channel plugin location";
    }
}
