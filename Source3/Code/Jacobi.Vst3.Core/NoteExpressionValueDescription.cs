using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Description of a Note Expression Type
    /// This structure is part of the NoteExpressionTypeInfo structure, it describes for given NoteExpressionTypeID its default value
    /// (for example 0.5 for a kTuningTypeID(kIsBipolar: centered)), its minimum and maximum(for predefined NoteExpressionTypeID the full range is predefined too)
    /// and a stepCount when the given NoteExpressionTypeID is limited to discrete values(like on/off state).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct NoteExpressionValueDescription
    {
        public static readonly int Size = Marshal.SizeOf<NoteExpressionValueDescription>();

        [MarshalAs(UnmanagedType.R8)] public Double DefaultValue;		// default normalized value [0,1]
        [MarshalAs(UnmanagedType.R8)] public Double Minimum;			// minimum normalized value [0,1]
        [MarshalAs(UnmanagedType.R8)] public Double Maximum;			// maximum normalized value [0,1]
        [MarshalAs(UnmanagedType.I4)] public Int32 StepCount;			// number of discrete steps (0: continuous, 1: toggle, discrete value otherwise - see \ref vst3parameterIntro)
    }
}
