using System;
using System.Runtime.InteropServices;
using static Steinberg.Vst3.PFactoryInfo;

namespace Steinberg.Vst3
{
    static partial class Constants
    {
        public const FactoryFlags DefaultFactoryFlags = FactoryFlags.Unicode;                       // no programs are used in the unit.
    }

    public enum MediaType
    {
        Audio = 0,		// audio
        Event,			// events
        NumMediaTypes
    }

    public enum BusDirection : int
    {
        Input = 0,		// input bus
        Output			// output bus
    }

    public enum BusType : int
    {
        Main = 0,		// main bus
        Aux			    // auxilliary bus (sidechain)
    }

    [Flags]
    public enum BusFlags : int
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

    /// <summary>
    /// BusInfo:
    /// This is the structure used with getBusInfo, informing the host about what is a specific given bus.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = Platform.StructurePack)]
    public struct BusInfo : IEquatable<BusInfo>
    {
        public static readonly int Size = Marshal.SizeOf<BusInfo>();

        [MarshalAs(UnmanagedType.I4)] public MediaType MediaType;		    // Media type - has to be a value of \ref MediaTypes
        [MarshalAs(UnmanagedType.I4)] public BusDirection Direction;	    // input or output \ref BusDirections
        [MarshalAs(UnmanagedType.I4)] public Int32 ChannelCount;		    // number of channels (if used then need to be recheck after \ref IAudioProcessor::setBusArrangements is called)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Platform.Fixed128)] public String Name; // name of the bus
        [MarshalAs(UnmanagedType.I4)] public BusType BusType;			    // main or aux - has to be a value of \ref BusTypes
        [MarshalAs(UnmanagedType.I4)] public BusFlags Flags;                // flags - a combination of \ref BusFlags

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

    /// <summary>
    /// I/O modes 
    /// </summary>
    public enum IoMode : int
    {
        Simple = 0,		    // 1:1 Input / Output. Only used for Instruments. See \ref vst3IoMode
        Advanced,			// n:m Input / Output. Only used for Instruments. 
        OfflineProcessing	// Plug-in used in an offline processing context
    }

    /// <summary>
    /// Routing Information:
    /// When the plug-in supports multiple I/O busses, a host may want to know how the busses are related. The
    /// relation of an event-input-channel to an audio-output-bus in particular is of interest to the host
    /// (in order to relate MIDI-tracks to audio-channels)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = Platform.StructurePack)]
    public struct RoutingInfo
    {
        public static readonly int Size = Marshal.SizeOf<RoutingInfo>();

        [MarshalAs(UnmanagedType.I4)] public MediaType MediaType;	// media type see \ref MediaTypes
        [MarshalAs(UnmanagedType.I4)] public Int32 BusIndex;		// bus index
        [MarshalAs(UnmanagedType.I4)] public Int32 Channel;			// channel (-1 for all channels)
    }

    /// <summary>
    /// Component base interface: Vst::IComponent
    /// This is the basic interface for a VST component and must always be supported.
    /// It contains the common parts of any kind of processing class. The parts that
    /// are specific to a media type are defined in a separate interface. An implementation
    /// component must provide both the specific interface and IComponent.
    /// </summary>
    [ComImport, Guid(Interfaces.IComponent), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponent : IPluginBase
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new TResult Initialize(
            [MarshalAs(UnmanagedType.IUnknown), In] Object context);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new TResult Terminate();

        //---------------------------------------------------------------------

        /// <summary>
        /// Called before initializing the component to get information about the controller class.
        /// </summary>
        /// <param name="controllerClassId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetControllerClassId(
            [MarshalAs(UnmanagedType.Struct), Out] out Guid controllerClassId);

        /// <summary>
        /// Called before 'initialize' to set the component usage (optional). See \ref IoModes
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetIoMode(
            [MarshalAs(UnmanagedType.I4), In] IoMode mode);

        /// <summary>
        /// Called after the plug-in is initialized. See \ref MediaTypes, BusDirections
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetBusCount(
            [MarshalAs(UnmanagedType.I4), In] MediaType type,
            [MarshalAs(UnmanagedType.I4), In] BusDirection dir);

        /// <summary>
        /// Called after the plug-in is initialized. See \ref MediaTypes, BusDirections
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dir"></param>
        /// <param name="index"></param>
        /// <param name="bus"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetBusInfo(
            [MarshalAs(UnmanagedType.I4), In] MediaType type,
            [MarshalAs(UnmanagedType.I4), In] BusDirection dir, Int32 index,
            [MarshalAs(UnmanagedType.Struct), Out] out BusInfo bus);

        /// <summary>
        /// Retrieves routing information (to be implemented when more than one regular input or output bus exists).
	    /// The inInfo always refers to an input bus while the returned outInfo must refer to an output bus!
        /// </summary>
        /// <param name="inInfo"></param>
        /// <param name="outInfo"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetRoutingInfo(
            [MarshalAs(UnmanagedType.Struct), In] ref RoutingInfo inInfo,
            [MarshalAs(UnmanagedType.Struct), Out] out RoutingInfo outInfo);

        /// <summary>
        /// Called upon (de-)activating a bus in the host application. The plug-in should only processed
        /// an activated bus, the host could provide less see \ref AudioBusBuffers in the process call
        /// (see \ref IAudioProcessor::process) if last busses are not activated. An already activated bus 
        /// does not need to be reactivated after a IAudioProcessor::setBusArrangements call.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dir"></param>
        /// <param name="index"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult ActivateBus(
            [MarshalAs(UnmanagedType.I4), In] MediaType type,
            [MarshalAs(UnmanagedType.I4), In] BusDirection dir,
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.U1), In] Boolean state);

        /// <summary>
        /// Activates / deactivates the component.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetActive(
            [MarshalAs(UnmanagedType.U1), In] Boolean state);

        /// <summary>
        /// Sets complete state of component.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetState(
            [MarshalAs(UnmanagedType.Interface), In] IBStream state);

        /// <summary>
        /// Retrieves complete state of component.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetState(
            [MarshalAs(UnmanagedType.Interface), In] IBStream state);
    }

    partial class Interfaces
    {
        public const string IComponent = "E831FF31-F2D5-4301-928E-BBEE25697802";
    }
}
