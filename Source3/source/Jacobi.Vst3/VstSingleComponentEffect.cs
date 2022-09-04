using Steinberg.Vst3.Utility;
using System;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3
{
    /// <summary>
    /// Default implementation for a non-distributable Plug-in that combines
    /// processor and edit controller in one component.
    /// </summary>
    public abstract class SingleComponentEffect : EditControllerEx1, IComponent, IAudioProcessor, IProcessContextRequirements
    {
        ProcessSetup processSetup;
        ProcessContextRequirements processContextRequirements;

        protected BusList audioInputs = new(MediaType.Audio, BusDirection.Input);
        protected BusList audioOutputs = new(MediaType.Audio, BusDirection.Output);
        protected BusList eventInputs = new(MediaType.Event, BusDirection.Input);
        protected BusList eventOutputs = new(MediaType.Event, BusDirection.Output);

        public SingleComponentEffect()
        {
            processSetup.MaxSamplesPerBlock = 1024;
            processSetup.ProcessMode = ProcessModes.Realtime;
            processSetup.SampleRate = 44100.0;
            processSetup.SymbolicSampleSize = SymbolicSampleSizes.Sample32;
        }

        #region IPluginBase

        public override TResult Initialize(object context)
            => base.Initialize(context);

        public override TResult Terminate()
        {
            parameters.RemoveAll();
            RemoveAllBusses();

            return base.Terminate();
        }

        #endregion

        #region IComponent

        public virtual TResult GetControllerClassId(out Guid classId)
        {
            classId = default;
            return kNotImplemented;
        }

        public virtual TResult SetIoMode(IoMode mode)
            => kNotImplemented;

        public virtual int GetBusCount(MediaType type, BusDirection dir)
        {
            var busList = GetBusList(type, dir);
            return busList != null ? busList.Count : 0;
        }

        public virtual TResult GetBusInfo(MediaType type, BusDirection dir, int index, out BusInfo info)
        {
            info = default;
            var busList = GetBusList(type, dir);
            if (busList == null || index >= busList.Count)
                return kInvalidArgument;

            info.MediaType = type;
            info.Direction = dir;
            if (busList[index].GetInfo(ref info))
                return kResultTrue;
            return kResultFalse;
        }

        public virtual TResult GetRoutingInfo(ref RoutingInfo inInfo, out RoutingInfo outInfo)
        {
            outInfo = default;
            return kNotImplemented;
        }

        public virtual TResult ActivateBus(MediaType type, BusDirection dir, int index, bool state)
        {
            var busList = GetBusList(type, dir);
            if (busList == null || index >= busList.Count)
                return kInvalidArgument;

            var bus = busList[index];
            if (bus == null)
                return kResultFalse;

            bus.Active = state;
            return kResultTrue;
        }

        public virtual TResult SetActive(bool state)
            => kResultOk;

        public override TResult SetState(IBStream state)
            => kNotImplemented;

        public override TResult GetState(IBStream state)
            => kNotImplemented;

        #endregion

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

        /// <summary>
        /// Removes all Audio Busses.
        /// </summary>
        /// <returns></returns>
        protected TResult RemoveAudioBusses()
        {
            audioInputs.Clear();
            audioOutputs.Clear();

            return kResultOk;
        }

        public TResult RemoveEventBusses()
        {
            eventInputs.Clear();
            eventOutputs.Clear();

            return kResultOk;
        }

        protected TResult RemoveAllBusses()
        {
            RemoveAudioBusses();
            RemoveEventBusses();

            return kResultOk;
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
            if (CanProcessSampleSize(setup.SymbolicSampleSize) != kResultTrue)
                return kResultFalse;

            processSetup = setup;
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

        protected BusList GetBusList(MediaType type, BusDirection dir)
            => type switch
            {
                MediaType.Audio => dir == BusDirection.Input ? audioInputs : audioOutputs,
                MediaType.Event => dir == BusDirection.Input ? eventInputs : eventOutputs,
                _ => null,
            };
    }
}
