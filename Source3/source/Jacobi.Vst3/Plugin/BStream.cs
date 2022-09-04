using Steinberg.Vst3;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Steinberg.Vst3
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
        readonly StreamAccessMode _mode;
        readonly int _unmanagedBufferSize;
        IntPtr _unmanagedBuffer;

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
                    SizeableStream.GetStreamSize(out size).ThrowIfFailed();
                    return size;
                }

                throw new NotSupportedException();
            }
        }

        public override long Position
        {
            get { var pos = 0L; return BaseStream.Tell(out pos).Succeeded() ? pos : -1; }
            set => Seek(value, SeekOrigin.Begin);
        }

        public override long Seek(long offset, SeekOrigin origin)
            => BaseStream.Seek(offset, origin, out var pos).Succeeded() ? pos : -1;

        public override void SetLength(long value)
        {
            if (SizeableStream != null) { SizeableStream.SetStreamSize(value).ThrowIfFailed(); return; }
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var unmanaged = GetUnmanagedBuffer(ref count);

            try
            {
                if (BaseStream.Read(unmanaged, count, out var readBytes).Succeeded())
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
                result.ThrowIfFailed();
            }
            finally
            {
                ReleaseUnmanagedBuffer(unmanaged);
            }
        }

        IntPtr GetUnmanagedBuffer(ref int size)
        {
            if (_unmanagedBuffer != IntPtr.Zero)
            {
                if (size > _unmanagedBufferSize) size = _unmanagedBufferSize;

                return _unmanagedBuffer;
            }

            return Marshal.AllocHGlobal(size);
        }

        void ReleaseUnmanagedBuffer(IntPtr mem)
        {
            if (_unmanagedBuffer == IntPtr.Zero) Marshal.FreeHGlobal(mem);
        }

        //SeekOrigin SeekOriginSeekMode(SeekOrigin seekOrigin)
        //    => seekOrigin switch
        //    {
        //        SeekOrigin.Begin => SeekOrigin.SeekSet,
        //        SeekOrigin.Current => SeekOrigin.SeekCur,
        //        SeekOrigin.End => SeekOrigin.SeekEnd,
        //        _ => SeekOrigin.SeekSet,
        //    };

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
