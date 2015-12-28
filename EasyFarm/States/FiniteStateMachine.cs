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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class FiniteStateMachine
    {
        private readonly TypeCache<bool> _cache = new TypeCache<bool>();
        private CancellationTokenSource _cancellation = new CancellationTokenSource();
        private readonly List<IState> _components = new List<IState>();

        public FiniteStateMachine(IMemoryAPI fface)
        {
            //Create the states
            AddComponent(new TrackPlayerState(fface) { Priority = 6 });
            AddComponent(new DeadState(fface) {Priority = 6});
            AddComponent(new ZoneState(fface) {Priority = 6});
            AddComponent(new SetTargetState(fface) {Priority = 6});
            AddComponent(new SetFightingState(fface) {Priority = 6});
            AddComponent(new FollowState(fface) {Priority = 5});
            AddComponent(new RestState(fface) {Priority = 2});
            AddComponent(new ApproachState(fface) {Priority = 0});
            AddComponent(new BattleState(fface) {Priority = 3});
            AddComponent(new WeaponskillState(fface) {Priority = 2});
            AddComponent(new PullState(fface) {Priority = 4});
            AddComponent(new StartState(fface) {Priority = 5});
            AddComponent(new TravelState(fface) {Priority = 1});
            AddComponent(new HealingState(fface) {Priority = 2});
            AddComponent(new EndState(fface) {Priority = 3});
            AddComponent(new StartEngineState(fface) {Priority = Constants.MaxPriority});

            _components.ForEach(x => x.Enabled = true);
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
            // Enable running of
            IState startEngineState = _components.FirstOrDefault(x => x.GetType() == typeof (StartEngineState));
            if (startEngineState != null) startEngineState.Enabled = true;
            MainLoop();
        }

        public void Stop()
        {
            _cancellation.Cancel();
        }

        private void MainLoop()
        {
            _cancellation = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {                    
                    // Sort the List, States may have updated Priorities.
                    _components.Sort();

                    // Find a State that says it needs to run.
                    foreach (var mc in _components.Where(x => x.Enabled).ToList())
                    {
                        _cancellation.Token.ThrowIfCancellationRequested();

                        bool isRunnable = mc.CheckComponent();

                        // Run last state's exits method.
                        if (_cache[mc] != isRunnable)
                        {
                            if (isRunnable) mc.EnterComponent();
                            else mc.ExitComponent();
                            _cache[mc] = isRunnable;
                        }

                        if (isRunnable)
                        {
                            // Run current state's run method.
                            mc.RunComponent();
                        }
                    }

                    Thread.Sleep(100);
                }
            }, _cancellation.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }

    public class TypeCache<T>
    {
        private readonly Dictionary<Type, T> _cache = new Dictionary<Type, T>();

        public T this[object @object]
        {
            get
            {
                if (@object == null) return default(T);

                if (_cache.ContainsKey(@object.GetType()))
                {
                    return _cache[@object.GetType()];
                }

                return default(T);
            }
            set
            {
                if (@object == null) return;

                if (_cache.ContainsKey(@object.GetType()))
                {
                    _cache[@object.GetType()] = value;
                }
                else
                {
                    _cache.Add(@object.GetType(), value);
                }
            }
        }
    }
}