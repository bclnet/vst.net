using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Steinberg.Vst3
{
    // Basic Bus object.
    public class Bus
    {
        protected string name;              // name
        protected BusType busType;          // kMain or kAux, see \ref BusTypes
        protected BusFlags flags;           // flags, see \ref BusInfo::BusFlags
        protected bool active;			    // activation state

        public Bus(string name, BusType busType, BusFlags flags)
        {
            this.name = name;
            this.busType = busType;
            this.flags = flags;
            active = false; // (flags & BusFlags.DefaultActive) != 0;
        }

        /// <summary>
        /// Gets or sets if bus is active.
        /// </summary>
        public bool Active
        {
            get => active;
            set => active = value;
        }

        /// <summary>
        /// Sets a new name for this bus.
        /// </summary>
        public string Name
        {
            //protected internal get => name;
            set => name = value;
        }

        /// <summary>
        /// Sets a new busType for this bus.
        /// </summary>
        public BusType BusType
        {
            //protected get => busType;
            set => busType = value;
        }

        /// <summary>
        /// Sets a new flags for this bus.
        /// </summary>
        /// <param name="newFlags"></param>
        public BusFlags Flags
        {
            //protected get => flags;
            set => flags = value;
        }

        /// <summary>
        /// Gets the BusInfo of this bus.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public virtual bool GetInfo(ref BusInfo info)
        {
            info.Name = name;
            info.BusType = busType;
            info.Flags = flags;
            return true;
        }
    }

    /// <summary>
    /// Description of an Event Bus.
    /// </summary>
    public class EventBus : Bus
    {
        protected int channelCount;

        public EventBus(string name, BusType busType, BusFlags flags, int channelCount)
            : base(name, busType, flags)
            => this.channelCount = channelCount;

        #region Bus

        /// <summary>
        /// Gets the BusInfo associated to this Event bus.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool GetInfo(ref BusInfo info)
        {
            info.ChannelCount = channelCount;
            return base.GetInfo(ref info);
        }

        #endregion
    }

    /// <summary>
    /// Description of an Audio Bus.
    /// </summary>
    public class AudioBus : Bus
    {
        protected SpeakerArrangement speakerArr;

        public AudioBus(string name, BusType busType, BusFlags flags, SpeakerArrangement speakerArr)
            : base(name, busType, flags)
            => this.speakerArr = speakerArr;

        // Gets or sets the speaker arrangement defining this Audio bus.
        public SpeakerArrangement Arrangement
        {
            get => speakerArr;
            set => speakerArr = value;
        }

        #region Bus

        /// <summary>
        /// Gets the BusInfo associated to this Audio bus.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool GetInfo(ref BusInfo info)
        {
            info.ChannelCount = speakerArr.GetChannelCount();
            return base.GetInfo(ref info);
        }

        #endregion
    }

    public class BusList : List<Bus>
    {
        protected MediaType type;
        protected BusDirection direction;

        public BusList(MediaType type, BusDirection dir)
        {
            this.type = type;
            direction = dir;
        }

        /// <summary>
        /// Returns the BusList Type.
        /// </summary>
        public MediaType Type => type;

        /// <summary>
        /// Returns the BusList direction.
        /// </summary>
        public BusDirection Direction => direction;
    }
}
