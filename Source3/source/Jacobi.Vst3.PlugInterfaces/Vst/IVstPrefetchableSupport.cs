using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3
{
    public enum PrefetchableSupport : uint
    {
        IsNeverPrefetchable = 0,   // every instance of the plug does not support prefetch processing
        IsYetPrefetchable,         // in the current state the plug support prefetch processing
        IsNotYetPrefetchable,      // in the current state the plug does not support prefetch processing
        NumPrefetchableSupport
    }

    /// <summary>
    /// Indicates that the plug-in could or not support Prefetch (dynamically): Vst::IPrefetchableSupport
    /// 
    /// The plug-in should implement this interface if it needs to dynamically change between prefetchable or not.
    /// By default (without implementing this interface) the host decides in which mode the plug-in is processed.
    /// For more info about the prefetch processing mode check the ProcessModes::kPrefetch documentation.
    /// </summary>
    [ComImport, Guid(Interfaces.IPrefetchableSupport), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPrefetchableSupport
    {
        /// <summary>
        /// retrieve the current prefetch support. Use IComponentHandler::restartComponent
        /// (kPrefetchableSupportChanged) to inform the host that this support has changed.
        /// </summary>
        /// <param name="prefetchable"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetPrefetchableSupport(
            [MarshalAs(UnmanagedType.U4), In, Out] ref PrefetchableSupport prefetchable);
    }

    partial class Interfaces
    {
        public const string IPrefetchableSupport = "8AE54FDA-E930-46B9-A285-55BCDC98E21E";
    }
}
