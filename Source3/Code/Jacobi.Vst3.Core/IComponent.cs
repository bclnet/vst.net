using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
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
        new Int32 Initialize(
            [MarshalAs(UnmanagedType.IUnknown), In] Object context);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 Terminate();

        //---------------------------------------------------------------------

        /// <summary>
        /// Called before initializing the component to get information about the controller class.
        /// </summary>
        /// <param name="controllerClassId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetControllerClassId(
            [MarshalAs(UnmanagedType.Struct), Out] out Guid controllerClassId);

        /// <summary>
        /// Called before 'initialize' to set the component usage (optional). See \ref IoModes
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetIoMode(
            [MarshalAs(UnmanagedType.I4), In] IoModes mode);

        /// <summary>
        /// Called after the plug-in is initialized. See \ref MediaTypes, BusDirections
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetBusCount(
            [MarshalAs(UnmanagedType.I4), In] MediaTypes type,
            [MarshalAs(UnmanagedType.I4), In] BusDirections dir);

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
        Int32 GetBusInfo(
            [MarshalAs(UnmanagedType.I4), In] MediaTypes type,
            [MarshalAs(UnmanagedType.I4), In] BusDirections dir, Int32 index,
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
        Int32 GetRoutingInfo(
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
        Int32 ActivateBus(
            [MarshalAs(UnmanagedType.I4), In] MediaTypes type,
            [MarshalAs(UnmanagedType.I4), In] BusDirections dir,
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.U1), In] Boolean state);

        /// <summary>
        /// Activates / deactivates the component.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetActive(
            [MarshalAs(UnmanagedType.U1), In] Boolean state);

        /// <summary>
        /// Sets complete state of component.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetState(
            [MarshalAs(UnmanagedType.Interface), In] IBStream state);

        /// <summary>
        /// Retrieves complete state of component.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetState(
            [MarshalAs(UnmanagedType.Interface), In] IBStream state);
    }

    static partial class Interfaces
    {
        public const string IComponent = "E831FF31-F2D5-4301-928E-BBEE25697802";
    }
}
