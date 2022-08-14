using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Basic host callback interface: Vst::IHostApplication
    /// Basic VST host application interface.
    /// </summary>
    [ComImport, Guid(Interfaces.IHostApplication), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IHostApplication
    {
        /// <summary>
        /// Gets host application name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetName(
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = Constants.Fixed128), In] StringBuilder name);

        /// <summary>
        /// Creates host object (e.g. Vst::IMessage).
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="interfaceId"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 CreateInstance(
            [MarshalAs(UnmanagedType.Struct), In] ref Guid classId,
            [MarshalAs(UnmanagedType.Struct), In] ref Guid interfaceId,
            [MarshalAs(UnmanagedType.SysInt, IidParameterIndex = 1), Out] out IntPtr instance);
    }

    static partial class Interfaces
    {
        public const string IHostApplication = "58E595CC-DB2D-4969-8B6A-AF8C36A664E5";
    }

    /// <summary>
    /// VST 3 to VST 2 Wrapper interface: Vst::IVst3ToVst2Wrapper
    /// - passed as 'context' to IPluginBase::Initialize()
    /// Informs the plug-in that a VST 3 to VST 2 wrapper is used between the plug-in and the real host.
    /// Implemented by the VST 2 Wrapper.
    /// </summary>
    [ComImport, Guid(Interfaces.IVst3ToVst2Wrapper), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVst3ToVst2Wrapper { }

    static partial class Interfaces
    {
        public const string IVst3ToVst2Wrapper = "29633AEC-1D1C-47E2-BB85-B97BD36EAC61";
    }

    /// <summary>
    /// VST 3 to AU Wrapper interface: Vst::IVst3ToAUWrapper
    /// - passed as 'context' to IPluginBase::Initialize()
    /// Informs the plug-in that a VST 3 to AU wrapper is used between the plug-in and the real host.
    /// Implemented by the AU Wrapper.
    /// </summary>
    [ComImport, Guid(Interfaces.IVst3ToAUWrapper), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVst3ToAUWrapper { }

    static partial class Interfaces
    {
        public const string IVst3ToAUWrapper = "A3B8C6C5-C095-4688-B091-6F0BB697AA44";
    }

    /// <summary>
    /// VST 3 to AAX Wrapper interface: Vst::IVst3ToAAXWrapper
    /// - passed as 'context' to IPluginBase::Initialize()
    /// Informs the plug-in that a VST 3 to AAX wrapper is used between the plug-in and the real host.
    /// Implemented by the AAX Wrapper.
    /// </summary>
    [ComImport, Guid(Interfaces.IVst3ToAAXWrapper), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVst3ToAAXWrapper { }

    static partial class Interfaces
    {
        public const string IVst3ToAAXWrapper = "6D319DC6-60C5-6242-B32C-951B93BEF4C6";
    }

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
