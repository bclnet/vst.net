using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
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
            [MarshalAs(UnmanagedType.U8), In, Out] ref SpeakerArrangement arr);

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
}
