using Steinberg.Vst3;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static Steinberg.Vst3.TResult;

namespace TestPlugin
{
    [DisplayName("My Plugin Component"), Guid("599B4AD4-932E-4B35-B8A7-E01508FD1AAB"), ClassInterface(ClassInterfaceType.None)]
    unsafe class PluginComponent : AudioEffect
    {
        public PluginComponent()
        {
            //ControlledClass = typeof(MyEditController).GUID;

            audioInputs.Add(new AudioBus("Main Input", BusType.Main, 0, SpeakerArrangement.kStereo));
            audioOutputs.Add(new AudioBus("Main Output", BusType.Main, 0, SpeakerArrangement.kStereo));
            eventInputs.Add(new EventBus("Input Events", BusType.Main, 0, 1));
            eventOutputs.Add(new EventBus("Output Events", BusType.Main, 0, 1));
        }

        //public override TResult CanProcessSampleSize(SymbolicSampleSizes symbolicSampleSize)
        //    => symbolicSampleSize == SymbolicSampleSizes.Sample32 ? kResultTrue : kResultFalse;

        public override TResult Process(ref ProcessData data)
        {
            //Trace.WriteLine($"IAudioProcessor.Process: numSamples={data.NumSamples}");
            var result = ProcessInParameters(ref data);
            if (result.Failed())
                return result;

            result = ProcessAudio(ref data);
            if (result.Failed())
                return result;

            return ProcessEvents(ref data);
        }

        TResult ProcessInParameters(ref ProcessData data)
        {
            var paramChanges = data.InputParameterChanges;
            if (paramChanges != null)
            {
                var paramCount = paramChanges.GetParameterCount();
                if (paramCount > 0)
                    for (var i = 0; i < paramCount; i++)
                    {
                        var paramValue = paramChanges.GetParameterData(i);

                        for (var p = 0; p < paramValue.GetPointCount(); p++)
                            paramValue.GetPoint(p, out var sampleOffset, out var val);
                    }
            }

            return kResultOk;
        }

        TResult ProcessAudio(ref ProcessData data)
        {
            // flushing parameters
            if (data.NumInputs == 0 || data.NumOutputs == 0 || data.NumSamples == 0)
                return kResultOk;

            if (data.NumInputs != audioInputs.Count || data.NumOutputs != audioOutputs.Count)
                return kNotInitialized;

            var inputBusInfo = audioInputs[0];
            var outputBusInfo = audioOutputs[0];

            if (!inputBusInfo.Active || !outputBusInfo.Active)
                return kResultFalse;

            // hard-coded on one stereo input and one stereo output bus (see ctor)
            //var inputBus = new AudioBusAccessor(ref data, BusDirection.Input, 0);
            //var outputBus = new AudioBusAccessor(ref data, BusDirection.Output, 0);

            //for (var c = 0; c < inputBus.ChannelCount; c++)
            //{
            //    var input = inputBus.GetBuffer32(c);
            //    var output = outputBus.GetBuffer32(c);
            //    outputBus.SetChannelSilent(0, input == null);

            //    if (input != null && output != null)
            //        for (var i = 0; i < outputBus.SampleCount; i++)
            //            output[i] = input[i];
            //}

            return kResultOk;
        }

        TResult ProcessEvents(ref ProcessData data)
        {
            var inputs = data.InputEvents;
            var outputs = data.OutputEvents;

            if (inputs != null && outputs != null)
            {
                var count = inputs.GetEventCount();
                for (var i = 0; i < count; i++)
                {
                    inputs.GetEvent(i, out var evnt);
                    outputs.AddEvent(evnt);
                }
            }

            return kResultOk;
        }

        //protected virtual BusList GetBusList(MediaType type, BusDirection dir)
        //    => type switch
        //    {
        //        MediaType.Audio => dir == BusDirection.Input ? audioInputs : audioOutputs,
        //        MediaType.Event => dir == BusDirection.Input ? eventInputs : eventOutputs,
        //        _ => null
        //    };
    }
}
