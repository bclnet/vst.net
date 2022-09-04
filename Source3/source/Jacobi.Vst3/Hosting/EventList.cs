using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.Hosting
{
    /// <summary>
    /// Example implementation of IEventList.
    /// </summary>
    public class EventList : IEventList
    {
        protected Event[] events;
        protected int maxSize;
        protected int fillCount;

        public EventList(int maxSize = 50)
            => SetMaxSize(maxSize);

        public int GetEventCount()
            => fillCount;

        public void SetMaxSize(int newMaxSize)
        {
            if (events != null)
            {
                events = null;
                fillCount = 0;
            }
            if (newMaxSize > 0)
                events = new Event[newMaxSize];
            maxSize = newMaxSize;
        }

        public TResult GetEvent(int index, out Event e)
        {
            var evnt = GetEventByIndex(index);
            if (evnt != null)
            {
                e = evnt.Value;
                return kResultTrue;
            }
            e = default;
            return kResultFalse;
        }

        public TResult AddEvent(ref Event e)
        {
            if (maxSize > fillCount)
            {
                events[fillCount] = e;
                fillCount++;
                return kResultTrue;
            }
            return kResultFalse;
        }

        public void Clear()
            => fillCount = 0;

        public Event? GetEventByIndex(int index)
            => index < fillCount ? events[index] : null;
    }
}
