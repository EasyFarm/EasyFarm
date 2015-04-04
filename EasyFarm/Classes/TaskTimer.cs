using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    /// <summary>
    /// A more reliable timer that handles running long 
    /// term processes. 
    /// </summary>
    public class TaskTimer
    {
        /// <summary>
        /// Handle for the threadpool task. 
        /// </summary>
        private RegisteredWaitHandle _handle = null;

        /// <summary>
        /// Signals this task to stop. 
        /// </summary>
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);

        /// <summary>
        /// The tick interval in milliseconds. 
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Sets whether the timer should tick only once. 
        /// </summary>
        public bool AutoReset { get; set; }

        /// <summary>
        /// The callback to call on tick. 
        /// </summary>
        public delegate void Elapsed(bool IsCanceled);

        /// <summary>
        /// Triggered when the time has elapsed. 
        /// </summary>
        public event Elapsed OnElapsed;

        public TaskTimer()
        {
            Interval = 100;
            AutoReset = true;
        }

        ~TaskTimer()
        {
            if (_handle != null)
            {
                _handle.Unregister(null);
            }

            _resetEvent.Dispose();
        }

        /// <summary>
        /// Stops the timer. 
        /// </summary>
        public void Stop()
        {
            // Unregister subscription for callbacks. 
            if (_handle != null)
            {
                _handle.Unregister(null);
            }
        }

        /// <summary>
        /// Private method for firing the OnElapsed event. 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="timedOut"></param>
        private void WorkerMethod(object state, bool timedOut)
        {
            OnElapsed(timedOut);
        }

        /// <summary>
        /// Starts the timer. 
        /// </summary>
        public void Start()
        {
            // Register for callbacks at specified intervals. 
            _handle = ThreadPool.RegisterWaitForSingleObject(
                _resetEvent,
                WorkerMethod,
                null,
                Interval,
                !AutoReset
            );
        }
    }
}
