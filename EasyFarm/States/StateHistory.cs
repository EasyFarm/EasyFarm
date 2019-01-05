using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyFarm.States
{
    public class StateHistory
    {
        private readonly IList<StateLog> _history = new List<StateLog>();

        public void AddCheck(IState state)
        {
            _history.Add(new StateLog(state, StateStatus.Check));
        }

        public void AddEnter(IState state)
        {
            _history.Add(new StateLog(state, StateStatus.Enter));
        }

        public void AddRun(IState state)
        {
            _history.Add(new StateLog(state, StateStatus.Run));
        }

        public void AddExit(IState state)
        {
            _history.Add(new StateLog(state, StateStatus.Exit));
        }

        public bool HasCheck(Type state)
        {
            return _history.Any(x => x.State.GetType() == state && x.Status == StateStatus.Check);
        }
    }
}