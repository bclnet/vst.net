using System;
using System.Collections.ObjectModel;

namespace Jacobi.Vst3
{
    public abstract class Bus
    {
        protected Bus(string name, BusTypes busType, BusInfo.BusFlags flags)
        {
            Name = name;
            BusType = busType;
            Flags = flags;

            IsActive = (flags & BusInfo.BusFlags.DefaultActive) != 0;
        }

        public MediaTypes MediaType { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public BusTypes BusType { get; set; }

        public BusInfo.BusFlags Flags { get; set; }

        public virtual bool GetInfo(ref BusInfo info)
        {
            info.MediaType = MediaType;
            info.BusType = BusType;
            info.Flags = Flags;
            info.Name = Name;

            return true;
        }
    }

    public class EventBus : Bus
    {
        public EventBus(string name, int channelCount)
            : this(name, channelCount, BusTypes.Main, BusInfo.BusFlags.DefaultActive) { }

        public EventBus(string name, int channelCount, BusTypes busType)
            : this(name, channelCount, busType, BusInfo.BusFlags.DefaultActive) { }

        public EventBus(string name, int channelCount, BusTypes busType, BusInfo.BusFlags flags)
            : base(name, busType, flags)
        {
            MediaType = MediaTypes.Event;
            ChannelCount = channelCount;
        }

        public int ChannelCount { get; private set; }

        public override bool GetInfo(ref BusInfo info)
        {
            info.ChannelCount = ChannelCount;
            return base.GetInfo(ref info);
        }
    }

    public class AudioBus : Bus
    {
        public AudioBus(string name, SpeakerArrangement speakerArr)
            : this(name, speakerArr, BusTypes.Main, BusInfo.BusFlags.DefaultActive) { }

        public AudioBus(string name, SpeakerArrangement speakerArr, BusTypes busType)
            : this(name, speakerArr, busType, BusInfo.BusFlags.DefaultActive) { }

        public AudioBus(string name, SpeakerArrangement speakerArr, BusTypes busType, BusInfo.BusFlags flags)
            : base(name, busType, flags)
        {
            MediaType = MediaTypes.Audio;
            SpeakerArrangement = speakerArr;
        }

        public SpeakerArrangement SpeakerArrangement { get; set; }

        public override bool GetInfo(ref BusInfo info)
        {
            // current definition of SpeakerArrangement is within 32 bits
            info.ChannelCount = (int)Platform.NumberOfSetBits((uint)SpeakerArrangement);
            return base.GetInfo(ref info);
        }
    }

    public class BusList : KeyedCollection<string, Bus>
    {
        public BusList(MediaTypes mediaType, BusDirections busDir)
        {
            MediaType = mediaType;
            BusDirection = busDir;
        }

        public MediaTypes MediaType { get; private set; }

        public BusDirections BusDirection { get; private set; }

        protected override string GetKeyForItem(Bus item)
        {
            if (item == null) return null;
            return item.Name;
        }

        protected override void InsertItem(int index, Bus item)
        {
            ThrowIfNotOfMediaType(item);
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Bus item)
        {
            ThrowIfNotOfMediaType(item);
            base.SetItem(index, item);
        }

        void ThrowIfNotOfMediaType(Bus item)
        {
            if (item != null && item.MediaType != MediaType) throw new ArgumentException("The MediaType for the item does not match the collection.", nameof(item));
        }
    }
}
