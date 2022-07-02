using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IStringResult), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStringResult
    {
        [PreserveSig]
        void SetText(
            [MarshalAs(UnmanagedType.LPStr), In] String text);
    }

    static partial class Interfaces
    {
        public const string IStringResult = "550798BC-8720-49DB-8492-0A153B50B7A8";
    }
}
