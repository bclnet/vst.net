namespace Jacobi.Vst3.Core
{
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
