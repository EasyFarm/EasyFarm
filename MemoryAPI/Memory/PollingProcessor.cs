// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
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