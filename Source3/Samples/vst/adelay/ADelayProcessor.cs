using Steinberg.Vst3;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Steinberg.Vst3.TResult;
using ParamValue = System.Double;

namespace Steinberg
{
    unsafe class ADelayProcessor : AudioEffect
    {
        protected ParamValue mDelay;
        protected float** mBuffer;
        protected int mBufferPos;
        protected bool mBypass;

        public ADelayProcessor()
        {
            mDelay = 1;
            mBuffer = null;
            mBufferPos = 0;
            mBypass = false;
            ControllerClass = typeof(IDelayTestController).GUID;
        }

        public override TResult Initialize(object context)
        {
            var result = base.Initialize(context);
            if (result == kResultTrue)
            {
                AddAudioInput("AudioInput", SpeakerArrangement.kStereo);
                AddAudioOutput("AudioOutput", SpeakerArrangement.kStereo);
            }
            return result;
        }
    }
}
