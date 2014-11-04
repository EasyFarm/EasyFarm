
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

using EasyFarm.FarmingTool;
using FFACETools;
using System;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool.Classes;
using System.Linq;
using EasyFarm.ViewModels;
using EasyFarm.UserSettings;

namespace EasyFarm.State
{
    public class PostBattle : BaseState
    {
        public PostBattle(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            if(AttackState.TargetUnit == null) return true;

            if (AttackState.TargetUnit.Status.Equals(Status.Dead1 | Status.Dead2)) return true;
            
            return false;
        }

        public override void EnterState() { }

        public override void RunState()
        {
            // Only execute post battle moves when we have a non-null target.
            if (AttackState.TargetUnit != null)
            {
                var UsableEndingMoves = Config.Instance.ActionInfo.StartList
                    .Where(x => ActionFilters.AbilityFilter(FFACE)(x))
                    .ToList();

                // Cast all spells making sure they land. Keeping  
                ftools.AbilityExecutor.EnsureSpellsCast(AttackState.TargetUnit, UsableEndingMoves,
                    Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN, 0);
            }

            // Get the next target.
            var target = ftools.UnitService.GetTarget(UnitFilters.MobFilter(FFACE), x => x.Distance);            

            // Set our new target at the end so that we don't accidentaly cast on a 
            // new target. 
            AttackState.TargetUnit = target;

            // Set to false in order to use starting moves again in the 
            // attack state. 
            AttackState.fightStarted = false;

        }

        public override void ExitState() { }
    }
}
