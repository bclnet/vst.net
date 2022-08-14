using Jacobi.Vst3.Common;
using Jacobi.Vst3.Core;
using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Plugin
{
    public unsafe class AudioBusAccessor
    {
        private readonly SymbolicSampleSizes _sampleSize;
        private readonly BusDirections _busDir;
        private readonly int _numSamples;
        private AudioBusBuffers _audioBuffers;

        public AudioBusAccessor(ref ProcessData processData, BusDirections busDir, int busIndex)
        {
            _busDir = busDir;
            _numSamples = processData.NumSamples;
            _sampleSize = processData.SymbolicSampleSize;

            if (busDir == BusDirections.Input)
            {
                Guard.ThrowIfOutOfRange(nameof(busIndex), busIndex, 0, processData.NumInputs);
                _audioBuffers = processData.InputsX[busIndex];
            }
            else
            {
                Guard.ThrowIfOutOfRange(nameof(busIndex), busIndex, 0, processData.NumOutputs);
                _audioBuffers = processData.OutputsX[busIndex];
            }
        }

        public BusDirections BusDirection => _busDir;

        public SymbolicSampleSizes SampleSize => _sampleSize;

        public int SampleCount => _numSamples;

        public int ChannelCount => _audioBuffers.NumChannels;

        public bool IsChannelSilent(int channelIndex)
        {
            Guard.ThrowIfOutOfRange(nameof(channelIndex), channelIndex, 0, _audioBuffers.NumChannels);
            return (_audioBuffers.SilenceFlags & (ulong)(1 << channelIndex)) != 0;
        }

        public void SetChannelSilent(int channelIndex, bool silent)
        {
            Guard.ThrowIfOutOfRange(nameof(channelIndex), channelIndex, 0, _audioBuffers.NumChannels);
            var mask = (ulong)(1 << channelIndex);

            // reset (not-silent)
            _audioBuffers.SilenceFlags &= ~mask;

            if (silent) _audioBuffers.SilenceFlags |= mask; // set
        }

        public int Read32(int channelIndex, float[] buffer, int length)
        {
            if (length > _numSamples) length = _numSamples;
            if (length > buffer.Length) length = buffer.Length;

            unsafe
            {
                var ptr = GetUnsafeBuffer32(channelIndex);
                if (ptr != null)
                {
                    for (var i = 0; i < length; i++) buffer[i] = ptr[i];
                    return length;
                }
            }

            return 0;
        }

        public int Write32(int channelIndex, float[] buffer, int length)
        {
            if (_busDir == BusDirections.Input) return 0;
            if (length > _numSamples) length = _numSamples;
            if (length > buffer.Length) length = buffer.Length;

            unsafe
            {
                var ptr = GetUnsafeBuffer32(channelIndex);
                if (ptr != null)
                {
                    for (var i = 0; i < length; i++) ptr[i] = buffer[i];
                    return length;
                }
            }

            return 0;
        }

        public int Read64(int channelIndex, double[] buffer, int length)
        {
            if (length > _numSamples) length = _numSamples;
            if (length > buffer.Length) length = buffer.Length;

            unsafe
            {
                var ptr = GetUnsafeBuffer64(channelIndex);
                if (ptr != null)
                {
                    for (var i = 0; i < length; i++) buffer[i] = ptr[i];
                    return length;
                }
            }

            return 0;
        }

        public int Write64(int channelIndex, double[] buffer, int length)
        {
            if (_busDir == BusDirections.Input) return 0;
            if (length > _numSamples) length = _numSamples;
            if (length > buffer.Length) length = buffer.Length;

            unsafe
            {
                var ptr = GetUnsafeBuffer64(channelIndex);
                if (ptr != null)
                {
                    for (var i = 0; i < length; i++) ptr[i] = buffer[i];
                    return length;
                }
            }

            return 0;
        }

        public unsafe float* GetUnsafeBuffer32(int channelIndex)
        {
            if (_sampleSize != SymbolicSampleSizes.Sample32) throw new InvalidOperationException("32 bit sample size is not supported.");
            Guard.ThrowIfOutOfRange(nameof(channelIndex), channelIndex, 0, _audioBuffers.NumChannels);

            if (_audioBuffers.ChannelBuffers32 != IntPtr.Zero && !IsChannelSilent(channelIndex))
            {
                var ptr = (float**)_audioBuffers.ChannelBuffers32.ToPointer();
                return ptr[channelIndex];
            }

            return null;
        }

        public unsafe double* GetUnsafeBuffer64(int channelIndex)
        {
            if (_sampleSize != SymbolicSampleSizes.Sample64) throw new InvalidOperationException("64 bit sample size is not supported.");
            Guard.ThrowIfOutOfRange(nameof(channelIndex), channelIndex, 0, _audioBuffers.NumChannels);

            if (_audioBuffers.ChannelBuffers64 != IntPtr.Zero && !IsChannelSilent(channelIndex))
            {
                var ptr = (double**)_audioBuffers.ChannelBuffers64.ToPointer();
                return ptr[channelIndex];
            }

            return null;
        }
    }
}
