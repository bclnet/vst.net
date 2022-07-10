﻿using Jacobi.Vst3.Core;
using Jacobi.Vst3.Plugin;
using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.TestPlugin
{
    [System.ComponentModel.DisplayName("My Plugin Component")]
    [Guid("599B4AD4-932E-4B35-B8A7-E01508FD1AAB")]
    [ClassInterface(ClassInterfaceType.None)]
    class PluginComponent : AudioEffect
    {
        private readonly BusCollection _audioInputs = new BusCollection(MediaTypes.Audio, BusDirections.Input);
        private readonly BusCollection _audioOutputs = new BusCollection(MediaTypes.Audio, BusDirections.Output);
        private readonly BusCollection _eventInputs = new BusCollection(MediaTypes.Event, BusDirections.Input);
        private readonly BusCollection _eventOutputs = new BusCollection(MediaTypes.Event, BusDirections.Output);

        public PluginComponent()
        {
            ControlledClassId = typeof(MyEditController).GUID;

            _audioInputs.Add(new AudioBus("Main Input", SpeakerArrangement.kStereo));
            _audioOutputs.Add(new AudioBus("Main Output", SpeakerArrangement.kStereo));
            _eventInputs.Add(new EventBus("Input Events", 1));
            _eventOutputs.Add(new EventBus("Output Events", 1));
        }

        public override int CanProcessSampleSize(SymbolicSampleSizes symbolicSampleSize)
        {
            System.Diagnostics.Trace.WriteLine("IAudioProcessor.CanProcessSampleSize(" + symbolicSampleSize + ")");

            return symbolicSampleSize == SymbolicSampleSizes.Sample32 ? TResult.S_True : TResult.S_False;
        }

        public override int Process(ref ProcessData data)
        {
            //System.Diagnostics.Trace.WriteLine("IAudioProcessor.Process: numSamples=" + data.NumSamples);

            int result = ProcessInParameters(ref data);
            if (TResult.Failed(result))
            {
                return result;
            }

            result = ProcessAudio(ref data);
            if (TResult.Failed(result))
            {
                return result;
            }

            return ProcessEvents(ref data);
        }

        private int ProcessInParameters(ref ProcessData data)
        {

            var paramChanges = data.GetInputParameterChanges();
            if (paramChanges != null)
            {
                var paramCount = paramChanges.GetParameterCount();

                if (paramCount > 0)
                {
                    System.Diagnostics.Trace.WriteLine("IAudioProcessor.Process: InputParameterChanges.GetParameterCount() = " + paramCount);

                    for (int i = 0; i < paramCount; i++)
                    {
                        var paramValue = paramChanges.GetParameterValue(i);

                        for (int p = 0; p < paramValue.GetPointCount(); p++)
                        {
                            int sampleOffset = 0;
                            double val = 0;

                            paramValue.GetPoint(p, ref sampleOffset, ref val);
                        }
                    }
                }
            }

            return TResult.S_OK;
        }

        private int ProcessAudio(ref ProcessData data)
        {
            // flushing parameters
            if (data.NumInputs == 0 || data.NumOutputs == 0 || data.NumSamples == 0)
            {
                return TResult.S_OK;
            }

            if (data.NumInputs != _audioInputs.Count || data.NumOutputs != _audioOutputs.Count)
            {
                return TResult.E_Unexpected;
            }

            var inputBusInfo = _audioInputs[0];
            var outputBusInfo = _audioOutputs[0];

            if (!inputBusInfo.IsActive || !outputBusInfo.IsActive)
            {
                return TResult.S_False;
            }

            // hard-coded on one stereo input and one stereo output bus (see ctor)
            var inputBus = new AudioBusAccessor(ref data, BusDirections.Input, 0);
            var outputBus = new AudioBusAccessor(ref data, BusDirections.Output, 0);

            unsafe
            {
                for (int c = 0; c < inputBus.ChannelCount; c++)
                {
                    var input = inputBus.GetUnsafeBuffer32(c);
                    var output = outputBus.GetUnsafeBuffer32(c);
                    outputBus.SetChannelSilent(0, input == null);

                    if (input != null && output != null)
                    {
                        for (int i = 0; i < outputBus.SampleCount; i++)
                        {
                            output[i] = input[i];
                        }
                    }
                }
            }

            return TResult.S_OK;
        }

        private int ProcessEvents(ref ProcessData data)
        {
            var inputs = data.GetInputEvents();
            var outputs = data.GetOutputEvents();

            if (inputs != null && outputs != null)
            {
                var count = inputs.GetEventCount();
                for (var i = 0; i < count; i++)
                {
                    inputs.GetEvent(i, out var evnt);
                    outputs.AddEvent(evnt);
                }
            }

            return TResult.S_OK;
        }

        protected override BusCollection GetBusCollection(MediaTypes mediaType, BusDirections busDir)
        {
            return mediaType switch
            {
                MediaTypes.Audio => busDir == BusDirections.Input ? _audioInputs : _audioOutputs,
                MediaTypes.Event => busDir == BusDirections.Input ? _eventInputs : _eventOutputs,
                _ => null
            };
        }
    }
}
