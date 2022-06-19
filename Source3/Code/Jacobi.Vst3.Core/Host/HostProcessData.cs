using Jacobi.Vst3.Core;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Host
{
    public unsafe class HostProcessData
    {
        public ProcessData _;
        protected bool channelBufferOwner;

        ~HostProcessData() => Unprepare();

        // Prepare buffer containers for all busses. If bufferSamples is not null buffers will be created.
        public bool Prepare(IComponent component, int bufferSamples, SymbolicSampleSizes symbolicSampleSize)
        {
            if (CheckIfReallocationNeeded(component, bufferSamples, symbolicSampleSize))
            {
                Unprepare();

                _.SymbolicSampleSize = symbolicSampleSize;
                channelBufferOwner = bufferSamples > 0;

                _.NumInputs = CreateBuffers(component, ref _.Inputs, BusDirections.Input, bufferSamples);
                _.NumOutputs = CreateBuffers(component, ref _.Outputs, BusDirections.Output, bufferSamples);
            }
            else
            {
                // reset silence flags
                for (var i = 0; i < _.NumInputs; i++) ((AudioBusBuffers*)_.Inputs)[i].SilenceFlags = 0;
                for (var i = 0; i < _.NumOutputs; i++) ((AudioBusBuffers*)_.Outputs)[i].SilenceFlags = 0;
            }
            _.SymbolicSampleSize = symbolicSampleSize;

            return true;
        }

        // Remove bus buffers.
        public void Unprepare()
        {
            DestroyBuffers(ref _.Inputs, ref _.NumInputs);
            DestroyBuffers(ref _.Outputs, ref _.NumOutputs);

            channelBufferOwner = false;
        }

        // Sets one sample buffer for all channels inside a bus.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffers(BusDirections dir, int busIndex, Single* sampleBuffer)
        {
            if (channelBufferOwner || _.SymbolicSampleSize != SymbolicSampleSizes.Sample32) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirections.Input ? ((AudioBusBuffers*)_.Inputs)[busIndex] : ((AudioBusBuffers*)_.Outputs)[busIndex];
            for (var i = 0; i < busBuffers.NumChannels; i++) ((Single**)busBuffers.ChannelBuffers32)[i] = sampleBuffer;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffers64(BusDirections dir, int busIndex, Double* sampleBuffer)
        {
            if (channelBufferOwner || _.SymbolicSampleSize != SymbolicSampleSizes.Sample64) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirections.Input ? ((AudioBusBuffers*)_.Inputs)[busIndex] : ((AudioBusBuffers*)_.Outputs)[busIndex];
            for (var i = 0; i < busBuffers.NumChannels; i++) ((Double**)busBuffers.ChannelBuffers64)[i] = sampleBuffer;
            return true;
        }

        // Sets individual sample buffers per channel inside a bus.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffers(BusDirections dir, int busIndex, Single*[] sampleBuffers, int bufferCount)
        {
            if (channelBufferOwner || _.SymbolicSampleSize != SymbolicSampleSizes.Sample32) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirections.Input ? ((AudioBusBuffers*)_.Inputs)[busIndex] : ((AudioBusBuffers*)_.Outputs)[busIndex];
            var count = bufferCount < busBuffers.NumChannels ? bufferCount : busBuffers.NumChannels;
            for (var i = 0; i < count; i++) ((Single**)busBuffers.ChannelBuffers32)[i] = sampleBuffers != null ? sampleBuffers[i] : null;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffers64(BusDirections dir, int busIndex, Double*[] sampleBuffers, int bufferCount)
        {
            if (channelBufferOwner || _.SymbolicSampleSize != SymbolicSampleSizes.Sample64) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirections.Input ? ((AudioBusBuffers*)_.Inputs)[busIndex] : ((AudioBusBuffers*)_.Outputs)[busIndex];
            var count = bufferCount < busBuffers.NumChannels ? bufferCount : busBuffers.NumChannels;
            for (var i = 0; i < count; i++) ((Double**)busBuffers.ChannelBuffers64)[i] = sampleBuffers != null ? sampleBuffers[i] : null;
            return true;
        }

        // Sets one sample buffer for a given channel inside a bus.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffer(BusDirections dir, int busIndex, int channelIndex, Single* sampleBuffer)
        {
            if (_.SymbolicSampleSize != SymbolicSampleSizes.Sample32) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirections.Input ? ((AudioBusBuffers*)_.Inputs)[busIndex] : ((AudioBusBuffers*)_.Outputs)[busIndex];
            if (channelIndex >= busBuffers.NumChannels) return false;
            ((Single**)busBuffers.ChannelBuffers32)[channelIndex] = sampleBuffer;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetChannelBuffer64(BusDirections dir, int busIndex, int channelIndex, Double* sampleBuffer)
        {
            if (_.SymbolicSampleSize != SymbolicSampleSizes.Sample64) return false;
            if (!IsValidBus(dir, busIndex)) return false;

            var busBuffers = dir == BusDirections.Input ? ((AudioBusBuffers*)_.Inputs)[busIndex] : ((AudioBusBuffers*)_.Outputs)[busIndex];
            if (channelIndex >= busBuffers.NumChannels) return false;
            ((Double**)busBuffers.ChannelBuffers64)[channelIndex] = sampleBuffer;
            return true;
        }

        public const ulong kAllChannelsSilent =
#if SMTG_OS_MACOS
	    0xffffffffffffffffULL;
#else
        0xffffffffffffffffUL;
#endif

        protected int CreateBuffers(IComponent component, ref IntPtr buffers2, BusDirections dir, int bufferSamples)
        {
            AudioBusBuffers* buffers = (AudioBusBuffers*)buffers2;
            var busCount = component.GetBusCount(MediaTypes.Audio, dir);
            if (busCount > 0)
            {
                buffers = (AudioBusBuffers*)Marshal.AllocHGlobal(AudioBusBuffers.Size * busCount);
                for (var i = 0; i < busCount; i++)
                {
                    BusInfo busInfo = new();
                    if (component.GetBusInfo(MediaTypes.Audio, dir, i, ref busInfo) == TResult.S_True)
                    {
                        buffers[i].NumChannels = busInfo.ChannelCount;

                        // allocate for each channel
                        if (busInfo.ChannelCount > 0)
                        {
                            if (_.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                                buffers[i].ChannelBuffers64 = Marshal.AllocHGlobal(sizeof(Double*) * busInfo.ChannelCount);
                            else
                                buffers[i].ChannelBuffers32 = Marshal.AllocHGlobal(sizeof(Single*) * busInfo.ChannelCount);

                            for (var j = 0; j < busInfo.ChannelCount; j++)
                                if (_.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                                    ((Double**)buffers[i].ChannelBuffers64)[j] = bufferSamples > 0 ? (Double*)Marshal.AllocHGlobal(sizeof(Double) * bufferSamples) : null;
                                else
                                    ((Single**)buffers[i].ChannelBuffers32)[j] = bufferSamples > 0 ? (Single*)Marshal.AllocHGlobal(sizeof(Single) * bufferSamples) : null;
                        }
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
                        {
                            if (_.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                            {
                                if (buffers[i].ChannelBuffers64 != IntPtr.Zero && ((Double**)buffers[i].ChannelBuffers64)[j] != null) Marshal.FreeHGlobal((IntPtr)((Double**)buffers[i].ChannelBuffers64)[j]);
                            }
                            else
                            {
                                if (buffers[i].ChannelBuffers32 != IntPtr.Zero && ((Single**)buffers[i].ChannelBuffers32)[j] != null) Marshal.FreeHGlobal((IntPtr)((Single**)buffers[i].ChannelBuffers32)[j]);
                            }
                        }

                    if (_.SymbolicSampleSize == SymbolicSampleSizes.Sample64)
                    {
                        if (buffers[i].ChannelBuffers64 != IntPtr.Zero) Marshal.FreeHGlobal(buffers[i].ChannelBuffers64);
                    }
                    else
                    {
                        if (buffers[i].ChannelBuffers32 != IntPtr.Zero) Marshal.FreeHGlobal(buffers[i].ChannelBuffers32);
                    }
                }

                Marshal.FreeHGlobal(buffers2);
                buffers2 = IntPtr.Zero;
            }
            busCount = 0;
        }

        protected bool CheckIfReallocationNeeded(IComponent component, int bufferSamples, SymbolicSampleSizes symbolicSampleSize)
        {
            if (channelBufferOwner != (bufferSamples > 0)) return true;
            if (_.SymbolicSampleSize != symbolicSampleSize) return true;

            var inBusCount = component.GetBusCount(MediaTypes.Audio, BusDirections.Input);
            if (inBusCount != _.NumInputs) return true;

            var outBusCount = component.GetBusCount(MediaTypes.Audio, BusDirections.Output);
            if (outBusCount != _.NumOutputs) return true;

            for (var i = 0; i < inBusCount; i++)
            {
                BusInfo busInfo = new();
                if (component.GetBusInfo(MediaTypes.Audio, BusDirections.Input, i, ref busInfo) == TResult.S_True)
                    if (((AudioBusBuffers*)_.Inputs)[i].NumChannels != busInfo.ChannelCount) return true;
            }
            for (var i = 0; i < outBusCount; i++)
            {
                BusInfo busInfo = new();
                if (component.GetBusInfo(MediaTypes.Audio, BusDirections.Output, i, ref busInfo) == TResult.S_True)
                    if (((AudioBusBuffers*)_.Outputs)[i].NumChannels != busInfo.ChannelCount) return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool IsValidBus(BusDirections dir, int busIndex)
        {
            if (dir == BusDirections.Input && (_.Inputs == IntPtr.Zero || busIndex >= _.NumInputs)) return false;
            if (dir == BusDirections.Output && (_.Outputs == IntPtr.Zero || busIndex >= _.NumOutputs)) return false;
            return true;
        }
    }
}
