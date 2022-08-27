using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    public unsafe static class Platform
    {
#if X86
        public const int StructurePack = 8;
#endif
#if X64
        public const int StructurePack = 16;
#endif
        public const CharSet CharacterSet = CharSet.Unicode;
        public const CallingConvention DefaultCallingConvention = CallingConvention.StdCall;

        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)] public static extern IntPtr Memset(IntPtr dest, int c, nint count);
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)] public static extern IntPtr Memcpy(IntPtr dest, IntPtr src, nint count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref int a, ref int b) => (b, a) = (a, b);

        public static uint NumberOfSetBits(uint value)
        {
            // don't ask...
            value -= value >> 1 & 0x55555555;
            value = (value & 0x33333333) + (value >> 2 & 0x33333333);
            return (value + (value >> 4) & 0x0F0F0F0F) * 0x01010101 >> 24;
        }

        public static void ReleaseRcw<T>(ref T t)
        {
            t = default;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void Swap64(long* v)
        {
        }

        public static void Swap32(int* v)
        {
        }

        //    //:ref https://lanzkron.wordpress.com/2011/01/06/wrappers-unwrapped/
        //    public static class MarshalX
        //    {
        //        public static T AddRcwRef<T>(T t)
        //        {
        //            var ptr = Marshal.GetIUnknownForObject(t);
        //            try
        //            {
        //                return (T)Marshal.GetObjectForIUnknown(ptr);
        //            }
        //            finally
        //            {
        //                Marshal.Release(ptr); // done with the IntPtr
        //            }
        //        }
        //    }
    }
}
