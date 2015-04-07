
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

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
