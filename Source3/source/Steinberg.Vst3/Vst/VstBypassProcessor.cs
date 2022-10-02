using System;
using System.Runtime.InteropServices;
using static Steinberg.Vst3.TResult;
using static Steinberg.Vst3.Speaker;
using System.Reflection.PortableExecutable;
using static Steinberg.Vst3.Hosting.ClassInfo;
using System.Security.Cryptography;
using System.Threading.Channels;
using Steinberg.Vst3;

namespace Steinberg.Vst3
{
    public unsafe class AudioBuffer : IDisposable
    {
        protected int sizeOf;
        protected IntPtr mBuffer;
        protected int mMaxSamples;

        public AudioBuffer(int sizeOf)
            => this.sizeOf = sizeOf;

        public void Dispose()
            => Resize(0);

        public void Resize(int maxSamples)
        {
            if (mMaxSamples != maxSamples)
            {
                mMaxSamples = maxSamples;
                if (mMaxSamples <= 0)
                {
                    if (mBuffer != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(mBuffer);
                        mBuffer = IntPtr.Zero;
                    }
                }
                else
                {
                    mBuffer = mBuffer != IntPtr.Zero
                        ? Marshal.ReAllocHGlobal(mBuffer, (IntPtr)(mMaxSamples * sizeOf))
                        : Marshal.AllocHGlobal(mMaxSamples * sizeOf);
                }
            }
        }

        public void Clear(int numSamples)
        {
            if (mBuffer != IntPtr.Zero)
            {
                var count = numSamples < mMaxSamples ? numSamples : mMaxSamples;
                Platform.memset(mBuffer, 0, count * sizeOf);
            }
        }

        public int MaxSamples => mMaxSamples;

        public void Release() => Resize(0);

        public void ClearAll()
        {
            if (mMaxSamples > 0)
                Clear(mMaxSamples);
        }

        public IntPtr Ptr => mBuffer;
    }

    unsafe partial class BypassProcessor
    {
        internal static bool Delay32(int sampleFrames, float* inStream, float* outStream, float* delayBuffer, int bufferSize, int bufferInPos, int bufferOutPos)
        {
            // delay inStream
            int remain, inFrames, outFrames;
            float* bufIn;
            float* bufOut;

            remain = sampleFrames;
            while (remain > 0)
            {
                bufIn = delayBuffer + bufferInPos;
                bufOut = delayBuffer + bufferOutPos;

                inFrames = bufferInPos > bufferOutPos
                    ? bufferSize - bufferInPos
                    : bufferOutPos - bufferInPos;

                outFrames = bufferSize - bufferOutPos;

                if (inFrames > remain)
                    inFrames = remain;

                if (outFrames > inFrames)
                    outFrames = inFrames;

                // order important for in-place processing!
                Platform.memcpy((IntPtr)bufIn, (IntPtr)inStream, inFrames * sizeof(float)); // copy to buffer
                Platform.memcpy((IntPtr)outStream, (IntPtr)bufOut, outFrames * sizeof(float)); // copy from buffer

                inStream += inFrames;
                outStream += outFrames;

                bufferInPos += inFrames;
                if (bufferInPos >= bufferSize)
                    bufferInPos -= bufferSize;
                bufferOutPos += outFrames;
                if (bufferOutPos >= bufferSize)
                    bufferOutPos -= bufferSize;

                if (inFrames > outFrames)
                {
                    // still some output to copy
                    bufOut = delayBuffer + bufferOutPos;
                    outFrames = inFrames - outFrames;

                    Platform.memcpy((IntPtr)outStream, (IntPtr)bufOut, outFrames * sizeof(float)); // copy from buffer

                    outStream += outFrames;

                    bufferOutPos += outFrames;
                    if (bufferOutPos >= bufferSize)
                        bufferOutPos -= bufferSize;
                }

                remain -= inFrames;
            }

            return true;
        }

        internal static bool Delay64(int sampleFrames, double* inStream, double* outStream, double* delayBuffer, int bufferSize, int bufferInPos, int bufferOutPos)
        {
            // delay inStream
            int remain, inFrames, outFrames;
            double* bufIn;
            double* bufOut;

            remain = sampleFrames;
            while (remain > 0)
            {
                bufIn = delayBuffer + bufferInPos;
                bufOut = delayBuffer + bufferOutPos;

                inFrames = bufferInPos > bufferOutPos
                    ? bufferSize - bufferInPos
                    : bufferOutPos - bufferInPos;

                outFrames = bufferSize - bufferOutPos;

                if (inFrames > remain)
                    inFrames = remain;

                if (outFrames > inFrames)
                    outFrames = inFrames;

                // order important for in-place processing!
                Platform.memcpy((IntPtr)bufIn, (IntPtr)inStream, inFrames * sizeof(double)); // copy to buffer
                Platform.memcpy((IntPtr)outStream, (IntPtr)bufOut, outFrames * sizeof(double)); // copy from buffer

                inStream += inFrames;
                outStream += outFrames;

                bufferInPos += inFrames;
                if (bufferInPos >= bufferSize)
                    bufferInPos -= bufferSize;
                bufferOutPos += outFrames;
                if (bufferOutPos >= bufferSize)
                    bufferOutPos -= bufferSize;

                if (inFrames > outFrames)
                {
                    // still some output to copy
                    bufOut = delayBuffer + bufferOutPos;
                    outFrames = inFrames - outFrames;

                    Platform.memcpy((IntPtr)outStream, (IntPtr)bufOut, outFrames * sizeof(double)); // copy from buffer

                    outStream += outFrames;

                    bufferOutPos += outFrames;
                    if (bufferOutPos >= bufferSize)
                        bufferOutPos -= bufferSize;
                }

                remain -= inFrames;
            }

            return true;
        }
    }

    public unsafe partial class BypassProcessor : IDisposable
    {
        const int kMaxChannelsSupported = 64;

        protected int sizeOf;
        protected int[] mInputPinLookup = new int[kMaxChannelsSupported];
        protected Delay[] mDelays = new Delay[kMaxChannelsSupported];
        protected bool mActive;
        protected bool mMainIOBypass;

        public class Delay
        {
            int sizeOf;
            AudioBuffer mDelayBuffer;
            int mDelaySamples;
            int mInPos;
            int mOutPos;

            public Delay(int sizeOf, int maxSamplesPerBlock, int delaySamples)
            {
                this.sizeOf = sizeOf;
                mDelayBuffer = new AudioBuffer(sizeOf);
                mInPos = -1;
                mOutPos = -1;
                mDelaySamples = delaySamples;
                if (mDelaySamples > 0)
                    mDelayBuffer.Resize(maxSamplesPerBlock + mDelaySamples);
            }

            public bool HasDelay => mDelaySamples > 0;

            public int BufferSamples => mDelayBuffer.MaxSamples;

            public bool Process(void* src, void* dst, int numSamples, bool silentIn)
            {
                var silentOut = false;

                if (HasDelay && src != null)
                {
                    var bufferSize = BufferSamples;
                    if (sizeOf == 32)
                        Delay32(numSamples, (float*)src, (float*)dst, (float*)mDelayBuffer.Ptr, bufferSize, mInPos, mOutPos);
                    else
                        Delay64(numSamples, (double*)src, (double*)dst, (double*)mDelayBuffer.Ptr, bufferSize, mInPos, mOutPos);

                    // update inPos, outPos
                    mInPos += numSamples;
                    if (mInPos >= bufferSize)
                        mInPos -= bufferSize;
                    mOutPos += numSamples;
                    if (mOutPos >= bufferSize)
                        mOutPos -= bufferSize;
                }
                else
                {
                    if (src != dst)
                    {
                        if (src != null && !silentIn)
                            Platform.memcpy((IntPtr)dst, (IntPtr)src, numSamples * sizeOf);
                        else
                        {
                            Platform.memset((IntPtr)dst, 0, numSamples * sizeOf);
                            silentOut = true;
                        }
                    }
                    else
                        silentOut = silentIn;
                }
                return silentOut;
            }

            public void Flush()
            {
                mDelayBuffer.ClearAll();

                mInPos = mOutPos = 0;
                if (HasDelay)
                    mOutPos = BufferSamples - mDelaySamples; // must be != inPos
            }
        }

        public BypassProcessor(int sizeOf)
        {
            this.sizeOf = sizeOf;
            for (var i = 0; i < kMaxChannelsSupported; i++)
            {
                mInputPinLookup[i] = -1;
                mDelays[i] = null;
            }
        }

        public void Dispose()
            => Reset();

        public void Setup(IAudioProcessor audioProcessor, ref ProcessSetup processSetup, int delaySamples)
        {
            Reset();

            var hasInput = audioProcessor.GetBusArrangement(BusDirection.Input, 0, out var inputArr) == kResultOk;

            var hasOutput = audioProcessor.GetBusArrangement(BusDirection.Output, 0, out var outputArr) == kResultOk;

            mMainIOBypass = hasInput && hasOutput;
            if (!mMainIOBypass)
                return;

            // create lookup table (in <- out) and delays...
            SpeakerArray inArray = new(inputArr);
            SpeakerArray outArray = new(outputArr);

            // security check (todo)
            if (outArray.Total >= kMaxChannelsSupported)
                return;

            for (var i = 0; i < outArray.Total; i++)
            {
                mInputPinLookup[i] = outArray[i] == kSpeakerL
                    ? inArray.Total == 1 && inArray[0] == kSpeakerM
                        ? 0
                        : inArray.GetSpeakerIndex(outArray[i])
                    : inArray.GetSpeakerIndex(outArray[i]);

                mDelays[i] = new Delay(sizeOf, processSetup.MaxSamplesPerBlock, delaySamples);
                mDelays[i].Flush();
            }
        }

        public void Reset()
        {
            mMainIOBypass = false;

            for (var i = 0; i < kMaxChannelsSupported; i++)
            {
                mInputPinLookup[i] = -1;
                if (mDelays[i] != null)
                    mDelays[i] = null;
            }
        }

        public bool Active
        {
            get => mActive;
            set
            {
                if (mActive == value)
                    return;

                mActive = value;

                // flush delays when turning on
                if (value && mMainIOBypass)
                    for (var i = 0; i < kMaxChannelsSupported; i++)
                    {
                        if (mDelays[i] == null)
                            break;
                        mDelays[i].Flush();
                    }
            }
        }

        public void Process(ref ProcessData data)
        {
            // flush
            if (data.NumInputs == 0 || data.NumOutputs == 0)
                return;

            var inBus = data.Inputs[0];
            var outBus = data.Outputs[0];

            if (data.SymbolicSampleSize == SymbolicSampleSizes.Sample32)
            {
                if (outBus.ChannelBuffers32 == null)
                    return;
            }
            else if (outBus.ChannelBuffers64 == null)
                return;

            if (mMainIOBypass)
            {
                for (var channel = 0; channel < outBus.NumChannels; channel++)
                {
                    void* src = null;
                    var silent = true;
                    var dst = data.SymbolicSampleSize == SymbolicSampleSizes.Sample32
                        ? (void*)outBus.ChannelBuffers32[channel]
                        : (void*)outBus.ChannelBuffers64[channel];
                    if (dst == null)
                        continue;

                    var inputChannel = mInputPinLookup[channel];
                    if (inputChannel != -1)
                    {
                        silent = (inBus.SilenceFlags & (1Ul << inputChannel)) != 0;
                        src = data.SymbolicSampleSize == SymbolicSampleSizes.Sample32
                            ? (void*)inBus.ChannelBuffers32[inputChannel]
                            : (void*)inBus.ChannelBuffers64[inputChannel];
                    }

                    if (mDelays[channel].Process(src, dst, data.NumSamples, silent))
                        outBus.SilenceFlags |= 1Ul << channel;
                    else
                        outBus.SilenceFlags = 0;
                }
            }

            // clear all other outputs
            for (var outBusIndex = mMainIOBypass ? 1 : 0; outBusIndex < data.NumOutputs; outBusIndex++)
            {
                outBus = data.Outputs[outBusIndex];

                for (var channel = 0; channel < outBus.NumChannels; channel++)
                {
                    var dst = data.SymbolicSampleSize == SymbolicSampleSizes.Sample32
                        ? (void*)outBus.ChannelBuffers32[channel]
                        : (void*)outBus.ChannelBuffers64[channel];
                    if (dst != null)
                    {
                        Platform.memset((IntPtr)dst, 0, data.NumSamples * (data.SymbolicSampleSize == SymbolicSampleSizes.Sample32 ? sizeof(float) : sizeof(double)));
                        outBus.SilenceFlags |= 1Ul << channel;
                    }
                }
            }
        }
    }
}
