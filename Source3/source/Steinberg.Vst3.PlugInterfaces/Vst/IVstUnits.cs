using System;
using System.Runtime.InteropServices;
using System.Text;
using ProgramListID = System.Int32;
using UnitID = System.Int32;

namespace Steinberg.Vst3
{
    public partial struct UnitInfo
    {
        /* Special UnitIDs for UnitInfo */
        public const UnitID RootUnitId = 0;          // identifier for the top level unit (root)
        public const UnitID NoParentUnitId = -1;	    // used for the root unit which doesn't have a parent.

        /* Special ProgramListIDs for UnitInfo */
        public const ProgramListID NoProgramListId = -1;	// no programs are used in the unit.
    }

    /// <summary>
    /// Basic Unit Description.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public partial struct UnitInfo
    {
        public static readonly int Size = Marshal.SizeOf<UnitInfo>();

        [MarshalAs(UnmanagedType.I4)] public Int32 Id;						    // unit identifier
        [MarshalAs(UnmanagedType.I4)] public Int32 ParentUnitId;			    // identifier of parent unit (kNoParentUnitId: does not apply, this unit is the root)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Platform.Fixed128)] public String Name; // name, optional for the root component, required otherwise
        [MarshalAs(UnmanagedType.I4)] public Int32 ProgramListId;	            // id of program list used in unit (kNoProgramListId = no programs used in this unit)
    }

    /// <summary>
    /// Basic Program List Description.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct ProgramListInfo
    {
        public static readonly int Size = Marshal.SizeOf<ProgramListInfo>();

        /* Special programIndex value for IUnitHandler::notifyProgramListChange */
        public const Int32 AllProgramInvalid = -1;	    // all program information is invalid

        [MarshalAs(UnmanagedType.I4)] public ProgramListID Id;				        // program list identifier
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Platform.Fixed128)] public String Name; // name of program list
        [MarshalAs(UnmanagedType.I4)] public Int32 ProgramCount;			// number of programs in this list
    }

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
        TResult NotifyUnitSelection(
            [MarshalAs(UnmanagedType.I4), In] UnitID unitId);

        /// <summary>
        /// Tell host that the plug-in controller changed a program list (rename, load, PitchName changes).
        /// </summary>
        /// <param name="listId">is the specified program list ID to inform.</param>
        /// <param name="programIndex">when kAllProgramInvalid, all program information is invalid, otherwise only the program of given index.</param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult NotifyProgramListChange(
            [MarshalAs(UnmanagedType.I4), In] ProgramListID listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex);
    }

    partial class Interfaces
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
        TResult NotifyUnitByBusChange();
    }

    partial class Interfaces
    {
        public const string IUnitHandler2 = "F89F8CDF-699E-4BA5-96AA-C9A481452B01";
    }

    /// <summary>
    /// IUnitInfo describes the internal structure of the plug-in.
    /// - The root unit is the component itself, so getUnitCount must return 1 at least.
    /// - The root unit id has to be 0 (kRootUnitId).
    /// - Each unit can reference one program list - this reference must not change.
    /// - Each unit, using a program list, references one program of the list.
    /// </summary>
    [ComImport, Guid(Interfaces.IUnitInfo), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUnitInfo
    {
        /// <summary>
        /// Returns the flat count of units.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetUnitCount();

        /// <summary>
        /// Gets UnitInfo for a given index in the flat list of unit.
        /// </summary>
        /// <param name="unitIndex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetUnitInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 unitIndex,
            [MarshalAs(UnmanagedType.Struct), Out] out UnitInfo info);

        /* Component intern program structure. */

        /// <summary>
        /// Gets the count of Program List.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetProgramListCount();

        /// <summary>
        /// Gets for a given index the Program List Info.
        /// </summary>
        /// <param name="listIndex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetProgramListInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 listIndex,
            [MarshalAs(UnmanagedType.I4), Out] out ProgramListInfo info);

        /// <summary>
        /// Gets for a given program list ID and program index its program name.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="programIndex"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetProgramName(
            [MarshalAs(UnmanagedType.I4), In] ProgramListID listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = Platform.Fixed128), In, Out] StringBuilder name);

        /// <summary>
        /// Gets for a given program list ID, program index and attributeId the associated attribute value.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="programIndex"></param>
        /// <param name="attributeId"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetProgramInfo(
            [MarshalAs(UnmanagedType.I4), In] ProgramListID listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.LPWStr), In] String attributeId,
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = Platform.Fixed128), In, Out] StringBuilder attributeValue);

        /// <summary>
        /// Returns kResultTrue if the given program index of a given program list ID supports PitchNames.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="programIndex"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult HasProgramPitchNames(
            [MarshalAs(UnmanagedType.I4), In] ProgramListID listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex);

        /// <summary>
        /// Gets the PitchName for a given program list ID, program index and pitch.
        /// If PitchNames are changed the plug-in should inform the host with IUnitHandler::notifyProgramListChange.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="programIndex"></param>
        /// <param name="midiPitch"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetProgramPitchName(
            [MarshalAs(UnmanagedType.I4), In] ProgramListID listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.I4), In] Int16 midiPitch,
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = Platform.Fixed128), In, Out] StringBuilder name);

        // units selection --------------------

        /// <summary>
        /// Gets the current selected unit.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        UnitID GetSelectedUnit();

        /// <summary>
        /// Sets a new selected unit.
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SelectUnit(
            [MarshalAs(UnmanagedType.I4), In] UnitID unitId);

        /// <summary>
        /// Gets the according unit if there is an unambiguous relation between a channel or a bus and a unit.
	    /// This method mainly is intended to find out which unit is related to a given MIDI input channel.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dir"></param>
        /// <param name="busIndex"></param>
        /// <param name="channel"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetUnitByBus(
            [MarshalAs(UnmanagedType.I4), In] MediaType type,
            [MarshalAs(UnmanagedType.I4), In] BusDirection dir,
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I4), In] Int32 channel,
            [MarshalAs(UnmanagedType.I4), Out] out UnitID unitId);

        /// <summary>
        /// Receives a preset data stream.
        /// - If the component supports program list data(IProgramListData), the destination of the data stream is the program specified by list-Id and program index(first and second parameter)
        /// - If the component supports unit data(IUnitData), the destination is the unit specified by the first parameter - in this case parameter programIndex is < 0).
        /// </summary>
        /// <param name="listOrUnitId"></param>
        /// <param name="programIndex"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetUnitProgramData(
            [MarshalAs(UnmanagedType.I4), In] Int32 listOrUnitId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.I4), In] IBStream data);
    }

    partial class Interfaces
    {
        public const string IUnitInfo = "3D4BD6B5-913A-4FD2-A886-E768A5EB92C1";
    }

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
        TResult ProgramDataSupported(
            [MarshalAs(UnmanagedType.I4), In] ProgramListID listId);

        /// <summary>
        /// Gets for a given program list ID and program index the program Data.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="programIndex"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetProgramData(
            [MarshalAs(UnmanagedType.I4), In] ProgramListID listId,
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
        TResult SetProgramData(
            [MarshalAs(UnmanagedType.I4), In] ProgramListID listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.Interface), In] IBStream data);
    }

    partial class Interfaces
    {
        public const string IProgramListData = "8683B01F-7B35-4F70-A265-1DEC353AF4FF";
    }

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
        TResult UnitDataSupported(
            [MarshalAs(UnmanagedType.I4), In] UnitID unitID);

        /// <summary>
        /// Gets the preset data for the specified unit.
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetUnitData(
            [MarshalAs(UnmanagedType.I4), In] UnitID unitId,
            [MarshalAs(UnmanagedType.Interface), In] IBStream data);

        /// <summary>
        /// Sets the preset data for the specified unit.
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult SetUnitData(
            [MarshalAs(UnmanagedType.I4), In] UnitID unitId,
            [MarshalAs(UnmanagedType.Interface), In] IBStream data);
    }

    partial class Interfaces
    {
        public const string IUnitData = "6C389611-D391-455D-B870-B83394A0EFDD";
    }
}
