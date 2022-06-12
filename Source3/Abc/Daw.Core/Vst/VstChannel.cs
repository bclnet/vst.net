using Daw.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Daw.Vst
{
    public class ChannelPresetImportEventArgs : EventArgs
    {
        public ChannelPreset ChannelPreset { get; private set; }
        public ChannelPresetImportEventArgs(ChannelPreset preset) => ChannelPreset = preset;
    }

    public class VstChannel
    {
        public const int NumberOfEffectPlugins = 4; // todo: dynamic effect plugins array size

        public VstChannel(int asioBuffSize)
        {
            // Create big enough buffers to fit all plugin types
            var inSize = 32;
            var outSize = 64;
            if (asioBuffSize != 0)
            {
                if (InputBuffers == null || inSize != InputBuffers.Count)
                {
                    InputBuffers = null; // Dispose first if already existed!
                    InputBuffers = new AudioBufferInfo(inSize, asioBuffSize);
                }
                if (OutputBuffers == null || outSize != OutputBuffers.Count)
                {
                    OutputBuffers = null; // Dispose first if already existed!
                    OutputBuffers = new AudioBufferInfo(outSize, asioBuffSize);
                }
            }
            Instrument = new VstPluginInstrument(InputBuffers, OutputBuffers, asioBuffSize);
            Effects = Enumerable.Repeat(new VstPlugin(InputBuffers, OutputBuffers, asioBuffSize, true), NumberOfEffectPlugins).ToArray();
        }

        public AudioBufferInfo OutputBuffers { get; set; }
        public AudioBufferInfo InputBuffers { get; set; }

        public VstPluginInstrument Instrument { get; set; }
        public VstPlugin[] Effects { get; private set; }

        public event EventHandler<ChannelPresetImportEventArgs> PresetImported;

        public List<VstPlugin> AllPlugins
        {
            get
            {
                var r = new List<VstPlugin> { Instrument };
                r.AddRange(Effects);
                return r;
            }
        }

        public ChannelPreset ExportChannelPreset()
            => new()
            {
                Volume = Instrument.Volume,
                Pan = Instrument.Pan,
                KeyZoneActive = Instrument.KeyZoneActive,
                KeyZoneLower = Instrument.KeyZoneLower,
                KeyZoneUpper = Instrument.KeyZoneUpper,
                Transpose = Instrument.Transpose,
                ExpressionPedalFunction = Instrument.ExpressionPedalFunction,
                ExpressionPedalInvert = Instrument.ExpressionPedalInvert,
                NoteDrop = Instrument.NoteDrop,
                NoteDropDelayIndex = Instrument.NoteDropDelayIndex,
                MidiChannel = Instrument.MidiChannel,
                SustainEnabled = Instrument.SustainEnabled,
                InstrumentVstPreset = Instrument.VstPreset,
                EffectVstPresets = Effects.Select(x => x.VstPreset).ToArray(),
            };

        public void ImportChannelPreset(ChannelPreset preset)
        {
            Instrument.Volume = preset.Volume; // int 0...127 value to float 0.0...1.0
            Instrument.Pan = preset.Pan;
            Instrument.KeyZoneActive = preset.KeyZoneActive;
            Instrument.KeyZoneLower = preset.KeyZoneLower;
            Instrument.KeyZoneUpper = preset.KeyZoneUpper;
            Instrument.Transpose = preset.Transpose;
            Instrument.ExpressionPedalFunction = preset.ExpressionPedalFunction;
            Instrument.ExpressionPedalInvert = preset.ExpressionPedalInvert;
            Instrument.NoteDrop = preset.NoteDrop;
            Instrument.NoteDropDelayIndex = preset.NoteDropDelayIndex;
            Instrument.MidiChannel = preset.MidiChannel;
            Instrument.SustainEnabled = preset.SustainEnabled;
            Instrument.VstPreset = preset.InstrumentVstPreset;
            for (var i = 0; i < NumberOfEffectPlugins; i++) Effects[i].VstPreset = preset.EffectVstPresets[i];
            PresetImported?.Invoke(this, new ChannelPresetImportEventArgs(preset));
        }
    }
}
