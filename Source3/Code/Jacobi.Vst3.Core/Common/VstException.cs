using System;

namespace Jacobi.Vst3.Core.Common
{
    [Serializable]
    public class VstException : Exception
    {
        public VstException(TResult result)
            => Result = result;

        public VstException(TResult result, string message)
            : base(message) => Result = result;

        public VstException(TResult result, string message, Exception inner)
            : base(message, inner) => Result = result;

        protected VstException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public TResult Result { get; private set; }
    }
}
