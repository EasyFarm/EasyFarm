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
        private List<IState> _components = new List<IState>();
        private CancellationTokenSource _cancellation = new CancellationTokenSource();
        private TypeCache<bool> _cache = new TypeCache<bool>();

        public FiniteStateEngine(FFACE fface)
        {
            //Create the states
            AddComponent(new SetTargetState(fface) { Priority = 6 });
            AddComponent(new SetFightingState(fface) { Priority = 6 });
            AddComponent(new FollowState(fface) { Priority = 5 });
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

        private void MainLoop()
        {
            _cancellation = new CancellationTokenSource();

            Task t = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _cancellation.Token.ThrowIfCancellationRequested();
                    Run();
                    Thread.Sleep(100);
                }
            }, _cancellation.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void Run()
        {
            // Sort the List, States may have updated Priorities.
            _components.Sort();

            // Find a State that says it needs to run.
            foreach (var mc in _components.Where(x => x.Enabled).ToList())
            {
                _cancellation.Token.ThrowIfCancellationRequested();

                bool IsRunnable = mc.CheckComponent();

                // Run last state's exits method.
                if (_cache[mc] != IsRunnable)
                {
                    if (IsRunnable) mc.EnterComponent();
                    else mc.ExitComponent();
                    _cache[mc] = IsRunnable;
                }

                if (IsRunnable)
                {
                    // Run current state's run method.
                    mc.RunComponent();
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
            MainLoop();
        }

        public void Stop()
        {
            _cancellation.Cancel();
        }
    }

    public class TypeCache<T>
    {
        private Dictionary<Type, T> cache = new Dictionary<Type, T>();

        public T this[object @object]
        {
            get
            {
                if (@object == null) return default(T);

                if (cache.ContainsKey(@object.GetType()))
                {
                    return cache[@object.GetType()];
                }
                else
                {
                    return default(T);
                }
            }
            set
            {
                if (@object == null) return;

                if (cache.ContainsKey(@object.GetType()))
                {
                    cache[@object.GetType()] = value;
                }
                else
                {
                    cache.Add(@object.GetType(), value);
                }
            }
        }
    }
}