using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// A component can support unit preset data via this interface or program list data(IProgramListData).
    /// </summary>
    [ComImport, Guid(Interfaces.IUnitData), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUnitData
    {
        /// <summary>
        /// Returns kResultTrue if the specified unit supports export and import of preset data.
        /// </summary>
        /// <param name="unitID"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 UnitDataSupported(
            [MarshalAs(UnmanagedType.I4), In] Int32 unitID);

        /// <summary>
        /// Gets the preset data for the specified unit.
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetUnitData(
            [MarshalAs(UnmanagedType.I4), In] Int32 unitId,
            [MarshalAs(UnmanagedType.Interface), In] IBStream data);

        /// <summary>
        /// Sets the preset data for the specified unit.
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetUnitData(
            [MarshalAs(UnmanagedType.I4), In] Int32 unitId,
            [MarshalAs(UnmanagedType.Interface), In] IBStream data);
    }

    static partial class Interfaces
    {
        public const string IUnitData = "6C389611-D391-455D-B870-B83394A0EFDD";
    }
}
