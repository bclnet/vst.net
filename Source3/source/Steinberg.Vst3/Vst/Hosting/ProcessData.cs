using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.Hosting
{
    public unsafe class HostProcessData
    {
        public ProcessData _;
        protected bool channelBufferOwner;

        public HostProcessData() { }

        ~HostProcessData() => Unprepare();

        // Prepare buffer containers for all busses. If bufferSamples is not null buffers will be created.
        public bool Prepare(IComponent component, int bufferSamples, SymbolicSampleSizes symbolicSampleSize)
        {
            if (CheckIfReallocationNeeded(component, bufferSamples, symbolicSampleSize))
            {
                Unprepare();

                _.SymbolicSampleSize = symbolicSampleSize;
                channelBufferOwner = bufferSamples > 0;

                _.NumInputs = CreateBuffers(component, ref _.InputsX, BusDirection.Input, bufferSamples);
                _.NumOutputs = CreateBuffers(component, ref _.OutputsX, BusDirection.Output, bufferSamples);
            }
            else
            {
                // reset silence flags
                for (var i = 0; i < _.NumInputs; i++)
                    _.Inputs[i].SilenceFlags = 0;
                for (var i = 0; i < _.NumOutputs; i++)
                    _.Outputs[i].SilenceFlags = 0;
            }
            _.SymbolicSampleSize = symbolicSampleSize;

            return true;
        }

        // Remove bus buffers.
        public void Unprepare()
        {
            DestroyBuffers(ref _.InputsX, ref _.NumInputs);
            DestroyBuffers(ref _.OutputsX, ref _.NumOutputs);

            channelBufferOwner = false;
        }

        // Sets one sample buffer for all channels inside a bus.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffers(BusDirection dir, int busIndex, Single* sampleBuffer)
        {
            if (channelBufferOwner || _.SymbolicSampleSize != SymbolicSampleSizes.Sample32) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirection.Input ? _.Inputs[busIndex] : _.Outputs[busIndex];
            for (var i = 0; i < busBuffers.NumChannels; i++) busBuffers.ChannelBuffers32[i] = sampleBuffer;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffers64(BusDirection dir, int busIndex, Double* sampleBuffer)
        {
            if (channelBufferOwner || _.SymbolicSampleSize != SymbolicSampleSizes.Sample64) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirection.Input ? _.Inputs[busIndex] : _.Outputs[busIndex];
            for (var i = 0; i < busBuffers.NumChannels; i++) busBuffers.ChannelBuffers64[i] = sampleBuffer;
            return true;
        }

        // Sets individual sample buffers per channel inside a bus.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffers(BusDirection dir, int busIndex, Single*[] sampleBuffers, int bufferCount)
        {
            if (channelBufferOwner || _.SymbolicSampleSize != SymbolicSampleSizes.Sample32) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirection.Input ? _.Inputs[busIndex] : _.Outputs[busIndex];
            var count = bufferCount < busBuffers.NumChannels ? bufferCount : busBuffers.NumChannels;
            for (var i = 0; i < count; i++) busBuffers.ChannelBuffers32[i] = sampleBuffers != null ? sampleBuffers[i] : null;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffers64(BusDirection dir, int busIndex, Double*[] sampleBuffers, int bufferCount)
        {
            if (channelBufferOwner || _.SymbolicSampleSize != SymbolicSampleSizes.Sample64) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirection.Input ? _.Inputs[busIndex] : _.Outputs[busIndex];
            var count = bufferCount < busBuffers.NumChannels ? bufferCount : busBuffers.NumChannels;
            for (var i = 0; i < count; i++) ((Double**)busBuffers.ChannelBuffers64X)[i] = sampleBuffers != null ? sampleBuffers[i] : null;
            return true;
        }

        // Sets one sample buffer for a given channel inside a bus.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffer(BusDirection dir, int busIndex, int channelIndex, Single* sampleBuffer)
        {
            if (_.SymbolicSampleSize != SymbolicSampleSizes.Sample32) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirection.Input ? _.Inputs[busIndex] : _.Outputs[busIndex];
            if (channelIndex >= busBuffers.NumChannels) return false;
            busBuffers.ChannelBuffers32[channelIndex] = sampleBuffer;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffer64(BusDirection dir, int busIndex, int channelIndex, Double* sampleBuffer)
        {
            if (_.SymbolicSampleSize != SymbolicSampleSizes.Sample64) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirection.Input ? _.Inputs[busIndex] : _.Outputs[busIndex];
            if (channelIndex >= busBuffers.NumChannels) return false;
            busBuffers.ChannelBuffers64[channelIndex] = sampleBuffer;
            return true;
        }

        public const ulong kAllChannelsSilent =
#if SMTG_OS_MACOS
	    0xffffffffffffffffULL;
#else
        0xffffffffffffffffUL;
#endif

        protected bool CheckIfReallocationNeeded(IComponent component, int bufferSamples, SymbolicSampleSizes symbolicSampleSize)
        {
            if (channelBufferOwner != bufferSamples > 0)
                return true;
            if (_.SymbolicSampleSize != symbolicSampleSize)
                return true;

            var inBusCount = component.GetBusCount(MediaType.Audio, BusDirection.Input);
            if (inBusCount != _.NumInputs)
                return true;

            var outBusCount = component.GetBusCount(MediaType.Audio, BusDirection.Output);
            if (outBusCount != _.NumOutputs)
                return true;

            for (var i = 0; i < inBusCount; i++)
            {
                if (component.GetBusInfo(MediaType.Audio, BusDirection.Input, i, out var busInfo) == kResultTrue)
                    if (_.Inputs[i].NumChannels != busInfo.ChannelCount)
                        return true;
            }
            for (var i = 0; i < outBusCount; i++)
            {
                if (component.GetBusInfo(MediaType.Audio, BusDirection.Output, i, out var busInfo) == kResultTrue)
                    if (_.Outputs[i].NumChannels != busInfo.ChannelCount)
                        return true;
            }
            return false;
        }

        protected int CreateBuffers(IComponent component, ref IntPtr buffers2, BusDirection dir, int bufferSamples)
        {
            var buffers = (AudioBusBuffers*)buffers2;
            var busCount = component.GetBusCount(MediaType.Audio, dir);
            if (busCount > 0)
            {
                buffers = (AudioBusBuffers*)Marshal.AllocHGlobal(AudioBusBuffers.Size * busCount);
                buffers2 = (IntPtr)buffers;

                for (var i = 0; i < busCount; i++)
                    if (component.GetBusInfo(MediaType.Audio, dir, i, out var busInfo) == kResultTrue)
                    {
                        buffers[i].NumChannels = busInfo.ChannelCount;

                        // allocate for each channel
                        if (busInfo.ChannelCount > 0)
                        {
                            if (_.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                                buffers[i].ChannelBuffers64X = Marshal.AllocHGlobal(sizeof(Double*) * busInfo.ChannelCount);
                            else
                                buffers[i].ChannelBuffers32X = Marshal.AllocHGlobal(sizeof(Single*) * busInfo.ChannelCount);

                            for (var j = 0; j < busInfo.ChannelCount; j++)
                                if (_.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                                    buffers[i].ChannelBuffers64[j] = bufferSamples > 0
                                        ? (Double*)Marshal.AllocHGlobal(sizeof(Double) * bufferSamples)
                                        : null;
                                else
                                    buffers[i].ChannelBuffers32[j] = bufferSamples > 0
                                        ? (Single*)Marshal.AllocHGlobal(sizeof(Single) * bufferSamples)
                                        : null;
                        }
                    }
            }
            return busCount;
        }

        protected void DestroyBuffers(ref IntPtr buffers2, ref int busCount)
        {
            var buffers = (AudioBusBuffers*)buffers2;
            if (buffers != null)
            {
                for (var i = 0; i < busCount; i++)
                {
                    if (channelBufferOwner)
                        for (var j = 0; j < buffers[i].NumChannels; j++)
                            if (_.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                            {
                                if (buffers[i].ChannelBuffers64 != null && buffers[i].ChannelBuffers64[j] != null)
                                    Marshal.FreeHGlobal((IntPtr)buffers[i].ChannelBuffers64[j]);
                            }
                            else
                            {
                                if (buffers[i].ChannelBuffers32 != null && buffers[i].ChannelBuffers32[j] != null)
                                    Marshal.FreeHGlobal((IntPtr)buffers[i].ChannelBuffers32[j]);
                            }

                    if (_.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                    {
                        if (buffers[i].ChannelBuffers64X != IntPtr.Zero)
                            Marshal.FreeHGlobal(buffers[i].ChannelBuffers64X);
                    }
                    else
                    {
                        if (buffers[i].ChannelBuffers32X != IntPtr.Zero)
                            Marshal.FreeHGlobal(buffers[i].ChannelBuffers32X);
                    }
                }

                Marshal.FreeHGlobal(buffers2);
                buffers2 = IntPtr.Zero;
            }
            busCount = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool IsValidBus(BusDirection dir, int busIndex)
        {
            if (dir == BusDirection.Input && (_.InputsX == IntPtr.Zero || busIndex >= _.NumInputs)) return false;
            if (dir == BusDirection.Output && (_.OutputsX == IntPtr.Zero || busIndex >= _.NumOutputs)) return false;
            return true;
        }
    }
}
