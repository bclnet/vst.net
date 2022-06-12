using System;

namespace Daw.Core
{
    public class ByteArrayRingBuffer
    {
        byte[] _buf;
        int _tail = 0;
        int _head = 0;
        int _capacityPlusOne;

        public ByteArrayRingBuffer(int capacity)
        {
            _capacityPlusOne = capacity + 1;
            _buf = new byte[_capacityPlusOne]; // Byte array actually contains one byte more than capacity, because tail can never filled up to head
        }
        
        public int Capacity => _capacityPlusOne - 1;

        /// <summary>
        /// Number of bytes in ringbuffer
        /// </summary>
        public int Count
            => _tail == _head ? 0
            : _tail > _head ? _tail - _head
            : _capacityPlusOne - _head + _tail;

        /// <summary>
        /// Put bytes in the ringbuffer
        /// </summary>
        /// <param name="data">Bytes to put in</param>
        /// <param name="len">Number of bytes</param>
        /// <returns></returns>
        public bool Put(byte[] data, int len)
        {
            if (_capacityPlusOne - 1 - Count < len) return false; // Space enough?
            lock (_buf)
                if (_tail + len > _capacityPlusOne)
                {
                    var split = _capacityPlusOne - _tail; // Split in two parts
                    Buffer.BlockCopy(data, 0, _buf, _tail, split); // First copy part to the end of the ringbuffer
                    Buffer.BlockCopy(data, split, _buf, 0, len - split); // Wrap around
                    _tail = len - split; // Set new tail
                }
                else
                {
                    Buffer.BlockCopy(data, 0, _buf, _tail, len); // Only one copy action
                    _tail += len; // Set new tail
                }
            return true;
        }

        /// <summary>
        /// Get a number of bytes from the ringbuffer
        /// </summary>
        /// <param name="len">Number of bytes to get</param>
        /// <returns></returns>
        public byte[] Get(int len)
        {
            if (len > Count) throw new ArgumentException(string.Format("Only {0} bytes in buffer, requested {1}", Count, len));
            var data = new byte[len];
            lock (_buf)
                if (_head + len > _capacityPlusOne)
                {
                    var split = _capacityPlusOne - _head; // Split in two parts
                    Buffer.BlockCopy(_buf, _head, data, 0, split); // First copy part to the end of the ringbuffer
                    Buffer.BlockCopy(_buf, 0, data, split, len - split); // Wrap around
                    _head = len - split; // Set new head
                }
                else
                {
                    Buffer.BlockCopy(_buf, _head, data, 0, len); // Only one copy action
                    _head += len; // Set new head
                }
            return data;
        }

        /// <summary>
        /// Clear the ringbuffer
        /// </summary>
        public void Clear() => _head = _tail;
    }
}
