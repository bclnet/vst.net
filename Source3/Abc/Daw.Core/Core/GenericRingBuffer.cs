using System;

namespace Daw.Core
{
    public sealed class GenericRingBuffer<T>
    {
        readonly T[] _array;
        int _head;
        int _tail;
        readonly int _capacity;

        /// <summary>
        /// Initializes a new instance of RingBuffer with maximum 511 elements
        /// </summary>
        public GenericRingBuffer() : this(512) { }

        /// <summary>
        /// Initializes RingBuffer with the specified capacity-1; e.g; size 512 means maximum 511 elements
        /// </summary>
        /// <param name="size">Maximum number of elements to store</param>
        public GenericRingBuffer(int size)
        {
            // Check if size is power of 2           
            if ((size & (size - 1)) != 0) throw new ArgumentOutOfRangeException(nameof(size), "Size must be a power of 2");
            _capacity = size;
            _array = new T[size];
        }

        /// <summary>
        /// Number of items in buffer
        /// </summary>
        public int Count
            => _tail == _head ? 0
            : _head > _tail ? _head - _tail
            : _capacity - _tail + _head;

        /// <summary>
        /// Add a single item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Returns true if added, returns false if buffer full</returns>
        public bool Add(T item)
        {
            if (_capacity - 1 - Count == 0) return false; // Space enough?
            var newHead = _head + 1;
            newHead %= _capacity;
            _array[newHead] = item;
            _head = newHead;
            return true;
        }

        /// <summary>
        /// Peek the oldest item  
        /// </summary>
        /// <returns>Single item</returns>
        public T Peek()
        {
            var newTail = _tail + 1;
            newTail %= _capacity;
            T ret = _array[newTail];
            return ret;
        }

        /// <summary>
        /// Peek all items  
        /// </summary>
        /// <param name="all">All items in buffer</param>
        /// <returns>Number of items in buffer</returns>
        public int PeekAll(out T[] all)
        {
            var count = Count;
            if (count == 0) all = null;
            else
            {
                all = new T[count];
                var newTail = _tail;
                for (var i = 0; i < count; i++)
                {
                    newTail++;
                    newTail %= _capacity;
                    all[i] = _array[newTail];
                }
            }
            return count;
        }

        /// <summary>
        /// Free number of items after being used with Peek or PeekAll
        /// </summary>
        /// <param name="count"></param>
        public void Free(int count)
        {
            var newTail = _tail + count;
            newTail %= _capacity;
            _tail = newTail;
        }

        /// <summary>
        /// Clear all the items in the buffer
        /// </summary>
        public void Clear() => _head = _tail;
    }
}

