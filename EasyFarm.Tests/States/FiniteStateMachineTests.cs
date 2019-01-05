using System.Collections.Generic;
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
            StartState startState = new StartState();
            sut.AddState(startState);
            // Exercise system
            IList<IState> result = sut.Run();
            // Verify outcome
            Assert.Contains(startState, result);
            // Teardown	
        }
    }

    public class NewFiniteStateMachine
    {
        private readonly IList<IState> _states = new List<IState>();

        public void AddState(IState state)
        {
            _states.Add(state);
        }

        public IList<IState> Run()
        {
            return _states;
        }
    }
}
