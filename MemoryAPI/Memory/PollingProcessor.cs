using System;
using System.Threading;

namespace MemoryAPI.Memory
{
    public class PollingProcessor
    {
        private readonly Timer _timer;
        private readonly Action _action;

        public PollingProcessor(Action action)
        {
            _action = action;
            _timer = new Timer(Poll);
        }

        public TimeSpan PollDelay { get; set; } = TimeSpan.FromMilliseconds(100);

        private void Poll(object state)
        {
            // Run action
            _action();

            // Start timer with delay.
            StartLazyTimer();
        }

        private void StartLazyTimer()
        {
            _timer.Change(PollDelay, Timeout.InfiniteTimeSpan);
        }

        public void Start()
        {
            StartInstantTimer();
        }

        private void StartInstantTimer()
        {
            _timer.Change(0, Timeout.Infinite);
        }

        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}