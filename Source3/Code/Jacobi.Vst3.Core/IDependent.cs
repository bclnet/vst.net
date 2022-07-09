using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// A dependent will get notified about changes of a model.
    /// </summary>
    [ComImport, Guid(Interfaces.IDependent), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDependent
    {
        /// <summary>
        /// Inform the dependent, that the passed FUnknown has changed.
        /// </summary>
        /// <param name="changedUnknown"></param>
        /// <param name="message"></param>
        [PreserveSig]
        void Update(
            [MarshalAs(UnmanagedType.IUnknown), In] Object changedUnknown,
            [MarshalAs(UnmanagedType.I4), In] Int32 message);

        public enum ChangeMessage
        {
            WillChange,
            Changed,
            Destroyed,
            WillDestroy,
            StdChangeMessageLast = WillDestroy
        }
    }

    internal static partial class Interfaces
    {
        public const string IDependent = "F52B7AAE-DE72-416d-8AF1-8ACE9DD7BD5E";
    }
}
