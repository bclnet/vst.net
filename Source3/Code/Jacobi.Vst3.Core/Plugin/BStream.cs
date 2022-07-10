﻿using Jacobi.Vst3.Core;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Plugin
{
    [Flags]
    public enum StreamAccessMode
    {
        None = 0,
        Read = 1,
        Write = 2,
        ReadWrite = 3,
    }

    public sealed class BStream : Stream
    {
        private readonly StreamAccessMode _mode;
        private readonly int _unmanagedBufferSize;
        private IntPtr _unmanagedBuffer;

        public BStream(IBStream streamToWrap, StreamAccessMode mode)
            : this(streamToWrap, mode, 0) { }

        public BStream(IBStream streamToWrap, StreamAccessMode mode, int unmanagedBufferSize)
        {
            BaseStream = streamToWrap;
            SizeableStream = streamToWrap as ISizeableStream;
            _mode = mode;

            if (unmanagedBufferSize > 0)
            {
                _unmanagedBufferSize = unmanagedBufferSize;
                _unmanagedBuffer = Marshal.AllocHGlobal(unmanagedBufferSize);
                GC.AddMemoryPressure(unmanagedBufferSize);
            }
        }

        public IBStream BaseStream { get; private set; }
        public ISizeableStream SizeableStream { get; private set; }

        public override bool CanRead => (_mode & StreamAccessMode.Read) > 0;

        public override bool CanSeek => true;

        public override bool CanWrite => (_mode & StreamAccessMode.Write) > 0;

        public override void Flush() { } // no-op

        public override long Length
        {
            get
            {
                if (SizeableStream != null)
                {
                    var size = 0L;
                    TResult.ThrowIfFailed(SizeableStream.GetStreamSize(out size));
                    return size;
                }

                throw new NotSupportedException();
            }
        }

        public override long Position
        {
            get { var pos = 0L; return TResult.Succeeded(BaseStream.Tell(out pos)) ? pos : -1; }
            set => Seek(value, SeekOrigin.Begin);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var pos = 0L;
            return TResult.Succeeded(BaseStream.Seek(offset, SeekOriginSeekMode(origin), ref pos)) ? pos : -1;
        }

        public override void SetLength(long value)
        {
            if (SizeableStream != null) { TResult.ThrowIfFailed(SizeableStream.SetStreamSize(value)); return; }
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var unmanaged = GetUnmanagedBuffer(ref count);

            try
            {
                if (TResult.Succeeded(BaseStream.Read(unmanaged, count, out var readBytes)))
                {
                    for (var i = 0; i < readBytes; i++)
                        buffer[offset + i] = Marshal.ReadByte(unmanaged, i);

                    return readBytes;
                }
            }
            finally
            {
                ReleaseUnmanagedBuffer(unmanaged);
            }

            return 0;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var unmanaged = GetUnmanagedBuffer(ref count);

            try
            {
                for (var i = 0; i < count; i++) Marshal.WriteByte(unmanaged, i, buffer[offset + i]);

                var result = BaseStream.Write(unmanaged, count, out var writtenBytes);
                TResult.ThrowIfFailed(result);
            }
            finally
            {
                ReleaseUnmanagedBuffer(unmanaged);
            }
        }

        private IntPtr GetUnmanagedBuffer(ref int size)
        {
            if (_unmanagedBuffer != IntPtr.Zero)
            {
                if (size > _unmanagedBufferSize) size = _unmanagedBufferSize;

                return _unmanagedBuffer;
            }

            return Marshal.AllocHGlobal(size);
        }

        private void ReleaseUnmanagedBuffer(IntPtr mem)
        {
            if (_unmanagedBuffer == IntPtr.Zero) Marshal.FreeHGlobal(mem);
        }

        private StreamSeekMode SeekOriginSeekMode(SeekOrigin seekOrigin)
            => seekOrigin switch
            {
                SeekOrigin.Begin => StreamSeekMode.SeekSet,
                SeekOrigin.Current => StreamSeekMode.SeekCur,
                SeekOrigin.End => StreamSeekMode.SeekEnd,
                _ => StreamSeekMode.SeekSet,
            };

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (_unmanagedBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_unmanagedBuffer);
                    GC.RemoveMemoryPressure(_unmanagedBufferSize);
                    _unmanagedBuffer = IntPtr.Zero;
                }

                BaseStream = null;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
