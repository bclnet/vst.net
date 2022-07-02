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
            [In] ref Guid classId,
            [In] ref Guid interfaceId,
            [MarshalAs(UnmanagedType.SysInt, IidParameterIndex = 1), In, Out] ref IntPtr instance);
    }

    internal static partial class Interfaces
    {
        public const string IHostApplication = "58E595CC-DB2D-4969-8B6A-AF8C36A664E5";
    }
}
