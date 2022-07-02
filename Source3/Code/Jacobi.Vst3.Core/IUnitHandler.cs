using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Host callback interface, used with IUnitInfo.
    /// Retrieve via queryInterface from IComponentHandler.
    /// </summary>
    [ComImport, Guid(Interfaces.IUnitHandler), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUnitHandler
    {
        /// <summary>
        /// Notify host when a module is selected in plug-in GUI.
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 NotifyUnitSelection(
            [MarshalAs(UnmanagedType.I4), In] Int32 unitId);

        /// <summary>
        /// Tell host that the plug-in controller changed a program list (rename, load, PitchName changes).
        /// </summary>
        /// <param name="listId">is the specified program list ID to inform.</param>
        /// <param name="programIndex">when kAllProgramInvalid, all program information is invalid, otherwise only the program of given index.</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 NotifyProgramListChange(
            [MarshalAs(UnmanagedType.I4), In] Int32 listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex);
    }

    static partial class Interfaces
    {
        public const string IUnitHandler = "4B5147F8-4654-486B-8DAB-30BA163A3C56";
    }

    /// <summary>
    /// Host callback interface, used with IUnitInfo.
    /// Retrieve via queryInterface from IComponentHandler.
    /// 
    /// The plug-in has the possibility to inform the host with notifyUnitByBusChange that something has
    /// changed in the bus - unit assignment, the host then has to recall IUnitInfo::getUnitByBus in order
    /// to get the new relations between busses and unit.
    /// </summary>
    [ComImport, Guid(Interfaces.IUnitHandler2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUnitHandler2
    {
        /// <summary>
        /// Tell host that assignment Unit-Bus defined by IUnitInfo::getUnitByBus has changed.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 NotifyUnitByBusChange();
    }

    static partial class Interfaces
    {
        public const string IUnitHandler2 = "F89F8CDF-699E-4BA5-96AA-C9A481452B01";
    }
}
