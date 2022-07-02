using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// A component can support program list data via this interface or/and unit preset data(IUnitData).
    /// </summary>
    [ComImport, Guid(Interfaces.IProgramListData), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IProgramListData
    {
        /// <summary>
        /// Returns kResultTrue if the given Program List ID supports Program Data.
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 ProgramDataSupported(
            [MarshalAs(UnmanagedType.I4), In] Int32 listId);

        /// <summary>
        /// Gets for a given program list ID and program index the program Data.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="programIndex"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetProgramData(
            [MarshalAs(UnmanagedType.I4), In] Int32 listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.Interface), In] IBStream data);

        /// <summary>
        /// Sets for a given program list ID and program index a program Data.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="programIndex"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetProgramData(
            [MarshalAs(UnmanagedType.I4), In] Int32 listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.Interface), In] IBStream data);
    }

    static partial class Interfaces
    {
        public const string IProgramListData = "8683B01F-7B35-4F70-A265-1DEC353AF4FF";
    }
}
