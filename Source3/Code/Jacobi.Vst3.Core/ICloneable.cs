using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.ICloneable), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICloneable
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.IUnknown)]
        Object Clone();
    }

    static partial class Interfaces
    {
        public const string ICloneable = "D45406B9-3A2D-4443-9DAD-9BA985A1454B";
    }
}
