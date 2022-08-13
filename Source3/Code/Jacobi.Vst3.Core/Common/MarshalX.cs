using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Common
{
    //:ref https://lanzkron.wordpress.com/2011/01/06/wrappers-unwrapped/
    public static class MarshalX
    {
        public static T AddRcwRef<T>(T t)
        {
            var ptr = Marshal.GetIUnknownForObject(t);
            try
            {
                return (T)Marshal.GetObjectForIUnknown(ptr);
            }
            finally
            {
                Marshal.Release(ptr); // done with the IntPtr
            }
        }

        public static void ReleaseRcw<T>(ref T t)
        {
            t = default;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
