using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// BusInfo:
    /// This is the structure used with getBusInfo, informing the host about what is a specific given bus.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = Platform.StructurePack)]
    public struct BusInfo : IEquatable<BusInfo>
    {
        public static readonly int Size = Marshal.SizeOf<BusInfo>();

        [MarshalAs(UnmanagedType.I4)] public MediaTypes MediaType;		    // Media type - has to be a value of \ref MediaTypes
        [MarshalAs(UnmanagedType.I4)] public BusDirections Direction;	    // input or output \ref BusDirections
        [MarshalAs(UnmanagedType.I4)] public Int32 ChannelCount;		    // number of channels (if used then need to be recheck after \ref IAudioProcessor::setBusArrangements is called)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.String128)] public String Name; // name of the bus
        [MarshalAs(UnmanagedType.I4)] public BusTypes BusType;			    // main or aux - has to be a value of \ref BusTypes
        [MarshalAs(UnmanagedType.I4)] public BusFlags Flags;                // flags - a combination of \ref BusFlags

        [Flags]
        public enum BusFlags
        {
            //None = 0,
            /// <summary>
            /// The bus should be activated by the host per default on instantiation (activateBus call is requested).
 		 	/// By default a bus is inactive.
            /// </summary>
            DefaultActive = 1 << 0,
            /// <summary>
            /// The bus does not contain ordinary audio data, but data used for control changes at sample rate.
            /// The data is in the same format as the audio data [-1..1].
            /// A host has to prevent unintended routing to speakers to prevent damage.
            /// Only valid for audio media type busses.
            /// [released: 3.7.0]
            /// </summary>
            IsControlVoltage = 1 << 1,
        }

        public void Clear()
        {
            MediaType = 0;
            Direction = 0;
            ChannelCount = 0;
            Name = null;
            BusType = 0;
            Flags = 0;
        }

        public override bool Equals(object obj) => Equals((BusInfo)obj);
        public bool Equals(BusInfo other) => MediaType == other.MediaType && Direction == other.Direction && ChannelCount == other.ChannelCount && Name == other.Name && BusType == other.BusType && Flags == other.Flags;
        public override int GetHashCode() => (MediaType, Direction, ChannelCount, Name, BusType, Flags).GetHashCode();
        public static bool operator ==(BusInfo lhs, BusInfo rhs) => lhs.Equals(rhs);
        public static bool operator !=(BusInfo lhs, BusInfo rhs) => !lhs.Equals(rhs);
    }
}
