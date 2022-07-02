using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
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
}
