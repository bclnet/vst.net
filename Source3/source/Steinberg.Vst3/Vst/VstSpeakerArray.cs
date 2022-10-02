using System;
using SpeakerType = System.UInt64;

namespace Steinberg.Vst3
{
    /// <summary>
    /// Helper class representing speaker arrangement as array of speaker types.
    /// </summary>
    public class SpeakerArray
    {
        const int kMaxSpeakers = 64;

        protected int count;
        protected SpeakerType[] speaker = new SpeakerType[kMaxSpeakers];

        public SpeakerArray(SpeakerArrangement arr = 0)
            => SetArrangement(arr);

        public int Total
            => count;

        public Speaker this[int index]
            => (Speaker)speaker[index];

        public void SetArrangement(SpeakerArrangement arr)
        {
            count = 0;
            Array.Clear(speaker);

            for (var i = 0; i < kMaxSpeakers; i++)
            {
                SpeakerType mask = 1UL << i;
                if (((ulong)arr & mask) != 0)
                    speaker[count++] = mask;
            }
        }

        public SpeakerArrangement GetArrangement()
        {
            var arr = 0UL;
            for (var i = 0; i < count; i++)
                arr |= speaker[i];
            return (SpeakerArrangement)arr;
        }

        public int GetSpeakerIndex(Speaker which)
        {
            for (var i = 0; i < count; i++)
                if (speaker[i] == (SpeakerType)which)
                    return i;
            return -1;
        }
    }
}
