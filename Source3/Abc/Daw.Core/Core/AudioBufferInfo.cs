using Jacobi.Vst.Core;
using Jacobi.Vst.Host.Interop;
using System;

namespace Daw.Core
{
    public unsafe class AudioBufferInfo : IDisposable
    {
        VstAudioBufferManager _vstAudioBufferManager;
        public float*[] Raw { get; private set; }
        public int Count { get; private set; }
        public VstAudioBuffer[] Buffers { get; private set; }

        public AudioBufferInfo(int count, int blockSize)
        {
            Count = count;
            // Create VST.NET output buffers
            Raw = new float*[count];
            Buffers = new VstAudioBuffer[count];
            _vstAudioBufferManager = new VstAudioBufferManager(count, blockSize);
            var bufferEnumerator = _vstAudioBufferManager.Buffers.GetEnumerator();
            bufferEnumerator.MoveNext();
            for (var i = 0; i < count; i++)
            {
                Buffers[i] = (VstAudioBuffer)bufferEnumerator.Current;
                Raw[i] = ((IDirectBufferAccess32)Buffers[i]).Buffer;
                bufferEnumerator.MoveNext();
            }
        }

        public void Dispose() => _vstAudioBufferManager?.Dispose();

        public AudioBufferInfo(int count, AudioBufferInfo parentBuffer)
        {
            // Create child from existing parent buffers
            Count = count;
            Raw = new float*[count];
            Buffers = new VstAudioBuffer[count];
            // Point to parent
            for (var i = 0; i < count; i++)
            {
                Buffers[i] = parentBuffer.Buffers[i];
                Raw[i] = parentBuffer.Raw[i];
            }
        }
    }
}
