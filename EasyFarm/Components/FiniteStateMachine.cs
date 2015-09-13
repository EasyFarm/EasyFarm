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

// Author: Myrmidon
// Site: FFEVO.net
// All credit to him!

using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EasyFarm.Components
{
    public class FiniteStateEngine
    {
        private List<IState> _components;
        private CancellationTokenSource _cancellation;
        private IState _lastRun;

        public FiniteStateEngine(FFACE fface)
        {
            _components = new List<IState>();

            //Create the states
            AddComponent(new RestComponent(fface) { Priority = 2 });
            AddComponent(new ApproachComponent(fface) { Priority = 0 });
            AddComponent(new BattleComponent(fface) { Priority = 3 });
            AddComponent(new WeaponSkillComponent(fface) { Priority = 2 });
            AddComponent(new PullComponent(fface) { Priority = 4 });
            AddComponent(new StartComponent(fface) { Priority = 5 });
            AddComponent(new TravelComponent(fface) { Priority = 1 });
            AddComponent(new HealingComponent(fface) { Priority = 2 });
            AddComponent(new EndComponent(fface) { Priority = 3 });

            _components.ForEach(x => x.Enabled = true);
        }

        private async Task MainLoop()
        {
            _cancellation = new CancellationTokenSource();

            while (true)
            {                
                Run();
                Task task = Task.Delay(100, _cancellation.Token);
                try { await task; }
                catch (TaskCanceledException){ return; }                
            }
        }

        private void Run()
        {
            // Sort the List, States may have updated Priorities.
            _components.Sort();

            // Find a State that says it needs to run.
            foreach (var mc in _components.Where(x => x.Enabled))
            {
                if (mc.CheckComponent())
                {
                    // Run current state's enter method. 
                    if (_lastRun != mc)
                    {
                        mc.EnterComponent();
                    }

                    // Run current state's run method. 
                    mc.RunComponent();
                    _lastRun = mc;

                    // Run last state's exits method. 
                    if (_lastRun != null && _lastRun != mc)
                    {
                        _lastRun.ExitComponent();
                    }
                }                
            }
        }

        public void AddComponent(IState component)
        {
            this._components.Add(component);
        }

        public void RemoveComponent(IState component)
        {
            this._components.Remove(component);
        }

        // Start and stop.
        public void Start()
        {
            Task.Run(() => MainLoop());
        }

        public void Stop()
        {
            _cancellation.Cancel();
        }
    }
}