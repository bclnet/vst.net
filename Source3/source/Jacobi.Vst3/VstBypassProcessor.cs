using System;
using System.Runtime.InteropServices;

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
                Platform.Memset(mBuffer, 0, count * sizeOf);
            }
        }

        public int MaxSamples => mMaxSamples;

        public void Release() => Resize(0);

        public void ClearAll()
        {
            if (mMaxSamples > 0)
                Clear(mMaxSamples);
        }

        public IntPtr Get() => mBuffer;
    }

    unsafe static partial class Helpers
    {
        public static bool Delay(int sampleFrames, float* inStream, float* outStream, float* delayBuffer, int bufferSize, int bufferInPos, int bufferOutPos)
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
                Platform.Memcpy((IntPtr)bufIn, (IntPtr)inStream, inFrames * sizeof(float)); // copy to buffer
                Platform.Memcpy((IntPtr)outStream, (IntPtr)bufOut, outFrames * sizeof(float)); // copy from buffer

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

                    Platform.Memcpy((IntPtr)outStream, (IntPtr)bufOut, outFrames * sizeof(float)); // copy from buffer

                    outStream += outFrames;

                    bufferOutPos += outFrames;
                    if (bufferOutPos >= bufferSize)
                        bufferOutPos -= bufferSize;
                }

                remain -= inFrames;
            }

            return true;
        }
        public static bool Delay(int sampleFrames, double* inStream, double* outStream, double* delayBuffer, int bufferSize, int bufferInPos, int bufferOutPos)
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
                Platform.Memcpy((IntPtr)bufIn, (IntPtr)inStream, inFrames * sizeof(double)); // copy to buffer
                Platform.Memcpy((IntPtr)outStream, (IntPtr)bufOut, outFrames * sizeof(double)); // copy from buffer

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

                    Platform.Memcpy((IntPtr)outStream, (IntPtr)bufOut, outFrames * sizeof(double)); // copy from buffer

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

}
