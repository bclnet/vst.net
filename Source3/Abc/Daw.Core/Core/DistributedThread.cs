using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Daw.Core
{
    public class DistributedThread
    {
        [DllImport("kernel32.dll")] public static extern int GetCurrentThreadId();
        [DllImport("kernel32.dll")] public static extern int GetCurrentProcessorNumber();

        ThreadStart _threadStart;
        ParameterizedThreadStart _parameterizedThreadStart;
        Thread _thread;

        public int ProcessorAffinity { get; set; }
        public Thread ManagedThread => _thread;

        DistributedThread() => _thread = new Thread(DistributedThreadStart);
        public DistributedThread(ThreadStart threadStart) : this() => _threadStart = threadStart;
        public DistributedThread(ParameterizedThreadStart threadStart) : this() => _parameterizedThreadStart = threadStart;

        public void Start()
        {
            if (_threadStart == null) throw new InvalidOperationException();
            _thread.Start(null);
        }

        public void Start(object parameter)
        {
            if (_parameterizedThreadStart == null) throw new InvalidOperationException();
            _thread.Start(parameter);
        }

        void DistributedThreadStart(object parameter)
        {
            try
            {
                // fix to OS thread
                Thread.BeginThreadAffinity();

                // set affinity
                if (ProcessorAffinity != 0) CurrentThread.ProcessorAffinity = new IntPtr(ProcessorAffinity);

                // call real thread
                if (_threadStart != null) _threadStart();
                else if (_parameterizedThreadStart != null) _parameterizedThreadStart(parameter);
                else throw new InvalidOperationException();
            }
            finally
            {
                // reset affinity
                CurrentThread.ProcessorAffinity = new IntPtr(0xFFFF);
                Thread.EndThreadAffinity();
            }
        }

        ProcessThread CurrentThread
        {
            get
            {
                var id = GetCurrentThreadId();
                return (from ProcessThread th in Process.GetCurrentProcess().Threads where th.Id == id select th).Single();
            }
        }
    }
}
