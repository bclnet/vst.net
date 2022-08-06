using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Jacobi.Vst3.Core
{
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
        Int32 GetUnitInfo(
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
        Int32 GetProgramListInfo(
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
        Int32 GetProgramName(
            [MarshalAs(UnmanagedType.I4), In] Int32 listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = Constants.Fixed128), In, Out] StringBuilder name);

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
        Int32 GetProgramInfo(
            [MarshalAs(UnmanagedType.I4), In] Int32 listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.LPWStr), In] String attributeId,
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = Constants.Fixed128), In, Out] StringBuilder attributeValue);

        /// <summary>
        /// Returns kResultTrue if the given program index of a given program list ID supports PitchNames.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="programIndex"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 HasProgramPitchNames(
            [MarshalAs(UnmanagedType.I4), In] Int32 listId,
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
        Int32 GetProgramPitchName(
            [MarshalAs(UnmanagedType.I4), In] Int32 listId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.I4), In] Int16 midiPitch,
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = Constants.Fixed128), In, Out] StringBuilder name);

        // units selection --------------------

        /// <summary>
        /// Gets the current selected unit.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetSelectedUnit();

        /// <summary>
        /// Sets a new selected unit.
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SelectUnit(
            [MarshalAs(UnmanagedType.I4), In] Int32 unitId);

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
        Int32 GetUnitByBus(
            [MarshalAs(UnmanagedType.I4), In] MediaTypes type,
            [MarshalAs(UnmanagedType.I4), In] BusDirections dir,
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.I4), In] Int32 channel,
            [MarshalAs(UnmanagedType.I4), Out] out Int32 unitId);

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
        Int32 SetUnitProgramData(
            [MarshalAs(UnmanagedType.I4), In] Int32 listOrUnitId,
            [MarshalAs(UnmanagedType.I4), In] Int32 programIndex,
            [MarshalAs(UnmanagedType.I4), In] IBStream data);
    }

    static partial class Interfaces
    {
        public const string IUnitInfo = "3D4BD6B5-913A-4FD2-A886-E768A5EB92C1";
    }
}
