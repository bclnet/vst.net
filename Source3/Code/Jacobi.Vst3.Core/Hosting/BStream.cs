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

        public TResult Read(IntPtr buffer, int numBytes, out int numBytesRead)
            => throw new NotImplementedException();

        public TResult Write(IntPtr buffer, int numBytes, out int numBytesWritten)
            => throw new NotImplementedException();

        public TResult Seek(long pos, SeekOrigin mode, out long result)
            => throw new NotImplementedException();

        public TResult Tell(out long pos)
            => throw new NotImplementedException();

        #endregion
    }
}
