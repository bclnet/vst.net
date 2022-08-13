using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Any data needed in audio processing.
	/// The host prepares AudioBusBuffers for each input/output bus,
    /// regardless of the bus activation state.Bus buffer indices always match
    /// with bus indices used in IComponent::getBusInfo of media type kAudio.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = Platform.StructurePack)]
    public struct ProcessData
    {
        public static readonly int Size = FieldOffset_SizeOf;

        [FieldOffset(FieldOffset_ProcessMode), MarshalAs(UnmanagedType.I4)] public ProcessModes ProcessMode;			                        // processing mode - value of \ref ProcessModes
        [FieldOffset(FieldOffset_SymbolicSampleSize), MarshalAs(UnmanagedType.I4)] public SymbolicSampleSizes SymbolicSampleSize;               // sample size - value of \ref SymbolicSampleSizes
        [FieldOffset(FieldOffset_NumSamples), MarshalAs(UnmanagedType.I4)] public Int32 NumSamples;			                                    // number of samples to process
        [FieldOffset(FieldOffset_NumInputs), MarshalAs(UnmanagedType.I4)] public Int32 NumInputs;			                                    // number of audio input buses
        [FieldOffset(FieldOffset_NumOutputs), MarshalAs(UnmanagedType.I4)] public Int32 NumOutputs;			                                    // number of audio output buses

        // AudioBusBuffers Inputs[NumBuses]
        [FieldOffset(FieldOffset_Inputs), MarshalAs(UnmanagedType.SysInt)] public IntPtr Inputs;	                                            // buffers of input buses
        [FieldOffset(FieldOffset_Inputs)] public unsafe AudioBusBuffers* InputsX;                                                               // buffers of input buses (unsafe)

        // AudioBusBuffers Outputs[NumBuses]
        [FieldOffset(FieldOffset_Outputs), MarshalAs(UnmanagedType.SysInt)] public IntPtr Outputs;	                                            // buffers of output buses
        [FieldOffset(FieldOffset_Outputs)] public unsafe AudioBusBuffers* OutputsX;                                                             // buffers of output buses (unsafe)

        [FieldOffset(FieldOffset_InputParameterChanges), MarshalAs(UnmanagedType.SysInt)] public IntPtr InputParameterChanges;                  // incoming parameter changes for this block 
        //[FieldOffset(FieldOffset_InputParameterChanges), MarshalAs(UnmanagedType.Interface)] public IParameterChanges InputParameterChangesX;	// incoming parameter changes for this block (unsafe)

        [FieldOffset(FieldOffset_OutputParameterChanges), MarshalAs(UnmanagedType.SysInt)] public IntPtr OutputParameterChanges;                // outgoing parameter changes for this block (optional)
        //[FieldOffset(FieldOffset_OutputParameterChanges), MarshalAs(UnmanagedType.Interface)] public IParameterChanges OutputParameterChangesX; // outgoing parameter changes for this block (unsafe, optional)

        [FieldOffset(FieldOffset_InputEvents), MarshalAs(UnmanagedType.SysInt)] public IntPtr InputEvents;                                      // incoming events for this block (optional)
        //[FieldOffset(FieldOffset_InputEvents), MarshalAs(UnmanagedType.Interface)] public IEventList InputEventsX;				            // incoming events for this block (unsafe, optional)

        [FieldOffset(FieldOffset_OutputEvents), MarshalAs(UnmanagedType.SysInt)] public IntPtr OutputEvents;                                    // outgoing events for this block (optional)
        //[FieldOffset(FieldOffset_OutputEvents), MarshalAs(UnmanagedType.Interface)] public IEventList OutputEventsX;				            // outgoing events for this block (unsafe, optional)

        // ProcessContext pointer
        [FieldOffset(FieldOffset_ProcessContext), MarshalAs(UnmanagedType.SysInt)] public IntPtr ProcessContext;                                // processing context (optional, but most welcome)

        internal const int FieldOffset_ProcessMode = 0;
        internal const int FieldOffset_SymbolicSampleSize = 4;
        internal const int FieldOffset_NumSamples = 8;
        internal const int FieldOffset_NumInputs = 12;
        internal const int FieldOffset_NumOutputs = 16;
#if X86
        internal const int FieldOffset_Inputs = 20;
        internal const int FieldOffset_Outputs = 24;
        internal const int FieldOffset_InputParameterChanges = 28;
        internal const int FieldOffset_OutputParameterChanges = 32;
        internal const int FieldOffset_InputEvents = 36;
        internal const int FieldOffset_OutputEvents = 40;
        internal const int FieldOffset_ProcessContext = 44;
        internal const int FieldOffset_SizeOf = 48;
#endif
#if X64
        internal const int FieldOffset_Inputs = 24;
        internal const int FieldOffset_Outputs = 32;
        internal const int FieldOffset_InputParameterChanges = 40;
        internal const int FieldOffset_OutputParameterChanges = 48;
        internal const int FieldOffset_InputEvents = 56;
        internal const int FieldOffset_OutputEvents = 64;
        internal const int FieldOffset_ProcessContext = 72;
        internal const int FieldOffset_SizeOf = 80;
#endif
    }
}
