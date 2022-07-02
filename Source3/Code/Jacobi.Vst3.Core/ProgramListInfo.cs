using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Basic Program List Description.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct ProgramListInfo
    {
        public static readonly int Size = Marshal.SizeOf<ProgramListInfo>();

        /* Special programIndex value for IUnitHandler::notifyProgramListChange */
        public const Int32 AllProgramInvalid = -1;	    // all program information is invalid

        [MarshalAs(UnmanagedType.I4)] public Int32 Id;				        // program list identifier
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.Fixed128)] public String Name; // name of program list
        [MarshalAs(UnmanagedType.I4)] public Int32 ProgramCount;			// number of programs in this list
    }
}
