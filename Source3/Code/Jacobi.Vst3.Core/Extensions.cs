using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    public static partial class Extensions
    {
        // IntPtr (unknown)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Cast<T>(this IntPtr unknownPtr) where T : class
            => unknownPtr == IntPtr.Zero
            ? null
            : Marshal.GetObjectForIUnknown(unknownPtr) as T;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CastT<T>(this T unknown) where T : class
            => unknown == null
            ? IntPtr.Zero
            : Marshal.GetIUnknownForObject(unknown);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T>(this T value, T min, T max)
            where T : IComparable<T>
            => value.CompareTo(max) > 0 ? max
            : value.CompareTo(min) < 0 ? min
            : value;

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
