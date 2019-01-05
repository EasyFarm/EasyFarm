using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EasyFarm.Context;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class FiniteStateMachineTests
    {
        [Fact]
        public void OnFirstRun_StartStateWillRun()
        {
            // Fixture setup
            TestContext context = new TestContext();
            NewFiniteStateMachine sut = new NewFiniteStateMachine();
            StartEngineState state = new StartEngineState() {Enabled = true};
            sut.AddState(state);
            // Exercise system
            var result = sut.Run(context, new CancellationTokenSource());
            // Verify outcome
            Assert.Equal(StateStatus.Check, result.First().Status);
            // Teardown	
        }
    }

    public class StateLog
    {
        public StateLog(IState state, StateStatus status)
        {
            State = state;
            Status = status;
        }

        public StateStatus Status { get; }
        public IState State { get; }
    }

    public class NewFiniteStateMachine
    {
        private readonly List<IState> _states = new List<IState>();
        private readonly TypeCache<bool> _cache = new TypeCache<bool>();

        public void AddState(IState state)
        {
            _states.Add(state);
        }

        public List<StateLog> Run(IGameContext context, CancellationTokenSource cancellation)
        {
            IList<StateLog> history = new List<StateLog>();

            // Sort the List, States may have updated Priorities.
            _states.Sort();

            // Find a State that says it needs to run.
            foreach (var mc in _states.Where(x => x.Enabled).ToList())
            {
                cancellation.Token.ThrowIfCancellationRequested();

                bool isRunnable = mc.Check(context);
                history.Add(new StateLog(mc, StateStatus.Check));

                // Run last state's exits method.
                if (_cache[mc] != isRunnable)
                {
                    if (isRunnable)
                    {
                        mc.Enter(context);
                        history.Add(new StateLog(mc, StateStatus.Enter));
                    }
                    else
                    {
                        mc.Exit(context);
                        history.Add(new StateLog(mc, StateStatus.Exit));
                    } 
                    _cache[mc] = isRunnable;
                }

                if (isRunnable)
                {
                    mc.Run(context);
                    history.Add(new StateLog(mc, StateStatus.Run));
                }
            }

            return history.ToList();
        }
    }
}
