using Daw.Core;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.IO;
using System.Threading;

namespace Daw.Asio
{
    public class AsioMP3Recorder : IDisposable
    {
        LameMP3FileWriter _mp3Writer;
        int _blockSize;
        FileStream _fs;
        Thread _readThread;
        ByteArrayRingBuffer _ringBuffer;
        ManualResetEvent _dataReady = new(false);
        ManualResetEvent _exitEvent = new(false);

        public event EventHandler Closing;

        public string FileName { get; private set; }

        public AsioMP3Recorder(string path, int rate, int bits, int channels)
        {
            FileName = path;
            _fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            _mp3Writer = new LameMP3FileWriter(_fs, new WaveFormat(rate, bits, channels), 192);
            _blockSize = 8192; //: _mp3Writer.OptimalBufferSize;
            _ringBuffer = new ByteArrayRingBuffer(_blockSize * 4);   // Fit 4x write block size in buffer
            _readThread = new Thread(ReadThread) { Priority = ThreadPriority.BelowNormal, IsBackground = true };
            _readThread.Start();
        }

        public void Dispose()
        {
            _mp3Writer?.Dispose();
            _dataReady?.Dispose();
            _exitEvent?.Dispose();
            _fs?.Dispose();
        }

        void ReadThread()
        {
            byte[] data;
            while (true)
            {
                // Check if any data in ring buffer
                lock (_ringBuffer) if (_ringBuffer.Count < _blockSize) _dataReady.Reset();

                // Wait untill data is received 
                var i = WaitHandle.WaitAny(new WaitHandle[] { _dataReady, _exitEvent });
                var count = _ringBuffer.Count;
                if (i == 1 && count < _blockSize)
                {
                    // Write remaining data in ringbuffer
                    if (_ringBuffer.Count > 0) _mp3Writer.Write(_ringBuffer.Get(count), 0, count);
                    _mp3Writer.Close();
                    _fs.Close();
                    return; // Exit!
                }
                lock (_ringBuffer) data = _ringBuffer.Get(_blockSize); // Copy the block data 
                _mp3Writer.Write(data, 0, _blockSize); // Handle data
            }
        }

        static byte[] SampleToBytes(float sample)
        {
            // clip
            if (sample > 1.0f) sample = 1.0f;
            if (sample < -1.0f) sample = -1.0f;
            var i16Val = (short)(sample * 32767);
            return BitConverter.GetBytes(i16Val);
        }

        //public bool WriteSamples(Channel left, Channel right, int size)
        //{
        //    // Convert stereo samples to 2xint16
        //    var arrSize = size * 2 * 2;
        //    var data = new byte[arrSize];
        //    var dst = 0;
        //    for (var i = 0; i < size; i++)
        //    {
        //        SampleToBytes(left[i]).CopyTo(data, dst); dst += 2;
        //        SampleToBytes(right[i]).CopyTo(data, dst); dst += 2;
        //    }

        //    lock (_ringBuffer)
        //    {
        //        if (!_ringBuffer.Put(data, arrSize)) return false;
        //        if (_ringBuffer.Count >= _blockSize) _dataReady.Set();
        //    }
        //    return true;
        //}

        /// <summary>
        /// Non-blocking close
        /// </summary>
        public void Close()
        {
            Closing?.Invoke(this, null);
            _exitEvent.Set();
        }
    }
}