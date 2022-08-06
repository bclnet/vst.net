using Jacobi.Vst3.Core;

namespace Jacobi.Vst3.Host
{
    /// <summary>
    /// Example implementation of IEventList.
    /// </summary>
    public class EventList : IEventList
    {
        protected Event[] events;
        protected int maxSize;
        protected int fillCount;

        public EventList(int maxSize = 50) => SetMaxSize(maxSize);

        public int GetEventCount() => fillCount;

        public int GetEvent(int index, out Event e)
        {
            var evnt = GetEventByIndex(index);
            if (evnt != null)
            {
                e = evnt.Value;
                return TResult.S_True;
            }
            e = default;
            return TResult.S_False;
        }

        public int AddEvent(ref Event e)
        {
            if (maxSize > fillCount)
            {
                events[fillCount] = e;
                fillCount++;
                return TResult.S_True;
            }
            return TResult.S_False;
        }

        void SetMaxSize(int newMaxSize)
        {
            if (events != null) { events = null; fillCount = 0; }
            if (newMaxSize > 0) events = new Event[newMaxSize];
            maxSize = newMaxSize;
        }

        public void Clear() => fillCount = 0;

        public Event? GetEventByIndex(int index)
            => index < fillCount ? events[index] : null;
    }
}
