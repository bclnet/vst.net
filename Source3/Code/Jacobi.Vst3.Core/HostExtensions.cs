using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    public static class HostExtensions
    {
        /// <summary>
        /// Helper to allocate a message
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMessage AllocateMessage(this IHostApplication host)
        {
            var msgType = typeof(IMessage);
            var iid = msgType.GUID;
            var ptr = IntPtr.Zero;
            var result = host.CreateInstance(ref iid, ref iid, out ptr);
            return result.Succeeded()
                ? (IMessage)Marshal.GetTypedObjectForIUnknown(ptr, msgType)
                : default;
            //try { return (IMessage)Marshal.GetTypedObjectForIUnknown(ptr, msgType); }
            //finally { Marshal.Release(ptr); }
        }
    }
}
