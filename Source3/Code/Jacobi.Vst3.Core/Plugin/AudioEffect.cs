using Jacobi.Vst3.Core;
using System.Diagnostics;
using static Jacobi.Vst3.Core.TResult;

namespace Jacobi.Vst3.Plugin
{
    public abstract class AudioEffect : Component, IAudioProcessor, IComponent
    {
        public bool IsProcessing { get; set; }

        public ProcessModes ProcessMode { get; set; }

        public SymbolicSampleSizes SampleSize { get; set; }

        public int MaxSamplesPerBlock { get; set; }

        public double SampleRate { get; set; }

        #region IAudioProcessor Members

        public virtual TResult SetBusArrangements(SpeakerArrangement[] inputs, int numIns, SpeakerArrangement[] outputs, int numOuts)
        {
            Trace.WriteLine("IAudioProcessor.SetBusArrangements");

            var index = 0;
            var busses = GetBusCollection(MediaTypes.Audio, BusDirections.Input);

            if (busses != null)
                foreach (AudioBus bus in busses)
                {
                    if (index < numIns) bus.SpeakerArrangement = inputs[index];
                    index++;
                }

            busses = GetBusCollection(MediaTypes.Audio, BusDirections.Output);

            if (busses != null)
            {
                index = 0;
                foreach (AudioBus bus in busses)
                {
                    if (index < numOuts) bus.SpeakerArrangement = outputs[index];
                    index++;
                }
            }

            return kResultOk;
        }

        public virtual TResult GetBusArrangement(BusDirections dir, int index, out SpeakerArrangement arr)
        {
            Trace.WriteLine($"IAudioProcessor.GetBusArrangement({dir}, {index})");

            arr = default;
            var busses = GetBusCollection(MediaTypes.Audio, dir);
            if (busses == null) return kNotImplemented;
            if (index < 0 || index > busses.Count) return kInvalidArgument;

            arr = ((AudioBus)busses[index]).SpeakerArrangement;

            return kResultOk;
        }

        public abstract TResult CanProcessSampleSize(SymbolicSampleSizes symbolicSampleSize);

        public virtual uint GetLatencySamples()
        {
            Trace.WriteLine("IAudioProcessor.CanProcessSampleSize");

            return 0;
        }

        public virtual TResult SetupProcessing(ref ProcessSetup setup)
        {
            Trace.WriteLine("IAudioProcessor.SetupProcessing");

            if (IsActive) return kNotInitialized;
            if (!CanProcessSampleSize(setup.SymbolicSampleSize).IsTrue()) return kResultFalse;

            MaxSamplesPerBlock = setup.MaxSamplesPerBlock;
            ProcessMode = setup.ProcessMode;
            SampleRate = setup.SampleRate;
            SampleSize = setup.SymbolicSampleSize;

            return kResultTrue;
        }

        public virtual TResult SetProcessing(bool state)
        {
            Trace.WriteLine($"IAudioProcessor.SetProcessing({state})");

            if (!IsActive) return kNotInitialized;

            IsProcessing = state;

            return kResultOk;
        }

        public abstract TResult Process(ref ProcessData data);

        public uint GetTailSamples()
        {
            Trace.WriteLine("IAudioProcessor.GetTailSamples");

            return Constants.kNoTail;
        }

        #endregion
    }
}
