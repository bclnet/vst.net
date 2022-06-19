using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    public static class FunctionNameTypes
    {
        public const string CompGainReduction = "Comp:GainReduction";
        public const string CompGainReductionMax = "Comp:GainReductionMax";
        public const string CompGainReductionPeakHold = "Comp:GainReductionPeakHold";
        public const string CompResetGainReductionMax = "Comp:ResetGainReductionMax";

        public const string LowLatencyMode = "LowLatencyMode";  // Useful for live situation where low latency is required:
                                                                // 0 means LowLatency disable,
                                                                // 1 means LowLatency enable
        public const string DryWetMix = "DryWetMix";            // Allowing to mix the original (Dry) Signal with the processed one (Wet):
                                                                // 0.0 means Dry Signal only,
                                                                // 0.5 means 50% Dry Signal + 50% Wet Signal,
                                                                // 1.0 means Wet Signal only
        public const string Randomize = "Randomize";            // Allow to assign some randomized values to some parameters in a controlled way

        public const string PanPosCenterX = "PanPosCenterX";    // Gravity point X-axis [0, 1]=>[L-R] (for stereo: middle between left and right)
        public const string PanPosCenterY = "PanPosCenterY";    // Gravity point Y-axis [0, 1]=>[Front-Rear]
        public const string PanPosCenterZ = "PanPosCenterZ";	// Gravity point Z-axis [0, 1]=>[Bottom-Top]
    }

    [ComImport, Guid(Interfaces.IParameterFunctionName), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IParameterFunctionName
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetParameterIDFromFunctionName(
            [MarshalAs(UnmanagedType.I4), In] Int32 unitID,
            [MarshalAs(UnmanagedType.LPStr), In] String functionName,
            [MarshalAs(UnmanagedType.I4), In, Out] ref UInt32 paramID);
    }
}
