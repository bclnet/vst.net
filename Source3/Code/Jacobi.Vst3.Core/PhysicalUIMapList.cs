using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct PhysicalUIMapList
    {
        public static readonly int Size = Marshal.SizeOf<PhysicalUIMapList>();

        [MarshalAs(UnmanagedType.U4)] public UInt32 Count;			            // Count of entries in the map array, set by the caller of getPhysicalUIMapping.
        [MarshalAs(UnmanagedType.SysInt)] public unsafe PhysicalUIMap* Map;	    // Pointer to a list of PhysicalUIMap containing count entries.
    }

    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct PhysicalUIMap
    {
        [MarshalAs(UnmanagedType.U4)] public UInt32 PhysicalUITypeID; // This represents the physical UI. /see PhysicalUITypeIDs, this is set by the caller of getPhysicalUIMapping

        // This represents the associated noteExpression TypeID to the given physicalUITypeID. This
        // will be filled by the plug-in in the call getPhysicalUIMapping, set it to kInvalidTypeID if
        // no Note Expression is associated to the given PUI.
        [MarshalAs(UnmanagedType.U4)] public UInt32 NoteExpressionTypeID;
    }

    [Flags]
    public enum PhysicalUITypeIDs : ulong
    {
        PUIXMovement = 0,                   // absolute X position when touching keys of PUIs. Range [0=left, 0.5=middle, 1=right]
        PUIYMovement,                       // absolute Y position when touching keys of PUIs. Range [0=bottom/near, 0.5=center, 1=top/far]
        PUIPressure,                        // pressing a key down on keys of PUIs. Range [0=No Pressure, 1=Full Pressure]
        PUITypeCount,                       // count of current defined PUIs
        InvalidPUITypeID = 0xFFFFFFFF,      // indicates that the associatedParameterID is valid and could be used
    }
}
