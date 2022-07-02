using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// A representation based on XML is a way to export, structure, and group plug-ins parameters for a specific remote (hardware or software rack (such as quick controls)).
    /// </summary>
    [ComImport, Guid(Interfaces.IXmlRepresentationController), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IXmlRepresentationController
    {
        /// <summary>
        /// Retrieves a stream containing a XmlRepresentation for a wanted representation info
        /// </summary>
        /// <param name="info"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetXmlRepresentationStream(
            [MarshalAs(UnmanagedType.Struct), In] ref RepresentationInfo info,
            [MarshalAs(UnmanagedType.Interface), In, Out] IBStream stream);
    }

    static partial class Interfaces
    {
        public const string IXmlRepresentationController = "A81A0471-48C3-4DC4-AC30-C9E13C8393D5";
    }
}
