using Daw.Core;
using System.Linq;

namespace Daw.Vst
{
    public class VstPluginInstrument : VstPlugin
    {
        public VstPluginInstrument(AudioBufferInfo parentInBuffers, AudioBufferInfo parentOutBuffers, int asioBuffSize)
            : base(parentInBuffers, parentOutBuffers, asioBuffSize, false) { }

        public int Transpose { get; set; }
        public float Volume { get; set; } // Range 0.0 ... 1.0 for fast mixing!
        public float Pan { get; set; }

        public bool KeyZoneActive { get; set; }
        public int KeyZoneLower { get; set; }
        public int KeyZoneUpper { get; set; }

        public Note[] Notes = Enumerable.Repeat(new Note(), 256).ToArray();
        public bool NoteDrop { get; set; }
        public int NoteDropDelay { get; set; }

        int _notedropDelayIndex;
        public int NoteDropDelayIndex
        {
            get => _notedropDelayIndex;
            set
            {
                _notedropDelayIndex = value;
                // todo: fix with databind
                var bpm = 120;
                var buffsize = 128;
                var tmp = (44100 / buffsize) * (60.0 / bpm);
                switch (value)
                {
                    case 0: NoteDropDelay = (int)(tmp / 8); break; // "1/8th":
                    case 1: NoteDropDelay = (int)(tmp / 4); break; //"1/4th":
                    case 2: NoteDropDelay = (int)(tmp / 2); break; //"1/2th":
                    case 3: NoteDropDelay = (int)(tmp); break; //"1":
                    case 4: NoteDropDelay = (int)(tmp * 2); break; // "2":
                    case 5: NoteDropDelay = (int)(tmp * 4); break;// "4":
                    default: break;
                }
            }
        }

        public bool SustainEnabled { get; set; }

        public ExpressionPedalFunction ExpressionPedalFunction { get; set; }
        public bool ExpressionPedalInvert { get; set; }

        public override void Deactivate()
        {
            base.Deactivate();
            foreach (var note in Notes) note.Pressed = false;
        }
    }
}
