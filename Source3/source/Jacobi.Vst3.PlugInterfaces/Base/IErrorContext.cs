using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3
{
    /// <summary>
    /// Interface for error handling.
    /// </summary>
    [ComImport, Guid(Interfaces.IErrorContext), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IErrorContext
    {
        /// <summary>
        /// Tells the plug-in to not show any UI elements on errors.
        /// </summary>
        /// <param name="state"></param>
        [PreserveSig]
        void DisableErrorUI(
            [MarshalAs(UnmanagedType.U1), In] Boolean state);

        /// <summary>
        /// If an error happens and disableErrorUI was not set this should return kResultTrue if the plug-in already showed a message to the user what happened.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult ErrorMessageShown();

        /// <summary>
        /// Fill message with error string. The host may show this to the user.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetErrorMessage(
            [MarshalAs(UnmanagedType.Interface), In] IString message);
    }

    partial class Interfaces
    {
        public const string IErrorContext = "12BCD07B-7C69-4336-B7DA-77C3444A0CD0";
    }
}
