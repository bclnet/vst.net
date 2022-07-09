using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Interface to return an ascii string of variable size. 
    /// In order to manage memory allocation and deallocation properly, 
    /// this interface is used to transfer a string as result parameter of
	/// a method requires a string of unknown size. 
    /// </summary>
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
