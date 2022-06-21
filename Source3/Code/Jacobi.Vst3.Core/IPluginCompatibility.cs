using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// optional interface to query the compatibility of the plug-ins classes
    /// - [plug imp]
    /// 
    /// A plug-in can add a class with this interface to its class factory if it cannot provide a
    /// moduleinfo.json file in its plug-in package/bundle where the compatibility is normally part of.
    /// 
    /// If the module contains a moduleinfo.json the host will ignore this class.
    /// 
    /// The class must write into the stream an UTF-8 encoded json description of the compatibility of
    /// the other classes in the factory.
    /// </summary>
    [ComImport, Guid(Interfaces.IPluginCompatibility), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPluginCompatibility
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetCompatibilityJSON(
            [MarshalAs(UnmanagedType.Struct), In] IBStream stream);
    }
}
