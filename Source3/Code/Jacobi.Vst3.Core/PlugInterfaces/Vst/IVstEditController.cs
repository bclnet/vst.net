using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    static partial class Constants
    {
        public const string kVstComponentControllerClass = "Component Controller Class";
    }

    /// <summary>
    /// Controller Parameter Info.
    /// A parameter info describes a parameter of the controller.
    /// The id must always be the same for a parameter as this uniquely identifies the parameter.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = Platform.StructurePack)]
    public struct ParameterInfo
    {
        public static readonly int Size = Marshal.SizeOf<ParameterInfo>();

        public const UInt32 NoParamId = UInt32.MaxValue;

        [MarshalAs(UnmanagedType.U4)] public UInt32 Id;                     // unique identifier of this parameter (named tag too)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.Fixed128)] public String Title;       // parameter title (e.g. "Volume")
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.Fixed128)] public String ShortTitle;  // parameter shortTitle (e.g. "Vol")
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.Fixed128)] public String Units;       // parameter unit (e.g. "dB")
        [MarshalAs(UnmanagedType.I4)] public Int32 StepCount;               // number of discrete steps (0: continuous, 1: toggle, discrete value otherwise 
                                                                            // (corresponding to max - min, for example: 127 for a min = 0 and a max = 127) - see \ref vst3ParameterIntro)
        [MarshalAs(UnmanagedType.R8)] public Double DefaultNormalizedValue; // default normalized value [0,1] (in case of discrete value: defaultNormalizedValue = defDiscreteValue / stepCount)
        [MarshalAs(UnmanagedType.I4)] public Int32 UnitId;                  // id of unit this parameter belongs to (see \ref vst3Units)
        [MarshalAs(UnmanagedType.I4)] public ParameterFlags Flags;          // ParameterFlags (see below)

        [Flags]
        public enum ParameterFlags
        {
            None = 0,                   // no flags wanted
            CanAutomate = 1 << 0,       // parameter can be automated
            IsReadOnly = 1 << 1,        // parameter cannot be changed from outside the plug-in (implies that kCanAutomate is NOT set)
            IsWrapAround = 1 << 2,      // attempts to set the parameter value out of the limits will result in a wrap around [SDK 3.0.2]
            IsList = 1 << 3,            // parameter should be displayed as list in generic editor or automation editing [SDK 3.1.0]
            IsHidden = 1 << 4,          // parameter should be NOT displayed and cannot be changed from outside the plug-in 
                                        // (implies that kCanAutomate is NOT set and kIsReadOnly is set) [SDK 3.7.0]

            IsProgramChange = 1 << 15,  // parameter is a program change (unitId gives info about associated unit - see \ref vst3ProgramLists)
            IsBypass = 1 << 16          // special bypass parameter (only one allowed): plug-in can handle bypass
                                        // (highly recommended to export a bypass parameter for effect plug-in)
        }
    }

    /// <summary>
    /// View Types used for IEditController::createView
    /// </summary>
    public static class ViewType
    {
        public const string Editor = "editor";
    }

    /// <summary>
    /// Flags used for IComponentHandler::restartComponent
    /// </summary>
    [Flags]
    public enum RestartFlags
    {
        //None = 0,
        ReloadComponent = 1 << 0,	        // The Component should be reloaded
        IoChanged = 1 << 1,	                // Input and/or Output Bus configuration has changed
        ParamValuesChanged = 1 << 2,	    // Multiple parameter values have changed (as result of a program change for example) 
        LatencyChanged = 1 << 3,	        // Latency has changed (IAudioProcessor.getLatencySamples)
        ParamTitlesChanged = 1 << 4,	    // Parameter titles, default values or flags (ParameterFlags) have changed [SDK 3.0.0]
        MidiCCAssignmentChanged = 1 << 5,	// MIDI Controllers and/or Program Changes Assignments have changed [SDK 3.0.1]
        NoteExpressionChanged = 1 << 6,	    // Note Expression has changed (info, count, PhysicalUIMapping, ...) [SDK 3.5.0]
        IoTitlesChanged = 1 << 7,	        // Input / Output bus titles have changed [SDK 3.5.0]
        IoPrefetchableSupportChanged = 1 << 8, // Prefetch support has changed [SDK 3.6.1]
        RoutingInfoChanged = 1 << 9,	    // RoutingInfo has changed [SDK 3.6.6]
        KeyswitchChanged = 1 << 10,	        // Key switches has changed (info, count) [SDK 3.7.3]
    }

    /// <summary>
    /// Host callback interface for an edit controller: Vst::IComponentHandler
    /// Allow transfer of parameter editing to component (processor) via host and support automation.
    /// Cause the host to react on configuration changes(restartComponent).
    /// </summary>
    [ComImport, Guid(Interfaces.IComponentHandler), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponentHandler
    {
        /// <summary>
        /// To be called before calling a performEdit (e.g. on mouse-click-down event).
        /// This must be called in the UI-Thread context!
        /// </summary>
        /// <param name="paramId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult BeginEdit(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId);

        /// <summary>
        /// Called between beginEdit and endEdit to inform the handler that a given parameter has a new
        /// value.This must be called in the UI-Thread context!
        /// </summary>
        /// <param name="paramId"></param>
        /// <param name="valueNormalized"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult PerformEdit(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId,
            [MarshalAs(UnmanagedType.R8), In] Double valueNormalized);

        /// <summary>
        /// To be called after calling a performEdit (e.g. on mouse-click-up event).
        /// This must be called in the UI-Thread context!
        /// </summary>
        /// <param name="paramId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult EndEdit(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId);

        /// <summary>
        /// Instructs host to restart the component. This must be called in the UI-Thread context!
        /// </summary>
        /// <param name="flags">flags is a combination of RestartFlags</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult RestartComponent(
            [MarshalAs(UnmanagedType.I4), In] RestartFlags flags);
    }

    static partial class Interfaces
    {
        public const string IComponentHandler = "93A0BEA3-0BD0-45DB-8E89-0B0CC1E46AC6";
    }

    /// <summary>
    /// Extended host callback interface for an edit controller: Vst::IComponentHandler2
    /// </summary>
    [ComImport, Guid(Interfaces.IComponentHandler2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponentHandler2
    {
        /// <summary>
        /// Tells host that the plug-in is dirty (something besides parameters has changed since last save),
        /// if true the host should apply a save before quitting.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetDirty(
            [MarshalAs(UnmanagedType.U1), In] Boolean state);

        /// <summary>
        /// Tells host that it should open the plug-in editor the next time it's possible.
        /// You should use this instead of showing an alert and blocking the program flow (especially on loading projects).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult RequestOpenEditor(
            [MarshalAs(UnmanagedType.LPStr), In] String name);

        /// <summary>
        /// Starts the group editing (call before a \ref IComponentHandler::beginEdit),
        /// the host will keep the current timestamp at this call and will use it for all \ref IComponentHandler::beginEdit
        /// \ref IComponentHandler::performEdit / \ref IComponentHandler::endEdit calls until a \ref finishGroupEdit().
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult StartGroupEdit();

        /// <summary>
        /// Finishes the group editing started by a \ref startGroupEdit (call after a \ref IComponentHandler::endEdit).
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult FinishGroupEdit();
    }

    static partial class Interfaces
    {
        public const string IComponentHandler2 = "F040B4B3-A360-45EC-ABCD-C045B4D5A2CC";
    }

    /// <summary>
    /// Extended host callback interface for an edit controller: Vst::IComponentHandlerBusActivation
    /// Allows the plug-in to request the host to activate or deactivate a specific bus. 
    /// If the host accepts this request, it will call later on \ref IComponent::activateBus.
    /// This is particularly useful for instruments with more than 1 outputs, where the user could request
    /// from the plug-in UI a given output bus activation.
    /// </summary>
    [ComImport, Guid(Interfaces.IComponentHandlerBusActivation), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponentHandlerBusActivation
    {
        /// <summary>
        /// request the host to activate or deactivate a specific bus.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dir"></param>
        /// <param name="index"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult RequestBusActivation(
            [MarshalAs(UnmanagedType.I4), In] MediaTypes type,
            [MarshalAs(UnmanagedType.I4), In] BusDirections dir,
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.I4), In] Boolean state);
    }

    static partial class Interfaces
    {
        public const string IComponentHandlerBusActivation = "067D02C1-5B4E-274D-A92D-90FD6EAF7240";
    }

    /// <summary>
    /// A component can support unit preset data via this interface or program list data(IProgramListData).
    /// </summary>
    [ComImport, Guid(Interfaces.IProgress), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IProgress
    {
        /// <summary>
        /// Start a new progress of a given type and optional Description. outID is as ID created by the
        /// host to identify this newly created progress (for update and finish method)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="optionalDescription"></param>
        /// <param name="outID"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult Start(
            [MarshalAs(UnmanagedType.U4), In] ProgressType type,
            [MarshalAs(UnmanagedType.LPWStr), In] string optionalDescription,
            [MarshalAs(UnmanagedType.U8), Out] out UInt64 outID);

        /// <summary>
        /// Update the progress value (normValue between [0, 1]) associated to the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="normValue"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult Update(
            [MarshalAs(UnmanagedType.U8), In] UInt64 id,
            [MarshalAs(UnmanagedType.R8), In] Double normValue);

        /// <summary>
        /// Finish the progress associated to the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult Finish(
            [MarshalAs(UnmanagedType.U8), In] UInt64 id);

        public enum ProgressType : uint
        {
            AsyncStateRestoration = 0,  ///< plug-in state is restored async (in a background Thread)
            UIBackgroundTask            ///< a plug-in task triggered by a UI action 
        }
    }

    static partial class Interfaces
    {
        public const string IProgress = "00C9DC5B-9D90-4254-91A3-88C8B4E91B69";
    }

    /// <summary>
    /// Knob Mode
    /// </summary>
    public enum KnobMode : uint
    {
        CircularMode = 0,		// Circular with jump to clicked position
        RelativCircularMode,	// Circular without jump to clicked position
        LinearMode				// Linear: depending on vertical movement
    }

    /// <summary>
    /// Edit controller component interface extension: Vst::IEditController2
    /// Extension to allow the host to inform the plug-in about the host Knob Mode,
    /// and to open the plug-in about box or help documentation.
    /// </summary>
    [ComImport, Guid(Interfaces.IEditController2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEditController2
    {
        /// <summary>
        /// Host could set the Knob Mode for the plug-in.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns>kResultFalse means not supported mode.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetKnobMode(
            [MarshalAs(UnmanagedType.I4), In] KnobMode mode);

        /// <summary>
        /// Host could ask to open the plug-in help (could be: opening a PDF document or link to a web page).
	    /// The host could call it with onlyCheck set to true for testing support of open Help.
        /// </summary>
        /// <param name="onlyCheck"></param>
        /// <returns>kResultFalse means not supported function.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult OpenHelp(
            [MarshalAs(UnmanagedType.U1), In] Boolean onlyCheck);

        /// <summary>
        /// Host could ask to open the plug-in about box.
	    /// The host could call it with onlyCheck set to true for testing support of open AboutBox.
        /// </summary>
        /// <param name="onlyCheck"></param>
        /// <returns>kResultFalse means not supported function.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult OpenAboutBox(
            [MarshalAs(UnmanagedType.U1), In] Boolean onlyCheck);
    }

    static partial class Interfaces
    {
        public const string IEditController2 = "7F4EFE59-F320-4967-AC27-A3AEAFB63038";
    }

    /// <summary>
    /// MIDI Mapping interface: Vst::IMidiMapping
    /// MIDI controllers are not transmitted directly to a VST component. MIDI as hardware protocol has
    /// restrictions that can be avoided in software.Controller data in particular come along with unclear
    /// and often ignored semantics.On top of this they can interfere with regular parameter automation and
    /// the host is unaware of what happens in the plug-in when passing MIDI controllers directly.
    /// </summary>
    [ComImport, Guid(Interfaces.IMidiMapping), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMidiMapping
    {
        /// <summary>
        /// Gets an (preferred) associated ParamID for a given Input Event Bus index, channel and MIDI Controller.
        /// </summary>
        /// <param name="busIndex">index of Input Event Bus</param>
        /// <param name="channel">channel of the bus</param>
        /// <param name="midiControllerNumber">see \ref ControllerNumbers for expected values (could be bigger than 127)</param>
        /// <param name="id">return the associated ParamID to the given midiControllerNumber</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetMidiControllerAssignment(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.I2), In] ControllerNumbers midiControllerNumber,
            [MarshalAs(UnmanagedType.U4), In, Out] ref UInt32 id);
    }

    static partial class Interfaces
    {
        public const string IMidiMapping = "DF0FF9F7-49B7-4669-B63A-B7327ADBF5E5";
    }

    /// <summary>
    /// Parameter Editing from host: Vst::IEditControllerHostEditing
    /// If this interface is implemented by the edit controller, and when performing edits from outside
    /// the plug-in (host / remote) of a not automatable and not read-only, and not hidden flagged parameter(kind of helper parameter),
    /// the host will start with a beginEditFromHost before calling setParamNormalized and end with an endEditFromHost.
    /// </summary>
    [ComImport, Guid(Interfaces.IEditControllerHostEditing), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEditControllerHostEditing
    {
        /// <summary>
        /// Called before a setParamNormalized sequence, a endEditFromHost will be call at the end of the editing action.
        /// </summary>
        /// <param name="paramID"></param>
        /// <returns></returns>
        Int32 BeginEditFromHost(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramID);

        /// <summary>
        /// Called after a beginEditFromHost and a sequence of setParamNormalized.
        /// </summary>
        /// <param name="paramID"></param>
        /// <returns></returns>
        Int32 EndEditFromHost(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramID);
    }

    static partial class Interfaces
    {
        public const string IEditControllerHostEditing = "C1271208-7059-4098-B9DD-34B36BB0195E";
    }
}
