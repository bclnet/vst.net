using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
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
}
