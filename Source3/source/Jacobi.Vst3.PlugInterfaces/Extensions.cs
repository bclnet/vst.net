using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3
{
    public static partial class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToPackedString(this Guid source)
            => source.ToString().Replace("-", "").ToUpper();

        #region Test

        //ref: https://stackoverflow.com/questions/29067873/convert-func-delegate-to-a-string
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Expect(this ITestResult source, Expression<Func<bool>> condition)
        {
            if (!condition.Compile()())
            {
                var frame = new StackTrace(true).GetFrame(0);
                var expression = condition.Body.ToString()
                    .Replace("AndAlso", "&&")
                    .Replace("OrElse", "||");
                source.AddErrorMessage($"{frame.GetFileName()}:{frame.GetFileLineNumber()}: error: {expression}");
                return false;
            }
            return true;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static bool ExpectNot(this ITestResult source, Expression<Func<bool>> condition)
        //{
        //    var compiled = condition.Compile();
        //    condition = () => !compiled();
        //    return Expect(source, condition);
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExpectTrue(this ITestResult source, bool condition)
            => Expect(source, () => condition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExpectFalse(this ITestResult source, bool condition)
            => Expect(source, () => !condition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExpectEQ<T>(this ITestResult source, T var1, T var2)
        {
            var condition = Expression.Lambda<Func<bool>>(Expression.Equal(
                Expression.Constant(var1),
                Expression.Constant(var2)));
            return Expect(source, condition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExpectNE<T>(this ITestResult source, T var1, T var2)
        {
            var condition = Expression.Lambda<Func<bool>>(Expression.NotEqual(
                Expression.Constant(var1),
                Expression.Constant(var2)));
            return Expect(source, condition);
        }

        #endregion

        #region IntPtr (unknown)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Cast<T>(this IntPtr unknownPtr) where T : class
            => unknownPtr == IntPtr.Zero
            ? null
            : Marshal.GetObjectForIUnknown(unknownPtr) as T;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CastT<T>(this T unknown) where T : class
            => unknown == null
            ? IntPtr.Zero
            : Marshal.GetIUnknownForObject(unknown);

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T>(this T value, T min, T max)
            where T : IComparable<T>
            => value.CompareTo(max) > 0 ? max
            : value.CompareTo(min) < 0 ? min
            : value;

        /// <summary>
        /// Helper to allocate a message
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMessage AllocateMessage(this IHostApplication host)
        {
            var msgType = typeof(IMessage);
            var iid = msgType.GUID;
            var ptr = IntPtr.Zero;
            var result = host.CreateInstance(ref iid, ref iid, out ptr);
            return result.Succeeded()
                ? (IMessage)Marshal.GetTypedObjectForIUnknown(ptr, msgType)
                : default;
            //try { return (IMessage)Marshal.GetTypedObjectForIUnknown(ptr, msgType); }
            //finally { Marshal.Release(ptr); }
        }

    }
}
