using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Extended plug-in interface IEditController for note expression event support: Vst::INoteExpressionPhysicalUIMapping
    /// With this plug-in interface, the host can retrieve the preferred physical mapping associated to note expression supported by the plug-in.
    /// When the mapping changes(for example when switching presets) the plug-in needs to inform the host about it via IComponentHandler::restartComponent(kNoteExpressionChanged).
    /// </summary>
    [ComImport, Guid(Interfaces.INoteExpressionPhysicalUIMapping), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface INoteExpressionPhysicalUIMapping
    {
        /// <summary>
        /// Fills the list of mapped [physical UI (in) - note expression (out)] for a given bus index and channel.
        /// </summary>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetPhysicalUIMapping(
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I2), In] Int16 channel,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref PhysicalUIMapList list);
    }

    internal static partial class Interfaces
    {
        public const string INoteExpressionPhysicalUIMapping = "B03078FF-94D2-4AC8-90CC-D303D4133324";
    }
}
