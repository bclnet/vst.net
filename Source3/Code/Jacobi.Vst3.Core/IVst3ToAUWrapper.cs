using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
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
}
