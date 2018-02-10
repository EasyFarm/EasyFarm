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
using System.Threading;
using System.Threading.Tasks;
using EasyFarm.States;
using MemoryAPI;

namespace EasyFarm.Classes
{
    public class PlayerMonitor
    {
        private CancellationTokenSource _tokenSource;
        private readonly PlayerMovementTracker _movementTracker;

        public PlayerMonitor(IMemoryAPI memory)
        {
            _movementTracker = new PlayerMovementTracker(memory);
        }

        public void Start()
        {
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(Monitor, _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void Stop()
        {
            _tokenSource.Cancel();
        }

        private void Monitor()
        {
            while (true)
            {
                if (_tokenSource.IsCancellationRequested)
                {
                    _tokenSource.Token.ThrowIfCancellationRequested();
                }

                _movementTracker.RunComponent();

                TimeWaiter.Pause(100);
            }
        }
    }
}
