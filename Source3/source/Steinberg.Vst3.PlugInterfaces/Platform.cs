﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Steinberg.Vst3
{
    public unsafe static class Platform
    {
        public const int Fixed128 = 128;
#if X86
        public const int StructurePack = 8;
#endif
#if X64
        public const int StructurePack = 16;
#endif
        public const CharSet CharacterSet = CharSet.Unicode;

        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)] public static extern IntPtr memset(IntPtr dest, int c, nint count);
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)] public static extern IntPtr memcpy(IntPtr dest, IntPtr src, nint count);

        //public static uint NumberOfSetBits(uint value)
        //{
        //    // don't ask...
        //    value -= value >> 1 & 0x55555555;
        //    value = (value & 0x33333333) + (value >> 2 & 0x33333333);
        //    return (value + (value >> 4) & 0x0F0F0F0F) * 0x01010101 >> 24;
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReleaseRcw<T>(ref T t)
        {
            t = default;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap64(long* v)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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