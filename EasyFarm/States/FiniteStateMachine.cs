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

using EasyFarm.Classes;
using EasyFarm.Logging;
using EasyFarm.ViewModels;
using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EasyFarm.States
{
    public class FiniteStateMachine
    {
        private readonly IMemoryAPI _fface;
        private readonly TypeCache<bool> _cache = new TypeCache<bool>();
        private CancellationTokenSource _cancellation = new CancellationTokenSource();
        private readonly List<IState> _states = new List<IState>();

        public FiniteStateMachine(IMemoryAPI fface)
        {
            _fface = fface;
            //Create the states
            AddState(new DeadState(fface) { Priority = 7 });
            AddState(new ZoneState(fface) { Priority = 7 });
            AddState(new SetTargetState(fface) { Priority = 7 });
            AddState(new SetFightingState(fface) { Priority = 7 });
            AddState(new FollowState(fface) { Priority = 5 });
            AddState(new RestState(fface) { Priority = 2 });
            AddState(new SummonTrustsState(fface) { Priority = 6 });
            AddState(new ApproachState(fface) { Priority = 0 });
            AddState(new BattleState(fface) { Priority = 3 });
            AddState(new WeaponskillState(fface) { Priority = 2 });
            AddState(new PullState(fface) { Priority = 4 });
            AddState(new StartState(fface) { Priority = 5 });
            AddState(new TravelState(fface, new ConfigFactory()) { Priority = 1 });
            AddState(new HealingState(fface) { Priority = 2 });
            AddState(new EndState(fface) { Priority = 3 });
            AddState(new StartEngineState(fface) { Priority = Constants.MaxPriority });

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
            IState startEngineState = _states.FirstOrDefault(x => x.GetType() == typeof(StartEngineState));
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
                if (!backgroundThread.Join(500))
                {
                    backgroundThread.Interrupt();
                }
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

                    bool isRunnable = mc.Check();

                    // Run last state's exits method.
                    if (_cache[mc] != isRunnable)
                    {
                        if (isRunnable) mc.Enter();
                        else mc.Exit();
                        _cache[mc] = isRunnable;
                    }

                    if (isRunnable)
                    {
                        // Run current state's run method.
                        mc.Run();
                    }
                }

                TimeWaiter.Pause(250);
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}