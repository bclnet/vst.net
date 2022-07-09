using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Controller Parameter Info.
    /// A parameter info describes a parameter of the controller.
    /// The id must always be the same for a parameter as this uniquely identifies the parameter.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = Platform.StructurePack)]
    public struct ParameterInfo
    {
        public static readonly int Size = Marshal.SizeOf<ParameterInfo>();

        public const UInt32 NoParamId = UInt32.MaxValue;

        [MarshalAs(UnmanagedType.U4)] public UInt32 Id;                     // unique identifier of this parameter (named tag too)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.String128)] public String Title;       // parameter title (e.g. "Volume")
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.String128)] public String ShortTitle;  // parameter shortTitle (e.g. "Vol")
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.String128)] public String Units;       // parameter unit (e.g. "dB")
        [MarshalAs(UnmanagedType.I4)] public Int32 StepCount;               // number of discrete steps (0: continuous, 1: toggle, discrete value otherwise 
                                                                            // (corresponding to max - min, for example: 127 for a min = 0 and a max = 127) - see \ref vst3ParameterIntro)
        [MarshalAs(UnmanagedType.R8)] public Double DefaultNormalizedValue; // default normalized value [0,1] (in case of discrete value: defaultNormalizedValue = defDiscreteValue / stepCount)
        [MarshalAs(UnmanagedType.I4)] public Int32 UnitId;                  // id of unit this parameter belongs to (see \ref vst3Units)
        [MarshalAs(UnmanagedType.I4)] public ParameterFlags Flags;          // ParameterFlags (see below)

        [Flags]
        public enum ParameterFlags
        {
            None = 0,                   // no flags wanted
            CanAutomate = 1 << 0,       // parameter can be automated
            IsReadOnly = 1 << 1,        // parameter cannot be changed from outside the plug-in (implies that kCanAutomate is NOT set)
            IsWrapAround = 1 << 2,      // attempts to set the parameter value out of the limits will result in a wrap around [SDK 3.0.2]
            IsList = 1 << 3,            // parameter should be displayed as list in generic editor or automation editing [SDK 3.1.0]
            IsHidden = 1 << 4,          // parameter should be NOT displayed and cannot be changed from outside the plug-in 
                                        // (implies that kCanAutomate is NOT set and kIsReadOnly is set) [SDK 3.7.0]

            IsProgramChange = 1 << 15,  // parameter is a program change (unitId gives info about associated unit - see \ref vst3ProgramLists)
            IsBypass = 1 << 16          // special bypass parameter (only one allowed): plug-in can handle bypass
                                        // (highly recommended to export a bypass parameter for effect plug-in)
        }
    }
}
