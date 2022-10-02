using System;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3
{
    // success (positive) and error (negative) codes 
    public enum TResult : int
    {
        kNoInterface = -2147467262,     // E_NOINTERFACE
        kResultOk = 0,                  // S_OK
        kResultTrue = kResultOk,
        kResultFalse = 1,               // S_FALSE
        kInvalidArgument = -2147024809, // E_INVALIDARG
        kNotImplemented = -2147467263,  // E_NOTIMPL
        kInternalError = -2147467259,   // E_FAIL
        //kInternalError = ??
        kNotInitialized = -2147418113,  // E_UNEXPECTED
        kOutOfMemory = -2147024882,     // E_OUTOFMEMORY
        E_Pointer = -2147467261,
        E_ClassNotReg = -2147221164,
        E_Abort = -2147467260,
    }

    public static partial class PlugExtensions
    {
        public static bool Succeeded(this TResult result) => result >= 0;
        public static bool Failed(this TResult result) => result < 0;
        public static bool IsTrue(this TResult result) => result == kResultTrue;
        public static bool IsFalse(this TResult result) => result == kResultFalse;
        public static void ThrowIfFailed(this TResult result) { if (Failed(result)) throw new VstException(result); }
    }

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
