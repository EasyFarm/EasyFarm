
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
using EasyFarm.Logging;

namespace EasyFarm.States
{
    [StateAttribute(priority: 2)]
    public class PostBattle : BaseState
    {
        public PostBattle(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return AttackState.fightStarted && AttackState.TargetUnit != null && AttackState.TargetUnit.IsDead;
        }

        public override void EnterState() { }

        public override void RunState()
        {
            if (AttackState.fightStarted)
            {
                var UsableEndingMoves = Config.Instance.ActionInfo.StartList
                    .Where(x => ActionFilters.AbilityFilter(FFACE)(x))
                    .ToList();

                // Cast all spells making sure they land. Keeping  
                ftools.AbilityExecutor.EnsureSpellsCast(UsableEndingMoves);
            }
           
            // Set to false in order to use starting moves again in the 
            // attack state. 
            AttackState.fightStarted = false;
        }

        public override void ExitState() 
        { 

        }
    }
}
