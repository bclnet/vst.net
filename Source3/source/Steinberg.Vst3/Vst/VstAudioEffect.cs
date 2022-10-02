using Steinberg.Vst3.Utility;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3
{
    public abstract class AudioEffect : Component, IAudioProcessor, IComponent, IProcessContextRequirements
    {
        protected ProcessSetup processSetup;
        protected ProcessContextRequirements processContextRequirements;

        public AudioEffect()
        {
            processSetup.MaxSamplesPerBlock = 1024;
            processSetup.ProcessMode = ProcessModes.Realtime;
            processSetup.SampleRate = 44100.0;
            processSetup.SymbolicSampleSize = SymbolicSampleSizes.Sample32;
        }

        // Creates and adds a new Audio input bus with a given speaker arrangement, busType (kMain or kAux).
        public AudioBus AddAudioInput(string name, SpeakerArrangement arr, BusType busType = BusType.Main, BusFlags flags = BusFlags.DefaultActive)
        {
            var newBus = new AudioBus(name, busType, flags, arr);
            audioInputs.Add(newBus);
            return newBus;
        }

        // Creates and adds a new Audio output bus with a given speaker arrangement, busType (kMain or kAux).
        public AudioBus AddAudioOutput(string name, SpeakerArrangement arr, BusType busType = BusType.Main, BusFlags flags = BusFlags.DefaultActive)
        {
            var newBus = new AudioBus(name, busType, flags, arr);
            audioOutputs.Add(newBus);
            return newBus;
        }

        // Retrieves an Audio Input Bus by index.
        public AudioBus GetAudioInput(int index)
        {
            AudioBus bus = null;
            if (index < audioInputs.Count)
                bus = (AudioBus)audioInputs[index];
            return bus;
        }

        // Retrieves an Audio Output Bus by index.
        public AudioBus GetAudioOutput(int index)
        {
            AudioBus bus = null;
            if (index < audioOutputs.Count)
                bus = (AudioBus)audioOutputs[index];
            return bus;
        }

        // Creates and adds a new Event input bus with a given speaker arrangement, busType (kMain or kAux).
        public EventBus AddEventInput(string name, int channels = 16, BusType busType = BusType.Main, BusFlags flags = BusFlags.DefaultActive)
        {
            var newBus = new EventBus(name, busType, flags, channels);
            eventInputs.Add(newBus);
            return newBus;
        }

        // Creates and adds a new Event output bus with a given speaker arrangement, busType (kMain or kAux).
        public EventBus AddEventOutput(string name, int channels = 16, BusType busType = BusType.Main, BusFlags flags = BusFlags.DefaultActive)
        {
            var newBus = new EventBus(name, busType, flags, channels);
            eventOutputs.Add(newBus);
            return newBus;
        }

        // Retrieves an Event Input Bus by index.
        public EventBus GetEventInput(int index)
        {
            EventBus bus = null;
            if (index < eventInputs.Count)
                bus = (EventBus)eventInputs[index];
            return bus;
        }

        // Retrieves an Event Output Bus by index.
        public EventBus GetEventOutput(int index)
        {
            EventBus bus = null;
            if (index < eventOutputs.Count)
                bus = (EventBus)eventOutputs[index];
            return bus;
        }

        #region IAudioProcessor

        public virtual TResult SetBusArrangements(SpeakerArrangement[] inputs, int numIns, SpeakerArrangement[] outputs, int numOuts)
        {
            if (numIns < 0 || numOuts < 0)
                return kInvalidArgument;

            if (numIns > audioInputs.Count ||
                numOuts > audioOutputs.Count)
                return kResultFalse;

            for (var index = 0; index < audioInputs.Count; ++index)
            {
                if (index >= numIns)
                    break;
                ((AudioBus)audioInputs[index]).Arrangement = inputs[index];
            }

            for (var index = 0; index < audioOutputs.Count; ++index)
            {
                if (index >= numOuts)
                    break;
                ((AudioBus)audioOutputs[index]).Arrangement = outputs[index];
            }

            return kResultOk;
        }

        public virtual TResult GetBusArrangement(BusDirection dir, int busIndex, out SpeakerArrangement arr)
        {
            arr = default;
            var busList = GetBusList(MediaType.Audio, dir);
            if (busList == null || busIndex < 0 || busList.Count <= busIndex)
                return kInvalidArgument;
            var audioBus = (AudioBus)busList[busIndex];
            if (audioBus != null)
            {
                arr = audioBus.Arrangement;
                return kResultTrue;
            }
            return kResultFalse;
        }

        public virtual uint GetLatencySamples()
            => 0;

        public virtual TResult SetupProcessing(ref ProcessSetup setup)
        {
            processSetup.MaxSamplesPerBlock = setup.MaxSamplesPerBlock;
            processSetup.ProcessMode = setup.ProcessMode;
            processSetup.SampleRate = setup.SampleRate;

            if (CanProcessSampleSize(setup.SymbolicSampleSize) != kResultTrue)
                return kResultFalse;

            processSetup.SymbolicSampleSize = setup.SymbolicSampleSize;

            return kResultOk;
        }

        public virtual TResult CanProcessSampleSize(SymbolicSampleSizes symbolicSampleSize)
            => symbolicSampleSize == SymbolicSampleSizes.Sample32 ? kResultTrue : kResultFalse;

        public virtual TResult SetProcessing(bool state)
            => kNotImplemented;

        public virtual TResult Process(ref ProcessData data)
            => kNotImplemented;

        public virtual uint GetTailSamples()
            => Constants.kNoTail;

        #endregion

        #region IProcessContextRequirements

        public uint GetProcessContextRequirements()
            => (uint)processContextRequirements.flags;

        #endregion
    }
}
