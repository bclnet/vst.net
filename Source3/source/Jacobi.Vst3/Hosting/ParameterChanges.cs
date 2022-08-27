using System.Collections.Generic;
using static Jacobi.Vst3.TResult;
using ParamID = System.UInt32;
using ParamValue = System.Double;

namespace Jacobi.Vst3.Hosting
{
    /// <summary>
    /// Implementation's example of IParamValueQueue - not threadsave!.
    /// </summary>
    public class ParameterValueQueue : IParamValueQueue
    {
        protected ParamID paramID;

        protected class ParameterQueueValue
        {
            public ParameterQueueValue(ParamValue value, int sampleOffset) { Value = value; SampleOffset = sampleOffset; }
            public ParamValue Value;
            public int SampleOffset;
        }
        protected List<ParameterQueueValue> values = new();

        const int kQueueReservedPoints = 5;

        public ParameterValueQueue(ParamID paramID)
        {
            this.paramID = paramID;
            values.Capacity = kQueueReservedPoints;
        }

        public void Clear()
            => values.Clear();

        public ParamID GetParameterId()
            => paramID;

        public int GetPointCount()
            => values.Count;

        public TResult GetPoint(int index, out int sampleOffset, out ParamValue value)
        {
            if (index >= 0 && index < values.Count)
            {
                var queueValue = values[index];
                sampleOffset = queueValue.SampleOffset;
                value = queueValue.Value;
                return kResultTrue;
            }
            sampleOffset = default;
            value = default;
            return kResultFalse;
        }

        public TResult AddPoint(int sampleOffset, ParamValue value, out int index)
        {
            var destIndex = values.Count;
            for (var i = 0; i < values.Count; i++)
            {
                if (values[i].SampleOffset == sampleOffset)
                {
                    values[i].Value = value;
                    index = i;
                    return kResultTrue;
                }
                else if (values[i].SampleOffset > sampleOffset)
                {
                    destIndex = i;
                    break;
                }
            }

            // need new point
            ParameterQueueValue queueValue = new(value, sampleOffset);
            if (destIndex == values.Count)
                values.Add(queueValue);
            else
                values.Insert(destIndex, queueValue);

            index = destIndex;

            return kResultTrue;
        }

        public void SetParamID(ParamID pID)
            => paramID = pID;
    }

    /// <summary>
    /// Implementation's example of IParameterChanges - not threadsave!.
    /// </summary>
    public class ParameterChanges : IParameterChanges
    {
        protected List<ParameterValueQueue> queues = new();
        protected int usedQueueCount;

        public ParameterChanges(int maxParameters = 0)
            => SetMaxParameters(maxParameters);

        public void SetMaxParameters(int maxParameters)
        {
            if (maxParameters < 0)
                return;

            while (queues.Count < maxParameters)
                queues.Add(new ParameterValueQueue(Constants.NoParamId));

            while (queues.Count > maxParameters)
                queues.RemoveAt(queues.Count - 1);

            if (usedQueueCount > maxParameters)
                usedQueueCount = maxParameters;
        }

        public void ClearQueue()
            => usedQueueCount = 0;

        //---IParameterChanges-----------------------------
        public int GetParameterCount()
            => usedQueueCount;

        public IParamValueQueue GetParameterData(int index)
        {
            if (index >= 0 && index < usedQueueCount)
                return queues[index];
            return null;
        }

        public IParamValueQueue AddParameterData(ParamID pid, out int index)
        {
            for (var i = 0; i < usedQueueCount; i++)
                if (queues[i].GetParameterId() == pid)
                {
                    index = i;
                    return queues[i];
                }

            ParameterValueQueue valueQueue;
            if (usedQueueCount < queues.Count)
            {
                valueQueue = queues[usedQueueCount];
                valueQueue.SetParamID(pid);
                valueQueue.Clear();
            }
            else
            {
                queues.Add(new ParameterValueQueue(pid));
                valueQueue = queues[^1];
            }

            index = usedQueueCount;
            usedQueueCount++;
            return valueQueue;
        }
    }

    /// <summary>
    /// Ring buffer for transferring parameter changes from a writer to a read thread .
    /// </summary>
    public class ParameterChangeTransfer
    {
        protected class ParameterChange
        {
            public ParamID Id;
            public ParamValue Value;
            public int SampleOffset;
        }

        protected int size;
        protected ParameterChange[] changes;

        protected volatile int readIndex;
        protected volatile int writeIndex;

        public ParameterChangeTransfer(int maxParameters = 0)
            => SetMaxParameters(maxParameters);

        public void SetMaxParameters(int maxParameters)
        {
            // reserve memory for twice the amount of all parameters
            var newSize = maxParameters * 2;
            if (size != newSize)
            {
                changes = null;
                size = newSize;
                if (size > 0)
                    changes = new ParameterChange[size];
            }
        }

        public void AddChange(ParamID pid, ParamValue value, int sampleOffset)
        {
            if (changes != null)
            {
                changes[writeIndex].Id = pid;
                changes[writeIndex].Value = value;
                changes[writeIndex].SampleOffset = sampleOffset;

                var newWriteIndex = writeIndex + 1;
                if (newWriteIndex >= size)
                    newWriteIndex = 0;
                if (readIndex != newWriteIndex)
                    writeIndex = newWriteIndex;
            }
        }

        public bool GetNextChange(out ParamID pid, out ParamValue value, out int sampleOffset)
        {
            if (changes == null)
            {
                pid = default;
                value = default;
                sampleOffset = default;
                return false;
            }

            var currentWriteIndex = writeIndex;
            if (readIndex != currentWriteIndex)
            {
                pid = changes[readIndex].Id;
                value = changes[readIndex].Value;
                sampleOffset = changes[readIndex].SampleOffset;

                var newReadIndex = readIndex + 1;
                if (newReadIndex >= size)
                    newReadIndex = 0;
                readIndex = newReadIndex;
                return true;
            }
            pid = default;
            value = default;
            sampleOffset = default;
            return false;
        }

        public void TransferChangesTo(ParameterChanges dest)
        {
            while (GetNextChange(out var pid, out var value, out var sampleOffset))
            {
                var queue = dest.AddParameterData(pid, out var index);
                if (queue != null)
                    queue.AddPoint(sampleOffset, value, out index);
            }
        }

        public void TransferChangesFrom(ParameterChanges source)
        {
            for (var i = 0; i < source.GetParameterCount(); i++)
            {
                var queue = source.GetParameterData(i);
                if (queue != null)
                    for (var j = 0; j < queue.GetPointCount(); j++)
                        if (queue.GetPoint(j, out var sampleOffset, out var value) == kResultTrue)
                            AddChange(queue.GetParameterId(), value, sampleOffset);
            }
        }

        public void RemoveChanges()
            => writeIndex = readIndex;
    }
}
