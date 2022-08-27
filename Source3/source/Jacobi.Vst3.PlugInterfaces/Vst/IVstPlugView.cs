using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3
{
    /// <summary>
    /// Extension for IPlugView to find view parameters (lookup value under mouse support): Vst::IParameterFinder
    /// It is highly recommended to implement this interface.
    /// A host can implement important functionality when a plug-in supports this interface.
    /// For example, all Steinberg hosts require this interface in order to support the "AI Knob".
    /// </summary>
    [ComImport, Guid(Interfaces.IParameterFinder), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IParameterFinder
    {
        /// <summary>
        /// Find out which parameter in plug-in view is at given position (relative to plug-in view).
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="resultTag"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult FindParameter(
            [MarshalAs(UnmanagedType.U4), In] Int32 xPos,
            [MarshalAs(UnmanagedType.U4), In] Int32 yPos,
            [MarshalAs(UnmanagedType.U4), In, Out] UInt32 resultTag);
    }

    partial class Interfaces
    {
        public const string IParameterFinder = "0F618302-215D-4587-A512-073C77B9D383";
    }
}
