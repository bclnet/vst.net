using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Wrapper MPE Support interface: Vst::IVst3WrapperMPESupport
    /// - passed as 'context' to IPluginBase::Initialize()
    /// Implemented on wrappers that support MPE to Note Expression translation.
    /// 
    /// By default, MPE input processing is enabled, the masterChannel will be zero, the memberBeginChannel
    /// will be one and the memberEndChannel will be 14.
    /// 
    /// As MPE is a subset of the VST3 Note Expression feature, mapping from the three MPE expressions is
    /// handled via the INoteExpressionPhysicalUIMapping interface.
    /// </summary>
    [ComImport, Guid(Interfaces.IVst3WrapperMPESupport), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVst3WrapperMPESupport
    {
        /// <summary>
        /// enable or disable MPE processing
        /// </summary>
        /// <param name="state">true to enable, false to disable MPE processing</param>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 EnableMPEInputProcessing(
            [MarshalAs(UnmanagedType.I4), In] Boolean state);

        /// <summary>
        /// setup the MPE processing
        /// </summary>
        /// <param name="masterChannel">MPE master channel (zero based)</param>
        /// <param name="memberBeginChannel">MPE member begin channel (zero based)</param>
        /// <param name="memberEndChannel">MPE member end channel (zero based)</param>
        /// <returns>kResultTrue on success</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetMPEInputDeviceSettings(
            [MarshalAs(UnmanagedType.I4), In] Int32 masterChannel,
            [MarshalAs(UnmanagedType.I4), In] Int32 memberBeginChannel,
            [MarshalAs(UnmanagedType.I4), In] Int32 memberEndChannel);
    }

    static partial class Interfaces
    {
        public const string IVst3WrapperMPESupport = "44149067-42CF-4BF9-8800-B750F7359FE3";
    }
}
