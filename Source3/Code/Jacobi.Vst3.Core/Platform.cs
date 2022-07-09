using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    public static class Platform
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
    }
}
