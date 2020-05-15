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
using EasyFarm.Parsing;
using EasyFarm.UserSettings;
using MemoryAPI;
using MemoryAPI.Navigation;
using StatusEffect = MemoryAPI.StatusEffect;

namespace EasyFarm.Classes
{   
    public class Executor
    {
        private readonly IMemoryAPI _fface;
        public String LastCommand { get; set; }

        public Executor(IMemoryAPI fface)
        {
            _fface = fface;
        }

        public void UseActions(IEnumerable<BattleAbility> actions)
        {
            if (actions == null) throw new ArgumentNullException(nameof(actions));

            foreach (var action in actions.ToList())
            {
                if (!ActionFilters.BuffingFilter(_fface, action))
                {
                    continue;
                }

                if (!CastSpell(action)) continue;

                action.Usages++;
                action.LastCast = DateTime.Now.AddSeconds(action.Recast);

                TimeWaiter.Pause(Config.Instance.GlobalCooldown);
            }
        }

        public void UseBuffingActions(IEnumerable<BattleAbility> actions)
        {
            if (actions == null) throw new ArgumentNullException(nameof(actions));

            var castables = actions.ToList();

            while (castables.Count > 0)
            {
                foreach (var action in castables.ToList())
                {
                    if (!ActionFilters.BuffingFilter(_fface, action))
                    {
                        castables.Remove(action);
                        continue;
                    }

                    if (!CastSpell(action)) continue;

                    castables.Remove(action);
                    action.Usages++;
                    action.LastCast = DateTime.Now.AddSeconds(action.Recast);

                    TimeWaiter.Pause(Config.Instance.GlobalCooldown);
                }
            }
        }   

        public void UseTargetedActions(EasyFarm.Context.IGameContext context, IEnumerable<BattleAbility> actions, IUnit target)
        {
            if (actions == null) throw new ArgumentNullException(nameof(actions));
            if (target == null) throw new ArgumentNullException(nameof(target));

            foreach (var action in actions)
            {
                var isInRange = MoveIntoActionRange(context, target, action);

                _fface.Navigator.FaceHeading(target.Position);
                Player.SetTarget(_fface, target);

                if (isInRange)
                {

                    _fface.Navigator.Reset();
                    TimeWaiter.Pause(100);

                    if (ResourceHelper.IsSpell(action.AbilityType))
                    {
                        CastSpell(action);
                    }
                    else
                    {
                        CastAbility(action);
                    }

                    action.Usages++;
                    action.LastCast = DateTime.Now.AddSeconds(action.Recast);

                    TimeWaiter.Pause(Config.Instance.GlobalCooldown);
                }
            }
        }

        private bool MoveIntoActionRange(EasyFarm.Context.IGameContext context, IUnit target, BattleAbility action)
        {
            if (target.Distance > action.Distance)
            {
                var path = context.NavMesh.FindPathBetween(context.API.Player.Position, context.Target.Position);
                if (path.Count > 0)
                {
                    if (path.Count > 2)
                    {
                        context.API.Navigator.DistanceTolerance = 1;
                    }
                    else
                    {
                        _fface.Navigator.DistanceTolerance = action.Distance;
                    }

                    while (path.Count > 0 && path.Peek().Distance(context.API.Player.Position) <= _fface.Navigator.DistanceTolerance)
                    {
                        path.Dequeue();
                    }

                    context.API.Navigator.GotoNPC(target.Id, path.Peek(), path.Count > 0);
                }

                return false;
            }

            return true;
        }        

        private bool EnsureCast(string command)
        {            
            var previous = _fface.Player.CastPercentEx;
            var startTime = DateTime.Now;
            var interval = startTime.AddSeconds(3);

            while (DateTime.Now < interval)
            {
                while(Player.Instance.IsMoving)
                {
                    Player.StopRunning(_fface);
                }

                if (_fface.Player.Status == Status.Healing)
                {
                    Player.Stand(_fface);
                }

                if (_fface.Player.StatusEffects.Contains(StatusEffect.Chainspell))
                {
                    _fface.Windower.SendString(command);
                    return true;
                }                             

                if (Math.Abs(previous - _fface.Player.CastPercentEx) > .5) return true;
                SendCommand(command);
                TimeWaiter.Pause(500);
            }

            return false;
        }

        private bool MonitorCast()
        {
            var prior = _fface.Player.CastPercentEx;

            var stopwatch = new Stopwatch();

            while (stopwatch.Elapsed.TotalSeconds < 2)
            {
                if (Math.Abs(prior - _fface.Player.CastPercentEx) < .5)
                {
                    if (!stopwatch.IsRunning) stopwatch.Start();
                }
                else
                {
                    stopwatch.Reset();
                }

                prior = _fface.Player.CastPercentEx;

                TimeWaiter.Pause(100);
            }

            return Math.Abs(prior - 100) < .5;
        }

        private bool CastAbility(BattleAbility ability)
        {
            SendCommand(ability.Command);
            TimeWaiter.Pause(100);
            return true;
        }

        private void SendCommand(String command)
        {
            LastCommand = command;
            _fface.Windower.SendString(command);
        }

        private bool CastSpell(BattleAbility ability)
        {
            if (EnsureCast(ability.Command)) return MonitorCast();
            return false;
        }
    }
}