using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3
{
    /// <summary>
    /// Interface allowing an object to be copied.
    /// </summary>
    [ComImport, Guid(Interfaces.ICloneable), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICloneable
    {
        /// <summary>
        /// Create exact copy of the object
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.IUnknown)]
        Object Clone();
    }

    partial class Interfaces
    {
        public const string ICloneable = "D45406B9-3A2D-4443-9DAD-9BA985A1454B";
    }
}
