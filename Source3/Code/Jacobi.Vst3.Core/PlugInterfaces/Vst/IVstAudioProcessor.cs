using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    static partial class Constants
    {
        public const string kVstAudioEffectClass = "Audio Module Class";
    }

    public class PlugType : Collection<string>
    {
        public const string FxAnalyzer = "Fx|Analyzer";	        // Scope, FFT-Display,...
        public const string FxDelay = "Fx|Delay";		        // Delay, Multi-tap Delay, Ping-Pong Delay...
        public const string FxDistortion = "Fx|Distortion";	    // Amp Simulator, Sub-Harmonic, SoftClipper...
        public const string FxDynamics = "Fx|Dynamics";	        // Compressor, Expander, Gate, Limiter, Maximizer, Tape Simulator, EnvelopeShaper...
        public const string FxEQ = "Fx|EQ";			            // Equalization, Graphical EQ...
        public const string FxFilter = "Fx|Filter";		        // WahWah, ToneBooster, Specific Filter,...
        public const string Fx = "Fx";				            // others type (not categorized)
        public const string FxInstrument = "Fx|Instrument";	    // Fx which could be loaded as Instrument too
        public const string FxInstrumentExternal = "Fx|Instrument|External";	// Fx which could be loaded as Instrument too and is external (wrapped Hardware)
        public const string FxSpatial = "Fx|Spatial";		    // MonoToStereo, StereoEnhancer,...
        public const string FxGenerator = "Fx|Generator";	    // Tone Generator, Noise Generator...
        public const string FxMastering = "Fx|Mastering";	    // Dither, Noise Shaping,...
        public const string FxModulation = "Fx|Modulation";	    // Phaser, Flanger, Chorus, Tremolo, Vibrato, AutoPan, Rotary, Cloner...
        public const string FxPitchShift = "Fx|Pitch Shift";	// Pitch Processing, Pitch Correction,...
        public const string FxRestoration = "Fx|Restoration";	// Denoiser, Declicker,...
        public const string FxReverb = "Fx|Reverb";		        // Reverberation, Room Simulation, Convolution Reverb...
        public const string FxSurround = "Fx|Surround";	        // dedicated to surround processing: LFE Splitter, Bass Manager...
        public const string FxTools = "Fx|Tools";		        // Volume, Mixer, Tuner...

        public const string Instrument = "Instrument";			// Effect used as instrument (sound generator), not as insert
        public const string InstrumentDrum = "Instrument|Drum";	// Instrument for Drum sounds
        public const string InstrumentSampler = "Instrument|Sampler";	// Instrument based on Samples
        public const string InstrumentSynth = "Instrument|Synth";	// Instrument based on Synthesis
        public const string InstrumentSynthSampler = "Instrument|Synth|Sampler";	// Instrument based on Synthesis and Samples
        public const string InstrumentExternal = "Instrument|External";// External Instrument (wrapped Hardware)

        public const string Spatial = "Spatial";		        // used for SurroundPanner
        public const string SpatialFx = "Spatial|Fx";		    // used for SurroundPanner and as insert effect
        public const string OnlyRealTime = "OnlyRT";			// indicates that it supports only realtime process call, no processing faster than realtime
        public const string OnlyOfflineProcess = "OnlyOfflineProcess";	// used for offline processing Plug-in (will not work as normal insert Plug-in)
        public const string UpDownMix = "Up-Downmix";		    // used for Mixconverter/Up-Mixer/Down-Mixer
        public const string Analyzer = "Analyzer";	            // Meter, Scope, FFT-Display, not selectable as insert plugin

        public const string Mono = "Mono";			            // used for Mono only Plug-in [optional]
        public const string Stereo = "Stereo";			        // used for Stereo only Plug-in [optional]
        public const string Surround = "Surround";		        // used for Surround only Plug-in [optional]

        public PlugType() { }
        public PlugType(string parse)
        {
            var cats = parse.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cat in cats) Add(cat);
        }

        public override string ToString() => string.Join("|", this.ToArray());
    }

    /// <summary>
    /// Component Flags used as classFlags in PClassInfo2
    /// </summary>
    public enum ComponentFlags
    {
        //None = 0,
        Distributable = 1 << 0,	        // Component can be run on remote computer
        SimpleModeSupported = 1 << 1	// Component supports simple IO mode (or works in simple mode anyway) see \ref vst3IoMode
    }

    /// <summary>
    /// Symbolic sample size.
    /// </summary>
    public enum SymbolicSampleSizes : int
    {
        Sample32,       // 32-bit precision
        Sample64        // 64-bit precision
    }

    /// <summary>
    /// Processing mode informs the plug-in about the context and at which frequency the process call is called.
    /// VST3 defines 3 modes:
    /// - kRealtime:    each process call is called at a realtime frequency(defined by [numSamples of ProcessData] / samplerate).
    ///                 The plug-in should always try to process as fast as possible in order to let enough time slice to other plug-ins.
    /// - kPrefetch:    each process call could be called at a variable frequency(jitter, slower / faster than realtime),
    ///                 the plug-in should process at the same quality level than realtime, plug-in must not slow down to realtime (e.g.disk streaming)!
    ///                 The host should avoid to process in kPrefetch mode such sampler based plug-in.
    /// - kOffline:     each process call could be faster than realtime or slower, higher quality than realtime could be used.
    ///                 plug-ins using disk streaming should be sure that they have enough time in the process call for streaming,
    ///                 if needed by slowing down to realtime or slower.
    /// 
    /// Note about Process Modes switching:
    /// - Switching between kRealtime and kPrefetch process modes are done in realtime thread without need of calling
    ///   IAudioProcessor::setupProcessing, the plug-in should check in process call the member processMode of ProcessData
    ///   in order to know in which mode it is processed.
    /// - Switching between kRealtime (or kPrefetch) and kOffline requires that the host calls IAudioProcessor::setupProcessing
    ///   in order to inform the plug-in about this mode change.
    /// </summary>
    public enum ProcessModes : int
    {
        Realtime,       // realtime processing
        Prefetch,       // prefetch processing
        Offline         // offline processing
    }

    static partial class Constants
    {
        public const uint kNoTail = 0;                   // kNoTail to be returned by getTailSamples when no tail is wanted
        public const uint kInfiniteTail = uint.MaxValue; // kInfiniteTail to be returned by getTailSamples when infinite tail is wanted
    }

    /// <summary>
    /// Audio processing setup.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = Platform.StructurePack)]
    public struct ProcessSetup
    {
        public static readonly int Size = Marshal.SizeOf<ProcessSetup>();

        [MarshalAs(UnmanagedType.I4)] public ProcessModes ProcessMode;		// \ref ProcessModes
        [MarshalAs(UnmanagedType.I4)] public SymbolicSampleSizes SymbolicSampleSize; // \ref SymbolicSampleSizes
        [MarshalAs(UnmanagedType.I4)] public Int32 MaxSamplesPerBlock;	    // maximum number of samples per audio block
        [MarshalAs(UnmanagedType.R8)] public Double SampleRate;		        // sample rate
    }

    /// <summary>
    /// Processing buffers of an audio bus.
    /// This structure contains the processing buffer for each channel of an audio bus.
    /// - The number of channels (numChannels) must always match the current bus arrangement.
    ///   It could be set to value '0' when the host wants to flush the parameters (when the plug-in is not processed).
    /// - The size of the channel buffer array must always match the number of channels. So the host
    ///   must always supply an array for the channel buffers, regardless if the
    ///   bus is active or not. However, if an audio bus is currently inactive, the actual sample
    ///   buffer addresses are safe to be null.
    /// - The silence flag is set when every sample of the according buffer has the value '0'. It is
    ///   intended to be used as help for optimizations allowing a plug-in to reduce processing activities.
    ///   But even if this flag is set for a channel, the channel buffers must still point to valid memory!
    ///   This flag is optional. A host is free to support it or not.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct AudioBusBuffers
    {
        public static readonly int Size = FieldOffset_SizeOf;

        [FieldOffset(FieldOffset_NumChannels), MarshalAs(UnmanagedType.I4)] public Int32 NumChannels;		// number of audio channels in bus
        [FieldOffset(FieldOffset_SilenceFlags), MarshalAs(UnmanagedType.U8)] public UInt64 SilenceFlags;	// Bitset of silence state per channel

        // Single** pointer to an array Single[NumChannels][NumSamples]
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.SysInt)] public IntPtr ChannelBuffers32; // sample buffers to process with 32-bit precision
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.SysInt)] public unsafe Single** ChannelBuffers32X; // sample buffers to process with 32-bit precision (unsafe)

        // Double** pointer to an array Double[NumChannels][NumSamples]
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.SysInt)] public IntPtr ChannelBuffers64; // sample buffers to process with 64-bit precision
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.SysInt)] public unsafe Double** ChannelBuffers64X; // sample buffers to process with 64-bit precision (unsafe)

#if X86
        internal const int FieldOffset_NumChannels = 0;
        internal const int FieldOffset_SilenceFlags = 8;
        internal const int FieldOffset_Union = 16;
        internal const int FieldOffset_SizeOf = 24;
#endif
#if X64
        internal const int FieldOffset_NumChannels = 0;
        internal const int FieldOffset_SilenceFlags = 8;
        internal const int FieldOffset_Union = 16;
        internal const int FieldOffset_SizeOf = 24;
#endif
    }

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

    /// <summary>
    /// Audio processing interface: Vst::IAudioProcessor
    /// This interface must always be supported by audio processing plug-ins.
    /// </summary>
    [ComImport, Guid(Interfaces.IAudioProcessor), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioProcessor
    {
        /// <summary>
        /// Try to set (host => plug-in) a wanted arrangement for inputs and outputs.
        /// The host should always deliver the same number of input and output busses than the plug-in
        /// needs(see \ref IComponent::getBusCount). The plug-in has 3 possibilities to react on this
        /// setBusArrangements call:
        /// 1. The plug-in accepts these arrangements, then it should modify, if needed, its busses to match 
        ///    these new arrangements (later on asked by the host with IComponent::getBusInfo() or
        ///    IAudioProcessor::getBusArrangement()) and then should return kResultTrue.
        /// 2. The plug-in does not accept or support these requested arrangements for all
        ///    inputs/outputs or just for some or only one bus, but the plug-in can try to adapt its current
        ///    arrangements according to the requested ones (requested arrangements for kMain busses should be
        ///    handled with more priority than the ones for kAux busses), then it should modify its busses arrangements
        ///    and should return kResultFalse.
        /// 3. Same than the point 2 above the plug-in does not support these requested arrangements 
        ///    but the plug-in cannot find corresponding arrangements, the plug-in could keep its current arrangement
        ///    or fall back to a default arrangement by modifying its busses arrangements and should return kResultFalse.
        /// </summary>
        /// <param name="inputs">pointer to an array of /ref SpeakerArrangement</param>
        /// <param name="numIns">number of /ref SpeakerArrangement in inputs array</param>
        /// <param name="outputs">pointer to an array of /ref SpeakerArrangement</param>
        /// <param name="numOuts">number of /ref SpeakerArrangement in outputs array</param>
        /// <returns>kResultTrue when Arrangements is supported and is the current one, else returns kResultFalse.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetBusArrangements(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1), In] SpeakerArrangement[] inputs,
            [MarshalAs(UnmanagedType.I4), In] Int32 numIns,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3), In] SpeakerArrangement[] outputs,
            [MarshalAs(UnmanagedType.I4), In] Int32 numOuts);

        /// <summary>
        /// Gets the bus arrangement for a given direction (input/output) and index.
		/// Note: IComponent::getBusInfo () and IAudioProcessor::getBusArrangement () should be always return the same 
		/// information about the busses arrangements.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="index"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetBusArrangement(
            [MarshalAs(UnmanagedType.I4), In] BusDirections dir,
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.U8), Out] out SpeakerArrangement arr);

        /// <summary>
        /// Asks if a given sample size is supported see \ref SymbolicSampleSizes.
        /// </summary>
        /// <param name="symbolicSampleSize"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 CanProcessSampleSize(
            [MarshalAs(UnmanagedType.I4), In] SymbolicSampleSizes symbolicSampleSize);

        /// <summary>
        /// Gets the current Latency in samples.
		/// The returned value defines the group delay or the latency of the plug-in. For example, if the plug-in internally needs
		/// to look in advance (like compressors) 512 samples then this plug-in should report 512 as latency.
		/// If during the use of the plug-in this latency change, the plug-in has to inform the host by
		/// using IComponentHandler::restartComponent (kLatencyChanged), this could lead to audio playback interruption
		/// because the host has to recompute its internal mixer delay compensation.
		/// Note that for player live recording this latency should be zero or small.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.U4)]
        UInt32 GetLatencySamples();

        /// <summary>
        /// Called in disable state (setActive not called with true) before setProcessing is called and processing will begin.
        /// </summary>
        /// <param name="setup"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetupProcessing(
            [MarshalAs(UnmanagedType.Struct), In] ref ProcessSetup setup);

        /// <summary>
        /// Informs the plug-in about the processing state. This will be called before any process calls
        /// start with true and after with false.
        /// Note that setProcessing (false) may be called after setProcessing (true) without any process calls.
        /// Note this function could be called in the UI or in Processing Thread, thats why the plug-in
        /// should only light operation (no memory allocation or big setup reconfiguration), 
        /// this could be used to reset some buffers (like Delay line or Reverb).
        /// The host has to be sure that it is called only when the plug-in is enable (setActive (true)
        /// was called).
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetProcessing(
            [MarshalAs(UnmanagedType.U1), In] Boolean state);

        /// <summary>
        /// The Process call, where all information (parameter changes, event, audio buffer) are passed.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Process(
            [MarshalAs(UnmanagedType.Struct), In] ref ProcessData data);

        /// <summary>
        /// Gets tail size in samples. For example, if the plug-in is a Reverb plug-in and it knows that
        /// the maximum length of the Reverb is 2sec, then it has to return in getTailSamples() 
        /// (in VST2 it was getGetTailSize()): 2*sampleRate.
        /// This information could be used by host for offline processing, process optimization and 
        /// downmix(avoiding signal cut (clicks)).
        /// It should return:
        /// - kNoTail when no tail
        /// - x * sampleRate when x Sec tail.
        /// - kInfiniteTail when infinite tail.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.U4)]
        UInt32 GetTailSamples();
    }

    static partial class Interfaces
    {
        public const string IAudioProcessor = "42043F99-B7DA-453C-A569-E79D9AAEC33D";
    }

    /// <summary>
    /// Extended IAudioProcessor interface for a component: Vst::IAudioPresentationLatency
    /// Inform the plug-in about how long from the moment of generation/acquiring (from file or from Input)
    /// it will take for its input to arrive, and how long it will take for its output to be presented (to output or to speaker).
    /// 
    /// Note for Input Presentation Latency: when reading from file, the first plug-in will have an input presentation latency set to zero.
    /// When monitoring audio input from an audio device, the initial input latency is the input latency of the audio device itself.
    /// 
    /// Note for Output Presentation Latency: when writing to a file, the last plug-in will have an output presentation latency set to zero.
    /// When the output of this plug-in is connected to an audio device, the initial output latency is the output
    /// latency of the audio device itself.
    /// 
    /// A value of zero either means no latency or an unknown latency.
    /// 
    /// Each plug-in adding a latency (returning a none zero value for IAudioProcessor::getLatencySamples) will modify the input 
    /// presentation latency of the next plug-ins in the mixer routing graph and will modify the output presentation latency 
    /// of the previous plug-ins.
    /// </summary>
    [ComImport, Guid(Interfaces.IAudioPresentationLatency), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioPresentationLatency
    {
        /// <summary>
        /// Informs the plug-in about the Audio Presentation Latency in samples for a given direction (kInput/kOutput) and bus index.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="busIndex"></param>
        /// <param name="latencyInSamples"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetAudioPresentationLatencySamples(
            [MarshalAs(UnmanagedType.I4), In] BusDirections dir,
            [MarshalAs(UnmanagedType.I4), In] Int32 busIndex,
            [MarshalAs(UnmanagedType.U4), In] UInt32 latencyInSamples);
    }

    static partial class Interfaces
    {
        public const string IAudioPresentationLatency = "309ECE78-EB7D-4FAE-8B22-25D909FD08B6";
    }

    /// <summary>
    /// Extended IAudioProcessor interface for a component: Vst::IProcessContextRequirements
    /// To get accurate process context information (Vst::ProcessContext), it is now required to implement this interface and
    /// return the desired bit mask of flags which your audio effect needs. If you do not implement this
    /// interface, you may not get any information at all of the process function.
    /// 
    /// The host asks for this information once between initialize and setActive. It cannot be changed afterwards.
    /// 
    /// This gives the host the opportunity to better optimize the audio process graph when it knows which
    /// plug-ins need which information.
    /// 
    /// Plug-Ins built with an earlier SDK version (< 3.7) will still get the old information, but the information
    /// may not be as accurate as when using this interface.
    /// </summary>
    [ComImport, Guid(Interfaces.IProcessContextRequirements), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IProcessContextRequirements
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        UInt32 GetProcessContextRequirements();

        public enum Flags
        {
            NeedSystemTime = 1 << 0, // kSystemTimeValid
            NeedContinousTimeSamples = 1 << 1, // kContTimeValid
            NeedProjectTimeMusic = 1 << 2, // kProjectTimeMusicValid
            NeedBarPositionMusic = 1 << 3, // kBarPositionValid
            NeedCycleMusic = 1 << 4, // kCycleValid
            NeedSamplesToNextClock = 1 << 5, // kClockValid
            NeedTempo = 1 << 6, // kTempoValid
            NeedTimeSignature = 1 << 7, // kTimeSigValid
            NeedChord = 1 << 8, // kChordValid
            NeedFrameRate = 1 << 9, // kSmpteValid
            NeedTransportState = 1 << 10, // kPlaying, kCycleActive, kRecording
        }
    }

    static partial class Interfaces
    {
        public const string IProcessContextRequirements = "2A654303-EF76-4E3D-95B5-FE83730EF6D0";
    }
}
