
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

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
using ZeroLimits.XITools;

namespace EasyFarm.State
{
    public class PostBattle : BaseState
    {
        public PostBattle(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return ftools.TargetData.TargetUnit != null && ftools
                .TargetData.IsDead;
        }

        public override void EnterState() { }

        public override void RunState()
        {                      
            // Cast all spells making sure they land. Keeping  
            ftools.AbilityExecutor.EnsureSpellsCast(ftools.TargetData.TargetUnit, ftools.PlayerActions.EndList, 
                Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN, 0);

            // Get the next target.
            var target = ftools.UnitService.GetTarget(UnitFilters.MobFilter(fface), x => x.Distance);            

            // Set our new target at the end so that we don't accidentaly cast on a 
            // new target. 
            ftools.TargetData.TargetUnit = target;

            // Set to false in order to use starting moves again in the 
            // attack state. 
            AttackState.fightStarted = false;

        }

        public override void ExitState() { }
    }
}
