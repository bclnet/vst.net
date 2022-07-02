using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
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
}
