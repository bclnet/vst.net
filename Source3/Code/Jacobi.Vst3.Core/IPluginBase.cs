using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Basic interface to a plug-in component: IPluginBase
    /// - initialize/terminate the plug-in component
    /// 
    /// The host uses this interface to initialize and to terminate the plug-in component.
    /// The context that is passed to the initialize method contains any interface to the
    /// host that the plug-in will need to work. These interfaces can vary from category to category.
    /// A list of supported host context interfaces should be included in the documentation
    /// of a specific category.
    /// </summary>
    [ComImport, Guid(Interfaces.IPluginBase), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginBase
    {
        /// <summary>
        /// The host passes a number of interfaces as context to initialize the plug-in class.
        /// \param context, passed by the host, is mandatory and should implement IHostApplication
        /// @note Extensive memory allocations etc. should be performed in this method rather than in
        /// the class' constructor! If the method does NOT return kResultOk, the object is released
        /// immediately. In this case terminate is not called!
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Initialize(
            [MarshalAs(UnmanagedType.IUnknown), In] Object context);

        /// <summary>
        /// This function is called before the plug-in is unloaded and can be used for
	    /// cleanups. You have to release all references to any host application interfaces.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Terminate();
    }

    static partial class Interfaces
    {
        public const string IPluginBase = "22888DDB-156E-45AE-8358-B34808190625";
    }
}
