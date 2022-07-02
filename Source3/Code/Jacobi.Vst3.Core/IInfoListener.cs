using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IInfoListener), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInfoListener
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetChannelContextInfos(
            [MarshalAs(UnmanagedType.Interface), In] IAttributeList list);
    }

    internal static partial class Interfaces
    {
        public const string IInfoListener = "0F194781-8D98-4ADA-BBA0-C1EFC011D8D0";
    }
}
