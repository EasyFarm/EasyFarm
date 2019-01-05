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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EasyFarm.Context;
using EasyFarm.States;

namespace EasyFarm.Tests.States
{
    public class NewFiniteStateMachine
    {
        private readonly List<IState> _states = new List<IState>();
        private readonly TypeCache<bool> _cache = new TypeCache<bool>();
        private readonly StateHistory _history = new StateHistory();

        public bool IsDryRun { get; set; }

        public void AddState(IState state)
        {
            state.Enabled = true;
            _states.Add(state);
        }

        public StateHistory Run(IGameContext context, CancellationTokenSource cancellation)
        {
            // Sort the List, States may have updated Priorities.
            _states.Sort();

            // Find a State that says it needs to run.
            foreach (var mc in _states.Where(x => x.Enabled).ToList())
            {
                cancellation.Token.ThrowIfCancellationRequested();

                bool isRunnable = mc.Check(context);
                _history.AddCheck(mc);

                // Run last state's exits method.
                if (_cache[mc] != isRunnable)
                {
                    if (isRunnable)
                    {
                        if (!IsDryRun) mc.Enter(context);
                        _history.AddEnter(mc);
                    }
                    else
                    {
                        if (!IsDryRun) mc.Exit(context);
                        _history.AddExit(mc);
                    } 
                    _cache[mc] = isRunnable;
                }

                if (isRunnable)
                {
                    if (!IsDryRun) mc.Run(context);
                    _history.AddRun(mc);
                }
            }

            return _history;
        }
    }
}