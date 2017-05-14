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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EasyFarm.Parsing;
using MemoryAPI;
using StatusEffect = MemoryAPI.StatusEffect;

namespace EasyFarm.Classes
{   
    public class Executor
    {
        private readonly IMemoryAPI _fface;

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

        public void UseTargetedActions(IEnumerable<BattleAbility> actions, IUnit target)
        {
            if (actions == null) throw new ArgumentNullException(nameof(actions));
            if (target == null) throw new ArgumentNullException(nameof(target));

            foreach (var action in actions)
            {
                MoveIntoActionRange(target, action);
                _fface.Navigator.FaceHeading(target.Position);
                Player.SetTarget(_fface, target);

                _fface.Navigator.Reset();
                TimeWaiter.Pause(100);

                if (ResourceHelper.IsSpell(action.Ability.AbilityType))
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

        private void MoveIntoActionRange(IUnit target, BattleAbility action)
        {
            if (target.Distance > action.Distance)
            {
                _fface.Navigator.DistanceTolerance = action.Distance;
                _fface.Navigator.GotoNPC(target.Id, Config.Instance.IsObjectAvoidanceEnabled);
            }
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
                _fface.Windower.SendString(command);
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
            _fface.Windower.SendString(ability.Command);
            TimeWaiter.Pause(100);
            return true;
        }

        private bool CastSpell(BattleAbility ability)
        {
            if (EnsureCast(ability.Command)) return MonitorCast();
            return false;
        }
    }
}