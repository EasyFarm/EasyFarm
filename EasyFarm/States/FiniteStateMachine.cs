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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyFarm.Classes;
using EasyFarm.Logging;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using MemoryAPI;

namespace EasyFarm.States
{
    public class FiniteStateMachine
    {
        private readonly TypeCache<bool> _cache = new TypeCache<bool>();
        private readonly IMemoryAPI _fface;
        private readonly List<IState> _states = new List<IState>();
        private CancellationTokenSource _cancellation = new CancellationTokenSource();

        public FiniteStateMachine(IMemoryAPI fface)
        {
            _fface = fface;
            var stateMemory = new StateMemory(fface);

            //Create the states
            AddState(new DeadState(stateMemory) {Priority = 7});
            AddState(new ZoneState(stateMemory) {Priority = 7});
            AddState(new SetTargetState(stateMemory) {Priority = 7});
            AddState(new SetFightingState(stateMemory) {Priority = 7});
            AddState(new FollowState(stateMemory) {Priority = 5});
            AddState(new RestState(stateMemory) {Priority = 2});
            AddState(new SummonTrustsState(stateMemory) {Priority = 6});
            AddState(new ApproachState(stateMemory) {Priority = 0});
            AddState(new BattleState(stateMemory) {Priority = 3});
            AddState(new WeaponskillState(stateMemory) {Priority = 2});
            AddState(new PullState(stateMemory) {Priority = 4});
            AddState(new StartState(stateMemory) {Priority = 5});
            AddState(new TravelState(stateMemory, new ConfigFactory()) {Priority = 1});
            AddState(new HealingState(stateMemory) {Priority = 2});
            AddState(new EndState(stateMemory) {Priority = 3});
            AddState(new StartEngineState(stateMemory) {Priority = Constants.MaxPriority});

            _states.ForEach(x => x.Enabled = true);
        }

        private void AddState(IState component)
        {
            _states.Add(component);
        }

        // Start and stop.
        public void Start()
        {
            ReEnableStartState();
            RunFiniteStateMainWithThread();
        }

        private void ReEnableStartState()
        {
            var startEngineState = _states.FirstOrDefault(x => x.GetType() == typeof(StartEngineState));
            if (startEngineState != null) startEngineState.Enabled = true;
        }

        public void Stop()
        {
            _cancellation.Cancel();
        }

        private void RunFiniteStateMainWithThread()
        {
            _cancellation = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                using (_cancellation.Token.Register(StopThreadQuickly(Thread.CurrentThread)))
                {
                    try
                    {
                        RunStateMachine();
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        Logger.Log(new LogEntry(LoggingEventType.Information, "FSM thread interrupted", ex));
                    }
                    catch (ThreadAbortException ex)
                    {
                        Logger.Log(new LogEntry(LoggingEventType.Information, "FSM thread aborted", ex));
                    }
                    catch (OperationCanceledException ex)
                    {
                        Logger.Log(new LogEntry(LoggingEventType.Information, "FSM thread cancelled", ex));
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(new LogEntry(LoggingEventType.Error, "FSM error", ex));
                        LogViewModel.Write("An error has occurred: please check easyfarm.log for more information");
                        AppServices.InformUser("An error occurred!");
                    }
                    finally
                    {
                        _fface.Navigator.Reset();
                    }
                }
            }, _cancellation.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private Action StopThreadQuickly(Thread backgroundThread)
        {
            return () =>
            {
                if (!backgroundThread.Join(500)) backgroundThread.Interrupt();
            };
        }

        private void RunStateMachine()
        {
            while (true)
            {
                // Sort the List, States may have updated Priorities.
                _states.Sort();

                // Find a State that says it needs to run.
                foreach (var mc in _states.Where(x => x.Enabled).ToList())
                {
                    _cancellation.Token.ThrowIfCancellationRequested();

                    var isRunnable = mc.Check();

                    // Run last state's exits method.
                    if (_cache[mc] != isRunnable)
                    {
                        if (isRunnable) mc.Enter();
                        else mc.Exit();
                        _cache[mc] = isRunnable;
                    }

                    if (isRunnable) mc.Run();
                }

                TimeWaiter.Pause(250);
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}