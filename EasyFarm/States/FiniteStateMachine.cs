// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyFarm.Classes;
using EasyFarm.Context;
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
        public readonly GameContext _context;

        public FiniteStateMachine(IMemoryAPI fface)
        {
            _fface = fface;
            _context = new GameContext(fface);

            AddState(new DeadState() { Priority = 7 });
            AddState(new ZoneState() { Priority = 7 });
            AddState(new SetTargetState() { Priority = 0 });
            AddState(new SetFightingState() { Priority = 7 });
            AddState(new FollowState() { Priority = 5 });
            AddState(new RestState() { Priority = 2 });
            AddState(new SummonTrustsState() { Priority = 6 });
            AddState(new ApproachState() { Priority = 0 });
            AddState(new BattleState() { Priority = 3 });
            AddState(new WeaponskillState() { Priority = 2 });
            AddState(new PullState() { Priority = 4 });
            AddState(new StartState() { Priority = 5 });
            AddState(new TravelState() { Priority = 1 });
            AddState(new HealingState() { Priority = 2 });
            AddState(new EndState() { Priority = 3 });
            AddState(new StartEngineState() { Priority = Constants.MaxPriority });

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
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (true)
            {
                if (timer.Elapsed.Minutes > _context.Config.MinutesToRun) {
                    LogViewModel.Write("Stopping due to time limit reached: " + timer.Elapsed.Minutes.ToString() + " minutes");
                    throw new Exception();
                }
                else if (_context.API.Player.JobLevel >= _context.Config.StopAtLevel)
                {
                    LogViewModel.Write("Stopping due to level limit reached: " + _context.API.Player.JobLevel.ToString());
                    throw new Exception();
                }

                // Sort the List, States may have updated Priorities.
                _states.Sort();

                // Find a State that says it needs to run.
                foreach (var mc in _states.Where(x => x.Enabled).ToList())
                {
                    _cancellation.Token.ThrowIfCancellationRequested();

                    var isRunnable = mc.Check(_context);

                    // Run last state's exits method.
                    if (_cache[mc] != isRunnable)
                    {
                        if (isRunnable) mc.Enter(_context);
                        else mc.Exit(_context);
                        _cache[mc] = isRunnable;
                    }

                    if (isRunnable) mc.Run(_context);
                }

                TimeWaiter.Pause(100);
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}