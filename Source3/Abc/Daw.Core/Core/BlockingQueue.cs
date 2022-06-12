using System;
using System.Threading;

namespace Daw.Core
{
    public sealed class BlockingQueue<T>
    {
        T[] _buffer;
        int _count;
        int _size;
        int _head;
        int _tail;
        readonly object _syncRoot = new();
        ManualResetEvent _queueNotFullEvent = new(true);
        ManualResetEvent _queueNotEmptyEvent = new(false);

        public BlockingQueue(int size)
        {
            if (size < 1) throw new ArgumentOutOfRangeException("Size must be > 0");
            _size = size;
            _buffer = new T[size];
        }

        public object[] Values
        {
            get
            {
                object[] values;
                lock (_syncRoot)
                {
                    values = new object[_count];
                    var pos = _head;
                    for (var i = 0; i < _count; i++)
                    {
                        values[i] = _buffer[pos];
                        pos = (pos + 1) % _size;
                    }
                }
                return values;
            }
        }

        public bool Enqueue(T item, int millisecondsTimeout = Timeout.Infinite)
        {
            while (true)
            {
                lock (_syncRoot)
                    if (_count < _size)
                    {
                        _buffer[_tail] = item;
                        _tail = (_tail + 1) % _size;
                        _count++;
                        if (_count == 1) _queueNotEmptyEvent.Set();
                        return true;
                    }
                    else _queueNotFullEvent.Reset();
                if (!_queueNotFullEvent.WaitOne(millisecondsTimeout, false)) return false;
            }
        }

        public bool Dequeue(out T obj, int millisecondsTimeout = Timeout.Infinite)
        {
            obj = default;
            while (true)
            {
                lock (_syncRoot)
                    if (_count > 0)
                    {
                        obj = _buffer[_head];
                        _buffer[_head] = default;
                        _head = (_head + 1) % _size;
                        _count--;
                        if (_count == (_size - 1)) _queueNotFullEvent.Set();
                        return true;
                    }
                    else _queueNotEmptyEvent.Reset();

                if (!_queueNotEmptyEvent.WaitOne(millisecondsTimeout, false)) return false;
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _count = 0;
                _head = 0;
                _tail = 0;
                for (var i = 0; i < _buffer.Length; i++) _buffer[i] = default;
            }
        }
    }
}
