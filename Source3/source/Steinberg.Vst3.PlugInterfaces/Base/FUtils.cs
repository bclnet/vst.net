using System;
using System.Runtime.CompilerServices;

namespace Steinberg.Vst3
{
    public static class FUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T t1, ref T t2)
            => (t2, t1) = (t1, t2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximateEqual<T>(double t1, double t2, double epsilon)
        {
            if (t1 == t2) return true;
            var diff = t1 - t2;
            if (diff < 0.0) diff = -diff;
            return diff < epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToNormalized<T>(double value, int numSteps)
            => value / numSteps;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FromNormalized<T>(double norm, int numSteps)
            => Math.Min(numSteps, (int)(norm * (numSteps + 1)));
    }
}

