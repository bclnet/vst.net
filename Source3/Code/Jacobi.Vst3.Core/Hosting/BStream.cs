using System;
using System.IO;

namespace Jacobi.Vst3.Core
{
    public class BStream : IBStream
    {
        public BStream(Stream streamToWrap)
            => BaseStream = streamToWrap;

        protected Stream BaseStream { get; private set; }

        #region IBStream Members

        public int Read(IntPtr buffer, int numBytes, ref int numBytesRead)
            => throw new NotImplementedException();

        public int Write(IntPtr buffer, int numBytes, ref int numBytesWritten)
            => throw new NotImplementedException();

        public int Seek(long pos, StreamSeekMode mode, ref long result)
            => throw new NotImplementedException();

        public int Tell(out long pos)
            => throw new NotImplementedException();

        #endregion
    }
}
