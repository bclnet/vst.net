using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// PhysicalUITypeIDs describes the type of Physical UI (PUI) which could be associated to a note expression.
    /// </summary>
    [Flags]
    public enum PhysicalUITypeIDs : ulong
    {
        PUIXMovement = 0,                   // absolute X position when touching keys of PUIs. Range [0=left, 0.5=middle, 1=right]
        PUIYMovement,                       // absolute Y position when touching keys of PUIs. Range [0=bottom/near, 0.5=center, 1=top/far]
        PUIPressure,                        // pressing a key down on keys of PUIs. Range [0=No Pressure, 1=Full Pressure]
        PUITypeCount,                       // count of current defined PUIs
        InvalidPUITypeID = 0xFFFFFFFF,      // indicates that the associatedParameterID is valid and could be used
    }

    /// <summary>
    /// PhysicalUIMap describes a mapping of a noteExpression Type to a Physical UI Type.
    /// It is used in PhysicalUIMapList.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct PhysicalUIMap
    {
        /// <summary>
        /// This represents the physical UI. /see PhysicalUITypeIDs, this is set by the caller of getPhysicalUIMapping
        /// </summary>
        [MarshalAs(UnmanagedType.U4)] public UInt32 PhysicalUITypeID;

        /// <summary>
        /// This represents the associated noteExpression TypeID to the given physicalUITypeID. This
        /// will be filled by the plug-in in the call getPhysicalUIMapping, set it to kInvalidTypeID if
        /// no Note Expression is associated to the given PUI.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)] public UInt32 NoteExpressionTypeID;
    }

    /// <summary>
    /// PhysicalUIMapList describes a list of PhysicalUIMap
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct PhysicalUIMapList
    {
        public static readonly int Size = Marshal.SizeOf<PhysicalUIMapList>();

        /// <summary>
        /// Count of entries in the map array, set by the caller of getPhysicalUIMapping.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)] public UInt32 Count;
        /// <summary>
        /// Pointer to a list of PhysicalUIMap containing count entries.
        /// </summary>
        [MarshalAs(UnmanagedType.SysInt)] public unsafe PhysicalUIMap* Map;
    }
}
