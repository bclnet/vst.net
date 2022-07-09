using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Edit controller component interface: Vst::IEditController
    /// The controller part of an effect or instrument with parameter handling (export, definition, conversion...).
    /// </summary>
    [ComImport, Guid(Interfaces.IEditController), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEditController : IPluginBase
    {
        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.Error)]
        //Int32 Initialize(
        //    [MarshalAs(UnmanagedType.IUnknown), In] Object context);

        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.Error)]
        //Int32 Terminate();

        //---------------------------------------------------------------------

        /// <summary>
        /// Receives the component state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetComponentState(
            [MarshalAs(UnmanagedType.Interface), In] IBStream state);

        /// <summary>
        /// Sets the controller state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetState(
            [MarshalAs(UnmanagedType.Interface), In] IBStream state);

        /// <summary>
        /// Gets the controller state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetState(
            [MarshalAs(UnmanagedType.Interface), In] IBStream state);

        // parameters -------------------------

        /// <summary>
        /// Returns the number of parameters exported.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetParameterCount();

        /// <summary>
        /// Gets for a given index the parameter information.
        /// </summary>
        /// <param name="paramIndex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetParameterInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 paramIndex,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref ParameterInfo info);

        /// <summary>
        /// Gets for a given paramID and normalized value its associated string representation.
        /// </summary>
        /// <param name="paramId"></param>
        /// <param name="valueNormalized"></param>
        /// <param name="string"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetParamStringByValue(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId,
            [MarshalAs(UnmanagedType.R8), In] Double valueNormalized,
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = Constants.String128), Out] StringBuilder @string);

        /// <summary>
        /// Gets for a given paramID and string its normalized value.
        /// </summary>
        /// <param name="paramId"></param>
        /// <param name="string"></param>
        /// <param name="valueNormalized"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetParamValueByString(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId,
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = Constants.String128), In] String @string,
            [MarshalAs(UnmanagedType.R8), In, Out] ref Double valueNormalized);

        /// <summary>
        /// Returns for a given paramID and a normalized value its plain representation (for example -6 for -6dB - see \ref vst3AutomationIntro).
        /// </summary>
        /// <param name="paramId"></param>
        /// <param name="valueNormalized"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.R8)]
        Double NormalizedParamToPlain(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId,
            [MarshalAs(UnmanagedType.R8), In] Double valueNormalized);

        /// <summary>
        /// Returns for a given paramID and a plain value its normalized value. (see \ref vst3AutomationIntro)
        /// </summary>
        /// <param name="paramId"></param>
        /// <param name="plainValue"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.R8)]
        Double PlainParamToNormalized(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId,
            [MarshalAs(UnmanagedType.R8), In] Double plainValue);

        /// <summary>
        /// Returns the normalized value of the parameter associated to the paramID.
        /// </summary>
        /// <param name="paramId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.R8)]
        Double GetParamNormalized(
            [MarshalAs(UnmanagedType.U4), In] UInt32 paramId);

        /// <summary>
        /// Sets the normalized value to the parameter associated to the paramID. The controller must never
	    /// pass this value-change back to the host via the IComponentHandler. It should update the according
        /// GUI element(s) only!
        /// </summary>
        /// <param name="paramId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetParamNormalized(
            [MarshalAs(UnmanagedType.I4), In] UInt32 paramId,
            [MarshalAs(UnmanagedType.R8), In] Double value);

        // handler ----------------------------

        /// <summary>
        /// Gets from host a handler which allows the Plugin-in to communicate with the host.
		/// Note: This is mandatory if the host is using the IEditController!
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetComponentHandler(
            [MarshalAs(UnmanagedType.Interface), In] IComponentHandler handler);

        /// <summary>
        /// Creates the editor view of the plug-in, currently only "editor" is supported, see \ref ViewType.
		/// The life time of the editor view will never exceed the life time of this controller instance.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        IPlugView CreateView(
            [MarshalAs(UnmanagedType.LPStr), In] String name);
    }

    internal static partial class Interfaces
    {
        public const string IEditController = "DCD7BBE3-7742-448D-A874-AACC979C759E";
    }
}
