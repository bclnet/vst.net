using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Daw.Core
{
    /// <summary>
    /// Defines constants for the multimedia Timer's event types.
    /// </summary>
    public enum TimerMode
    {
        /// <summary>
        /// Timer event occurs once.
        /// </summary>
        OneShot,
        /// <summary>
        /// Timer event occurs periodically.
        /// </summary>
        Periodic
    }

    /// <summary>
    /// Represents information about the multimedia Timer's capabilities.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TimerCaps
    {
        /// <summary>
        /// Minimum supported period in milliseconds.
        /// </summary>
        public int periodMin;
        /// <summary>
        /// Maximum supported period in milliseconds.
        /// </summary>
        public int periodMax;
    }

    /// <summary>
    /// The exception that is thrown when a timer fails to start.
    /// </summary>
    public class TimerStartException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the TimerStartException class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception. 
        /// </param>
        public TimerStartException(string message) : base(message) { }
    }

    /// <summary>
    /// Represents the Windows multimedia timer.
    /// </summary>
    public sealed class MediaTimer : IComponent
    {
        delegate void TimeProc(int id, int msg, int user, int param1, int param2); // Represents the method that is called by Windows when a timer event occurs.
        delegate void EventRaiser(EventArgs e); // Represents methods that raise events.
        [DllImport("winmm.dll")] static extern int timeGetDevCaps(ref TimerCaps caps, int sizeOfTimerCaps); // Gets timer capabilities.
        [DllImport("winmm.dll")] static extern int timeSetEvent(int delay, int resolution, TimeProc proc, int user, int mode); // Creates and starts the timer.
        [DllImport("winmm.dll")] static extern int timeKillEvent(int id); // Stops and destroys the timer.
        const int TIMERR_NOERROR = 0; // Indicates that the operation was successful.

        int timerID; // Timer identifier.
        volatile TimerMode mode; // Timer mode.
        volatile int period; // Period between timer events in milliseconds.
        volatile int resolution; // Timer resolution in milliseconds.
        TimeProc timeProcPeriodic; // Called by Windows when a timer periodic event occurs.
        TimeProc timeProcOneShot; // Called by Windows when a timer one shot event occurs.
        EventRaiser tickRaiser; // Represents the method that raises the Tick event.
        bool running = false; // Indicates whether or not the timer is running.
        volatile bool disposed = false; // Indicates whether or not the timer has been disposed.
        ISynchronizeInvoke synchronizingObject = null; // The ISynchronizeInvoke object to use for marshaling events.
        ISite site = null; // For implementing IComponent.
        static TimerCaps caps; // Multimedia timer capabilities.

        /// <summary>
        /// Initialize class.
        /// </summary>
        static MediaTimer() => timeGetDevCaps(ref caps, Marshal.SizeOf(caps)); // Get multimedia timer capabilities.

        /// <summary>
        /// Initializes a new instance of the Timer class with the specified IContainer.
        /// </summary>
        /// <param name="container">
        /// The IContainer to which the Timer will add itself.
        /// </param>
        public MediaTimer(IContainer container)
        {
            /// Required for Windows.Forms Class Composition Designer support
            container.Add(this);
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the Timer class.
        /// </summary>
        public MediaTimer() => Initialize();
        ~MediaTimer() { if (IsRunning) timeKillEvent(timerID); } // Stop and destroy timer.

        /// <summary>
        /// Frees timer resources.
        /// </summary>
        public void Dispose()
        {
            if (disposed) return;
            if (IsRunning) Stop();
            disposed = true;
            OnDisposed(EventArgs.Empty);
        }

        public event EventHandler Disposed;

        // Initialize timer with default values.
        void Initialize()
        {
            mode = TimerMode.Periodic;
            period = Capabilities.periodMin;
            resolution = 1;
            running = false;
            timeProcPeriodic = new TimeProc(TimerPeriodicEventCallback);
            timeProcOneShot = new TimeProc(TimerOneShotEventCallback);
            tickRaiser = new EventRaiser(OnTick);
        }

        /// <summary>
        /// Occurs when the Timer has started;
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when the Timer has stopped;
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Occurs when the time period has elapsed.
        /// </summary>
        public event EventHandler Tick;

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The timer has already been disposed.
        /// </exception>
        /// <exception cref="TimerStartException">
        /// The timer failed to start.
        /// </exception>
        public void Start()
        {
            if (disposed) throw new ObjectDisposedException("Timer");
            if (IsRunning) return;
            // If the periodic event callback should be used.
            if (Mode == TimerMode.Periodic) timerID = timeSetEvent(Period, Resolution, timeProcPeriodic, 0, (int)Mode); // Create and start timer.
            // Else the one shot event callback should be used.
            else timerID = timeSetEvent(Period, Resolution, timeProcOneShot, 0, (int)Mode); // Create and start timer.
            // If the timer was created successfully.
            if (timerID != 0)
            {
                running = true;
                if (SynchronizingObject != null && SynchronizingObject.InvokeRequired) SynchronizingObject.BeginInvoke(new EventRaiser(OnStarted), new object[] { EventArgs.Empty });
                else OnStarted(EventArgs.Empty);
            }
            else throw new TimerStartException("Unable to start multimedia Timer.");
        }

        /// <summary>
        /// Stops timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>
        public void Stop()
        {
            if (disposed) throw new ObjectDisposedException("Timer");
            if (!running) return;
            // Stop and destroy timer.
            var result = timeKillEvent(timerID);
            Debug.Assert(result == TIMERR_NOERROR);
            running = false;
            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired) SynchronizingObject.BeginInvoke(new EventRaiser(OnStopped), new object[] { EventArgs.Empty });
            else OnStopped(EventArgs.Empty);
        }

        // Callback method called by the Win32 multimedia timer when a timer periodic event occurs.
        void TimerPeriodicEventCallback(int id, int msg, int user, int param1, int param2)
        {
            if (synchronizingObject != null) synchronizingObject.BeginInvoke(tickRaiser, new object[] { EventArgs.Empty });
            else OnTick(EventArgs.Empty);
        }

        // Callback method called by the Win32 multimedia timer when a timer one shot event occurs.
        void TimerOneShotEventCallback(int id, int msg, int user, int param1, int param2)
        {
            if (synchronizingObject != null) { synchronizingObject.BeginInvoke(tickRaiser, new object[] { EventArgs.Empty }); Stop(); }
            else { OnTick(EventArgs.Empty); Stop(); }
        }

        void OnDisposed(EventArgs e) => Disposed?.Invoke(this, e); // Raises the Disposed event.
        void OnStarted(EventArgs e) => Started?.Invoke(this, e); // Raises the Started event.
        void OnStopped(EventArgs e) => Stopped?.Invoke(this, e); // Raises the Stopped event.
        void OnTick(EventArgs e) => Tick?.Invoke(this, e); // Raises the Tick event.

        /// <summary>
        /// Gets or sets the object used to marshal event-handler calls.
        /// </summary>
        public ISynchronizeInvoke SynchronizingObject
        {
            get
            {
                if (disposed) throw new ObjectDisposedException("Timer");
                return synchronizingObject;
            }
            set
            {
                if (disposed) throw new ObjectDisposedException("Timer");
                synchronizingObject = value;
            }
        }

        /// <summary>
        /// Gets or sets the time between Tick events.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>   
        public int Period
        {
            get
            {
                if (disposed) throw new ObjectDisposedException("Timer");
                return period;
            }
            set
            {
                if (disposed) throw new ObjectDisposedException("Timer");
                else if (value < Capabilities.periodMin || value > Capabilities.periodMax) throw new ArgumentOutOfRangeException("Period", value, "Multimedia Timer period out of range.");
                period = value;
                if (IsRunning) { Stop(); Start(); }
            }
        }

        /// <summary>
        /// Gets or sets the timer resolution in [ms].
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>        
        /// <remarks>
        /// The resolution is in milliseconds. The resolution increases 
        /// with smaller values; a resolution of 0 indicates periodic events 
        /// should occur with the greatest possible accuracy. To reduce system 
        /// overhead, however, you should use the maximum value appropriate 
        /// for your application.
        /// </remarks>
        public int Resolution
        {
            get
            {
                if (disposed) throw new ObjectDisposedException("Timer");
                return resolution;
            }
            set
            {
                if (disposed) throw new ObjectDisposedException("Timer");
                else if (value < 0) throw new ArgumentOutOfRangeException("Resolution", value, "Multimedia timer resolution out of range.");
                resolution = value;
                if (IsRunning) { Stop(); Start(); }
            }
        }

        /// <summary>
        /// Gets the timer mode.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>
        public TimerMode Mode
        {
            get
            {
                if (disposed) throw new ObjectDisposedException("Timer");
                return mode;
            }
            set
            {
                if (disposed) throw new ObjectDisposedException("Timer");
                mode = value;
                if (IsRunning) { Stop(); Start(); }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Timer is running.
        /// </summary>
        public bool IsRunning => running;

        /// <summary>
        /// Gets the timer capabilities.
        /// </summary>
        public static TimerCaps Capabilities => caps;

        public ISite Site
        {
            get => site;
            set => site = value;
        }
    }
}
