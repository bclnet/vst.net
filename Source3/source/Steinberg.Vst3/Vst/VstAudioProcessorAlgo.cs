using System;
using System.Runtime.CompilerServices;
using static Steinberg.Vst3.TResult;
using ParamID = System.UInt32;
using ParamValue = System.Double;
using Sample32 = System.Single;
using Sample64 = System.Double;

namespace Steinberg.Vst3
{
    unsafe static partial class Helpers
    {
        /// <summary>
        /// Returns the current channelBuffers used (depending of symbolicSampleSize).
        /// </summary>
        /// <param name="processSetup"></param>
        /// <param name="bufs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void** GetChannelBuffersPointerX(this ProcessSetup processSetup, AudioBusBuffers bufs)
            => processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32
                ? (void**)bufs.ChannelBuffers32
                : (void**)bufs.ChannelBuffers64;

        /// <summary>
        /// Returns the size in bytes of numSamples for one channel depending of symbolicSampleSize.
        /// </summary>
        /// <param name="processSetup"></param>
        /// <param name="numSamples"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSampleFramesSizeInBytes(this ProcessSetup processSetup, int numSamples)
            => processSetup.SymbolicSampleSize == SymbolicSampleSizes.Sample32
                ? numSamples * sizeof(Sample32)
                : numSamples * sizeof(Sample64);
    }

    public unsafe static class Algo
    {
        public delegate void BufferAction(ref AudioBusBuffers buffer);
        public delegate void BufferAction32(Sample32* buffer);
        public delegate void BufferAction64(Sample64* obj);
        public delegate void TwoBufferAction32(Sample32* buffer1, Sample32* buffer2, int channelIndex);
        public delegate void TwoBufferAction64(Sample64* buffer1, Sample64* buffer2, int channelIndex);
        public delegate void EventAction(ref Event evnt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Foreach(ref AudioBusBuffers[] audioBusBuffers, int busCount, BufferAction func)
        {
            if (audioBusBuffers == null)
                return;

            for (var busIndex = 0; busIndex < busCount; ++busIndex)
                func(ref audioBusBuffers[busIndex]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Foreach32(ref AudioBusBuffers audioBuffer, BufferAction32 func)
        {
            for (var channelIndex = 0; channelIndex < audioBuffer.NumChannels; ++channelIndex)
            {
                if (audioBuffer.ChannelBuffers32[channelIndex] == null)
                    return;

                func(audioBuffer.ChannelBuffers32[channelIndex]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Foreach64(ref AudioBusBuffers audioBuffer, BufferAction64 func)
        {
            for (var channelIndex = 0; channelIndex < audioBuffer.NumChannels; ++channelIndex)
            {
                if (audioBuffer.ChannelBuffers64[channelIndex] == null)
                    return;

                func(audioBuffer.ChannelBuffers64[channelIndex]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Foreach32(ref AudioBusBuffers buffer1, ref AudioBusBuffers buffer2, TwoBufferAction32 func)
        {
            var numChannels = Math.Min(buffer1.NumChannels, buffer2.NumChannels);

            for (var channelIndex = 0; channelIndex < numChannels; ++channelIndex)
                func(buffer1.ChannelBuffers32[channelIndex], buffer2.ChannelBuffers32[channelIndex], channelIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Foreach64(ref AudioBusBuffers buffer1, ref AudioBusBuffers buffer2, TwoBufferAction64 func)
        {
            var numChannels = Math.Min(buffer1.NumChannels, buffer2.NumChannels);

            for (var channelIndex = 0; channelIndex < numChannels; ++channelIndex)
                func(buffer1.ChannelBuffers64[channelIndex], buffer2.ChannelBuffers64[channelIndex], channelIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy32(ref AudioBusBuffers src, ref AudioBusBuffers dest, int sliceSize, int startIndex)
        {
            //if (src == default || dest == default)
            //    return;

            var numChannels = Math.Min(src.NumChannels, dest.NumChannels);
            var numBytes = sliceSize * sizeof(Sample32);
            for (var chIdx = 0; chIdx < numChannels; ++chIdx)
                Platform.memcpy((IntPtr)dest.ChannelBuffers32[chIdx][startIndex], (IntPtr)src.ChannelBuffers32[chIdx], numBytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy64(ref AudioBusBuffers src, ref AudioBusBuffers dest, int sliceSize, int startIndex)
        {
            //if (src == default || dest == default)
            //    return;

            var numChannels = Math.Min(src.NumChannels, dest.NumChannels);
            var numBytes = sliceSize * sizeof(Sample64);
            for (var chIdx = 0; chIdx < numChannels; ++chIdx)
                Platform.memcpy((IntPtr)dest.ChannelBuffers64[chIdx][startIndex], (IntPtr)src.ChannelBuffers64[chIdx], numBytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear32(ref AudioBusBuffers[] audioBusBuffers, int sampleCount, int busCount = 1)
        {
            //if (audioBusBuffers == default)
            //    return;

            var numBytes = sampleCount * sizeof(Sample32);
            Foreach(ref audioBusBuffers, busCount, (ref AudioBusBuffers audioBuffer) =>
                Foreach32(ref audioBuffer, (Sample32* channelBuffer) =>
                    Platform.memset((IntPtr)channelBuffer, 0, numBytes)
                )
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear64(ref AudioBusBuffers[] audioBusBuffers, int sampleCount, int busCount = 1)
        {
            //if (audioBusBuffers == default)
            //    return;

            var numBytes = sampleCount * sizeof(Sample64);
            Foreach(ref audioBusBuffers, busCount, (ref AudioBusBuffers audioBuffer) =>
                Foreach64(ref audioBuffer, (Sample64* channelBuffer) =>
                    Platform.memset((IntPtr)channelBuffer, 0, numBytes)
                )
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Mix32(ref AudioBusBuffers src, ref AudioBusBuffers dest, int sampleCount)
        {
            Foreach32(ref src, ref dest, (Sample32* srcBuffer, Sample32* destBuffer, int channelIndex) =>
            {
                for (var sampleIndex = 0; sampleIndex < sampleCount; ++sampleIndex)
                    destBuffer[sampleIndex] += srcBuffer[sampleIndex];
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Mix64(ref AudioBusBuffers src, ref AudioBusBuffers dest, int sampleCount)
        {
            Foreach64(ref src, ref dest, (Sample64* srcBuffer, Sample64* destBuffer, int channelIndex) =>
            {
                for (var sampleIndex = 0; sampleIndex < sampleCount; ++sampleIndex)
                    destBuffer[sampleIndex] += srcBuffer[sampleIndex];
            });
        }

        /// <summary>
        /// Multiply buffer with a constant
        /// </summary>
        /// <param name="srcBuffer"></param>
        /// <param name="destBuffer"></param>
        /// <param name="sampleCount"></param>
        /// <param name="factor"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(Sample32* srcBuffer, Sample32* destBuffer, int sampleCount, Sample32 factor)
        {
            for (var sampleIndex = 0; sampleIndex < sampleCount; ++sampleIndex)
                destBuffer[sampleIndex] = srcBuffer[sampleIndex] * factor;
        }

        /// <summary>
        /// Multiply buffer with a constant
        /// </summary>
        /// <param name="srcBuffer"></param>
        /// <param name="destBuffer"></param>
        /// <param name="sampleCount"></param>
        /// <param name="factor"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(Sample64* srcBuffer, Sample64* destBuffer, int sampleCount, Sample64 factor)
        {
            for (var sampleIndex = 0; sampleIndex < sampleCount; ++sampleIndex)
                destBuffer[sampleIndex] = srcBuffer[sampleIndex] * factor;
        }

        /// <summary>
        /// Multiply all channels of AudioBusBuffer with a constant
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <param name="sampleCount"></param>
        /// <param name="factor"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply32(ref AudioBusBuffers src, ref AudioBusBuffers dest, int sampleCount, float factor)
        {
            Foreach32(ref src, ref dest, (Sample32* srcBuffer, Sample32* destBuffer, int channelIndex) =>
                Multiply(srcBuffer, destBuffer, sampleCount, factor)
            );
        }

        /// <summary>
        /// Multiply all channels of AudioBusBuffer with a constant
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <param name="sampleCount"></param>
        /// <param name="factor"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply64(ref AudioBusBuffers src, ref AudioBusBuffers dest, int sampleCount, float factor)
        {
            Foreach64(ref src, ref dest, (Sample64* srcBuffer, Sample64* destBuffer, int channelIndex) =>
                Multiply(srcBuffer, destBuffer, sampleCount, factor)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSilent32(ref AudioBusBuffers audioBuffer, int sampleCount, int startIndex = 0)
        {
            const double epsilon = 1e-10f; // under -200dB...

            sampleCount += startIndex;
            for (var channelIndex = 0; channelIndex < audioBuffer.NumChannels; ++channelIndex)
            {
                if (audioBuffer.ChannelBuffers32[channelIndex] == null)
                    return true;

                for (var sampleIndex = startIndex; sampleIndex < sampleCount; ++sampleIndex)
                {
                    var val = audioBuffer.ChannelBuffers32[channelIndex][sampleIndex];
                    if (Math.Abs(val) > epsilon)
                        return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSilent64(ref AudioBusBuffers audioBuffer, int sampleCount, int startIndex = 0)
        {
            const double epsilon = 1e-10f; // under -200dB...

            sampleCount += startIndex;
            for (var channelIndex = 0; channelIndex < audioBuffer.NumChannels; ++channelIndex)
            {
                if (audioBuffer.ChannelBuffers64[channelIndex] == null)
                    return true;

                for (var sampleIndex = startIndex; sampleIndex < sampleCount; ++sampleIndex)
                {
                    var val = audioBuffer.ChannelBuffers64[channelIndex][sampleIndex];
                    if (Math.Abs(val) > epsilon)
                        return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Foreach(IEventList eventList, EventAction func)
        {
            if (eventList == null)
                return;

            var eventCount = eventList.GetEventCount();
            for (var eventIndex = 0; eventIndex < eventCount; ++eventIndex)
            {
                if (eventList.GetEvent(eventIndex, out var evnt) != kResultOk)
                    continue;

                func(ref evnt);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Foreach(IParamValueQueue paramQueue, Action<ParamID, int, ParamValue> func)
        {
            var paramId = paramQueue.GetParameterId();
            var numPoints = paramQueue.GetPointCount();
            for (var pointIndex = 0; pointIndex < numPoints; ++pointIndex)
            {
                if (paramQueue.GetPoint(pointIndex, out var sampleOffset, out var value) != kResultOk)
                    continue;

                func(paramId, sampleOffset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForeachLast(IParamValueQueue paramQueue, Action<ParamID, int, ParamValue> func)
        {
            var paramId = paramQueue.GetParameterId();
            var numPoints = paramQueue.GetPointCount();
            if (paramQueue.GetPoint(numPoints - 1, out var sampleOffset, out var value) == kResultOk)
                func(paramId, sampleOffset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Foreach(IParameterChanges changes, Action<IParamValueQueue> func)
        {
            if (changes == null)
                return;

            var paramCount = changes.GetParameterCount();
            for (var paramIndex = 0; paramIndex < paramCount; ++paramIndex)
            {
                var paramValueQueue = changes.GetParameterData(paramIndex);
                if (paramValueQueue == null)
                    continue;

                func(paramValueQueue);
            }
        }
    }
}
