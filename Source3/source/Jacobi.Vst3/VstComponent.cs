using System;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3
{
    public abstract class Component : ComponentBase, IComponent
    {
        protected Guid controllerClass;
        protected BusList audioInputs = new(MediaType.Audio, BusDirection.Input);
        protected BusList audioOutputs = new(MediaType.Audio, BusDirection.Output);
        protected BusList eventInputs = new(MediaType.Event, BusDirection.Input);
        protected BusList eventOutputs = new(MediaType.Event, BusDirection.Output);

        #region ComponentBase

        public override TResult Initialize(object context)
            => base.Initialize(context);

        public override TResult Terminate()
        {
            // remove all busses
            RemoveAllBusses();

            return base.Terminate();
        }

        #endregion

        protected BusList GetBusList(MediaType type, BusDirection dir)
            => type switch
            {
                MediaType.Audio => dir == BusDirection.Input ? audioInputs : audioOutputs,
                MediaType.Event => dir == BusDirection.Input ? eventInputs : eventOutputs,
                _ => null,
            };

        // Sets the controller Class ID associated to its component.
        public Guid ControlledClass
        {
            //protected get => controllerClass;
            set => controllerClass = value;
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

        #region IComponent

        public virtual TResult GetControllerClassId(out Guid classID)
        {
            if (controllerClass == Guid.Empty)
            {
                classID = controllerClass;
                return kResultTrue;
            }
            classID = default;
            return kResultFalse;
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
            if (index < 0)
                return kInvalidArgument;
            var busList = GetBusList(type, dir);
            if (busList == null)
                return kInvalidArgument;
            if (index >= busList.Count)
                return kInvalidArgument;

            var bus = busList[index];
            info.MediaType = type;
            info.Direction = dir;
            if (bus.GetInfo(ref info))
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
            if (index < 0)
                return kInvalidArgument;
            var busList = GetBusList(type, dir);
            if (busList == null)
                return kInvalidArgument;
            if (index >= busList.Count)
                return kInvalidArgument;

            var bus = busList[index];
            bus.Active = state;
            return kResultTrue;
        }

        public virtual TResult SetActive(bool state)
            => kResultOk;

        public virtual TResult SetState(IBStream state)
            => kNotImplemented;

        public virtual TResult GetState(IBStream state)
            => kNotImplemented;

        #endregion

        /// <summary>
        /// Renames a specific bus. Do not forget to inform the host about this (see \ref IComponentHandler::restartComponent (kIoTitlesChanged)).
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dir"></param>
        /// <param name="index"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public TResult RenameBus(MediaType type, BusDirection dir, int index, string newName)
        {
            if (index < 0)
                return kInvalidArgument;
            var busList = GetBusList(type, dir);
            if (busList == null)
                return kInvalidArgument;
            if (index >= busList.Count)
                return kInvalidArgument;

            var bus = busList[index];
            bus.Name = newName;
            return kResultTrue;
        }

        // Gets the channel index of a given speaker in a arrangement, returns kResultFalse if speaker not
        // part of the arrangement else returns kResultTrue.
        public static TResult GetSpeakerChannelIndex(SpeakerArrangement arrangement, Speaker speaker, out int channel)
        {
            channel = SpeakerArrExtensions.GetSpeakerIndex(speaker, arrangement);
            return channel < 0 ? kResultFalse : kResultTrue;
        }
    }
}
