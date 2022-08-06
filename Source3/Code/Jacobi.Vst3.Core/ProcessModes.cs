﻿namespace Jacobi.Vst3.Core
{
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
}
