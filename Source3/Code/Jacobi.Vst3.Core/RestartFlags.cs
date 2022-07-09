using System;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Flags used for IComponentHandler::restartComponent
    /// </summary>
    [Flags]
    public enum RestartFlags
    {
        //None = 0,
        ReloadComponent = 1 << 0,	        // The Component should be reloaded
        IoChanged = 1 << 1,	                // Input and/or Output Bus configuration has changed
        ParamValuesChanged = 1 << 2,	    // Multiple parameter values have changed (as result of a program change for example) 
        LatencyChanged = 1 << 3,	        // Latency has changed (IAudioProcessor.getLatencySamples)
        ParamTitlesChanged = 1 << 4,	    // Parameter titles, default values or flags (ParameterFlags) have changed [SDK 3.0.0]
        MidiCCAssignmentChanged = 1 << 5,	// MIDI Controllers and/or Program Changes Assignments have changed [SDK 3.0.1]
        NoteExpressionChanged = 1 << 6,	    // Note Expression has changed (info, count, PhysicalUIMapping, ...) [SDK 3.5.0]
        IoTitlesChanged = 1 << 7,	        // Input / Output bus titles have changed [SDK 3.5.0]
        IoPrefetchableSupportChanged = 1 << 8, // Prefetch support has changed [SDK 3.6.1]
        RoutingInfoChanged = 1 << 9,	    // RoutingInfo has changed [SDK 3.6.6]
        KeyswitchChanged = 1 << 10,	        // Key switches has changed (info, count) [SDK 3.7.3]
    }
}
