using System;

namespace Jacobi.Vst3.Core
{
    public static class Constants
    {
        public const int String128 = 128;

        // IAudioProcessor.GetTailSamples()
        public const uint kNoTail = 0;                   // kNoTail to be returned by getTailSamples when no tail is wanted
        public const uint kInfiniteTail = uint.MaxValue; // kInfiniteTail to be returned by getTailSamples when infinite tail is wanted

        public const string kVstAudioEffectClass = "Audio Module Class";
        public const string kVstComponentControllerClass = "Component Controller Class";
        public const string kTestClass = "Test Class";
        public const string kPluginCompatibilityClass = "Plugin Compatibility Class";

        public static readonly Version Vst3SdkVersion = new(3, 6, 14);
    }
}
