using System.Runtime.CompilerServices;
using ColorSpec = System.UInt32;
using ColorComponent = System.Byte;

namespace Jacobi.Vst3.Core
{
    public static class ColorSpecX
    {
        /// <summary>
        /// Returns the Blue part of the given ColorSpec 
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorComponent GetBlue(ColorSpec cs) => (ColorComponent)(cs & 0x000000FF);

        /// <summary>
        /// Returns the Green part of the given ColorSpec
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorComponent GetGreen(ColorSpec cs) => (ColorComponent)((cs >> 8) & 0x000000FF);

        /// <summary>
        /// Returns the Red part of the given ColorSpec
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorComponent GetRed(ColorSpec cs) => (ColorComponent)((cs >> 16) & 0x000000FF);

        /// <summary>
        /// Returns the Alpha part of the given ColorSpec
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorComponent GetAlpha(ColorSpec cs) => (ColorComponent)((cs >> 24) & 0x000000FF);
    }
}
