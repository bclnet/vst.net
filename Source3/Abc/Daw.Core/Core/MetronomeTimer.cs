using System;

namespace Daw.Core.Core
{
    class MetronomeTimer
    {
        MediaTimer _timer = new() { Resolution = 1, Period = BpmToMilliseconds(120) };

        public MetronomeTimer() => _timer.Tick += TimerOnTick;

        public delegate void TickDelegate();

        public event TickDelegate Tick;

        public bool IsRunning { get; private set; }

        public int BPM
        {
            set
            {
                try
                {
                    _timer.Period = BpmToMilliseconds(value);
                }
                catch { } // Do nothing
            }
        }

        void TimerOnTick(object sender, EventArgs e) => Tick.Invoke();

        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                _timer.Start();
                TimerOnTick(null, null);
            }
        }

        public void Stop()
        {
            IsRunning = false;
            _timer.Stop();
        }

        static int BpmToMilliseconds(int bpm) => 60000 / bpm;
    }
}
