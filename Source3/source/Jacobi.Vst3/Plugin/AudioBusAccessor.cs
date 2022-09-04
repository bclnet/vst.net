using System;

namespace Steinberg.Vst3
{
    public unsafe class AudioBusAccessor
    {
        readonly SymbolicSampleSizes _sampleSize;
        readonly BusDirection _busDir;
        readonly int _numSamples;
        AudioBusBuffers* _audioBuffers;

        public AudioBusAccessor(ref ProcessData processData, BusDirection busDir, int busIndex)
        {
            _busDir = busDir;
            _numSamples = processData.NumSamples;
            _sampleSize = processData.SymbolicSampleSize;

            if (busDir == BusDirection.Input)
            {
                Guard.ThrowIfOutOfRange(nameof(busIndex), busIndex, 0, processData.NumInputs);
                _audioBuffers = &processData.InputsX[busIndex];
            }
            else
            {
                Guard.ThrowIfOutOfRange(nameof(busIndex), busIndex, 0, processData.NumOutputs);
                _audioBuffers = &processData.OutputsX[busIndex];
            }
        }

        public BusDirection BusDirection => _busDir;

        public SymbolicSampleSizes SampleSize => _sampleSize;

        public int SampleCount => _numSamples;

        public int ChannelCount => _audioBuffers->NumChannels;

        public bool IsChannelSilent(int channelIndex)
        {
            Guard.ThrowIfOutOfRange(nameof(channelIndex), channelIndex, 0, _audioBuffers->NumChannels);
            return (_audioBuffers->SilenceFlags & (ulong)(1 << channelIndex)) != 0;
        }

        public void SetChannelSilent(int channelIndex, bool silent)
        {
            Guard.ThrowIfOutOfRange(nameof(channelIndex), channelIndex, 0, _audioBuffers->NumChannels);
            var mask = (ulong)(1 << channelIndex);

            // reset (not-silent)
            _audioBuffers->SilenceFlags &= ~mask;

            if (silent) _audioBuffers->SilenceFlags |= mask; // set
        }

        public int Read32(int channelIndex, float[] buffer, int length)
        {
            if (length > _numSamples) length = _numSamples;
            if (length > buffer.Length) length = buffer.Length;

            var ptr = GetBuffer32(channelIndex);
            if (ptr != null)
            {
                for (var i = 0; i < length; i++) buffer[i] = ptr[i];
                return length;
            }

            return 0;
        }

        public int Write32(int channelIndex, float[] buffer, int length)
        {
            if (_busDir == BusDirection.Input) return 0;
            if (length > _numSamples) length = _numSamples;
            if (length > buffer.Length) length = buffer.Length;

            var ptr = GetBuffer32(channelIndex);
            if (ptr != null)
            {
                for (var i = 0; i < length; i++) ptr[i] = buffer[i];
                return length;
            }

            return 0;
        }

        public int Read64(int channelIndex, double[] buffer, int length)
        {
            if (length > _numSamples) length = _numSamples;
            if (length > buffer.Length) length = buffer.Length;

            var ptr = GetBuffer64(channelIndex);
            if (ptr != null)
            {
                for (var i = 0; i < length; i++) buffer[i] = ptr[i];
                return length;
            }

            return 0;
        }

        public int Write64(int channelIndex, double[] buffer, int length)
        {
            if (_busDir == BusDirection.Input) return 0;
            if (length > _numSamples) length = _numSamples;
            if (length > buffer.Length) length = buffer.Length;

            var ptr = GetBuffer64(channelIndex);
            if (ptr != null)
            {
                for (var i = 0; i < length; i++) ptr[i] = buffer[i];
                return length;
            }

            return 0;
        }

        public float* GetBuffer32(int channelIndex)
        {
            if (_sampleSize != SymbolicSampleSizes.Sample32) throw new InvalidOperationException("32 bit sample size is not supported.");
            Guard.ThrowIfOutOfRange(nameof(channelIndex), channelIndex, 0, _audioBuffers->NumChannels);

            return _audioBuffers->ChannelBuffers32 != IntPtr.Zero && !IsChannelSilent(channelIndex)
                ? _audioBuffers->ChannelBuffers32X[channelIndex]
                : (float*)null;
        }

        public double* GetBuffer64(int channelIndex)
        {
            if (_sampleSize != SymbolicSampleSizes.Sample64) throw new InvalidOperationException("64 bit sample size is not supported.");
            Guard.ThrowIfOutOfRange(nameof(channelIndex), channelIndex, 0, _audioBuffers->NumChannels);

            return _audioBuffers->ChannelBuffers64 != IntPtr.Zero && !IsChannelSilent(channelIndex)
                ? _audioBuffers->ChannelBuffers64X[channelIndex]
                : (double*)null;
        }
    }
}
