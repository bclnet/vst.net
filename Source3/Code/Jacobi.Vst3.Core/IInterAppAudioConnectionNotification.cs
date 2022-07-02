using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Extended plug-in interface IEditController for Inter-App Audio connection state change notifications
    /// </summary>
    [ComImport, Guid(Interfaces.IInterAppAudioConnectionNotification), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInterAppAudioConnectionNotification
    {
        /// <summary>
        /// called when the Inter-App Audio connection state changes
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        void OnInterAppAudioConnectionStateChange(
            [MarshalAs(UnmanagedType.U4), In] Boolean newState);
    }

    static partial class Interfaces
    {
        public const string IInterAppAudioConnectionNotification = "6020C72D-5FC2-4AA1-B095-0DB5D7D6D5CF";
    }
}
