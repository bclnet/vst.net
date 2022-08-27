using Jacobi.Vst3;
using Jacobi.Vst3.Plugin;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Jacobi.Vst3.TResult;

namespace Jacobi.Vst3.TestPlugin
{
    [DisplayName("My Plugin Component"), Guid("599B4AD4-932E-4B35-B8A7-E01508FD1AAB"), ClassInterface(ClassInterfaceType.None)]
    unsafe class ADelayIds : AudioEffect
    {
        readonly BusList _audioInputs = new(MediaTypes.Audio, BusDirections.Input);
        readonly BusList _audioOutputs = new(MediaTypes.Audio, BusDirections.Output);
        readonly BusList _eventInputs = new(MediaTypes.Event, BusDirections.Input);
        readonly BusList _eventOutputs = new(MediaTypes.Event, BusDirections.Output);

        public ADelayIds()
        {
            ControlledClassId = typeof(MyEditController).GUID;

            _audioInputs.Add(new AudioBus("Main Input", SpeakerArrangement.kStereo));
            _audioOutputs.Add(new AudioBus("Main Output", SpeakerArrangement.kStereo));
            _eventInputs.Add(new EventBus("Input Events", 1));
            _eventOutputs.Add(new EventBus("Output Events", 1));
        }

        public override TResult CanProcessSampleSize(SymbolicSampleSizes symbolicSampleSize)
        {
            Trace.WriteLine($"IAudioProcessor.CanProcessSampleSize({symbolicSampleSize})");

            return symbolicSampleSize == SymbolicSampleSizes.Sample32 ? kResultTrue : kResultFalse;
        }

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
                {
                    Trace.WriteLine($"IAudioProcessor.Process: InputParameterChanges.GetParameterCount() = {paramCount}");

                    for (var i = 0; i < paramCount; i++)
                    {
                        var paramValue = paramChanges.GetParameterData(i);

                        for (var p = 0; p < paramValue.GetPointCount(); p++)
                        {
                            int sampleOffset = 0;
                            double val = 0;

                            paramValue.GetPoint(p, ref sampleOffset, ref val);
                        }
                    }
                }
            }

            return kResultOk;
        }

        TResult ProcessAudio(ref ProcessData data)
        {
            // flushing parameters
            if (data.NumInputs == 0 || data.NumOutputs == 0 || data.NumSamples == 0)
                return kResultOk;

            if (data.NumInputs != _audioInputs.Count || data.NumOutputs != _audioOutputs.Count)
                return kNotInitialized;

            var inputBusInfo = _audioInputs[0];
            var outputBusInfo = _audioOutputs[0];

            if (!inputBusInfo.IsActive || !outputBusInfo.IsActive)
                return kResultFalse;

            // hard-coded on one stereo input and one stereo output bus (see ctor)
            var inputBus = new AudioBusAccessor(ref data, BusDirections.Input, 0);
            var outputBus = new AudioBusAccessor(ref data, BusDirections.Output, 0);

            for (var c = 0; c < inputBus.ChannelCount; c++)
            {
                var input = inputBus.GetBuffer32(c);
                var output = outputBus.GetBuffer32(c);
                outputBus.SetChannelSilent(0, input == null);

                if (input != null && output != null)
                    for (var i = 0; i < outputBus.SampleCount; i++)
                        output[i] = input[i];
            }

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

        protected override BusList GetBusCollection(MediaTypes mediaType, BusDirections busDir)
            => mediaType switch
            {
                MediaTypes.Audio => busDir == BusDirections.Input ? _audioInputs : _audioOutputs,
                MediaTypes.Event => busDir == BusDirections.Input ? _eventInputs : _eventOutputs,
                _ => null
            };
    }
}
