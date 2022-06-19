using Jacobi.Vst3.Common;
using Jacobi.Vst3.Core;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Plugin
{
    public static class VstExtensions
    {
        public static IMessage CreateMessage(this IHostApplication host)
        {
            Guard.ThrowIfNull("host", host);

            var msgType = typeof(IMessage);
            var iid = msgType.GUID;
            var ptr = IntPtr.Zero;

            var result = host.CreateInstance(ref iid, ref iid, ref ptr);

            if (TResult.Succeeded(result)) return (IMessage)Marshal.GetTypedObjectForIUnknown(ptr, msgType);

            return null;
        }

        public static uint NumberOfSetBits(uint value)
        {
            // don't ask...
            value -= ((value >> 1) & 0x55555555);
            value = (value & 0x33333333) + ((value >> 2) & 0x33333333);
            return (((value + (value >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        /// <summary>
        /// Returns number of channels used in speaker arrangement. ingroup speakerArrangements
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetChannelCount(this SpeakerArrangement source)
        {
            var arr = (ulong)source;
            var count = 0;
            while (arr != 0)
            {
                if ((arr & 1) != 0) ++count;
                arr >>= 1;
            }
            return count;
        }

        /// <summary>
        /// Returns the index of a given speaker in a speaker arrangement (-1 if speaker is not part of the arrangement)
        /// </summary>
        /// <param name="speaker"></param>
        /// <param name="arrangement"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSpeakerIndex(this Speakers source, SpeakerArrangement arrangement)
        {
            var speaker = (ulong)source;
            // check if speaker is present in arrangement
            if (((ulong)arrangement & speaker) == 0) return -1;

            var result = 0;
            var i = 1UL;
            while (i < speaker)
            {
                if (((ulong)arrangement & i) != 0) result++;
                i <<= 1;
            }

            return result;
        }
    }
}
