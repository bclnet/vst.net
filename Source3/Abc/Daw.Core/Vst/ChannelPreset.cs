using System;
using System.Linq;

namespace Daw.Vst
{
    [Serializable]
    public class ChannelPreset
    {   
        public const int NumberOfEffectPlugins = 8;
        public float Volume = 1.0f;
        public float Pan;               
        public bool KeyZoneActive;
        public int KeyZoneLower;
        public int KeyZoneUpper;
        public int Transpose;
        public ExpressionPedalFunction ExpressionPedalFunction = ExpressionPedalFunction.EffectControl;
        public bool ExpressionPedalInvert;
        public bool NoteDrop;
        public int NoteDropDelayIndex; 
        public MidiChannel MidiChannel = MidiChannel.Channel1;
        public bool SustainEnabled = true;
        public VstPreset InstrumentVstPreset = new();
        public VstPreset[] EffectVstPresets = Enumerable.Repeat(new VstPreset { State = PluginState.Empty }, NumberOfEffectPlugins).ToArray();
    }
}
