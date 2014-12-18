
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

<<<<<<< HEAD:EasyFarm/Components/PostBattleComponent.cs
namespace EasyFarm.Components
{
    public class PostBattleComponent : BaseComponent
=======
namespace EasyFarm.States
{
    [StateAttribute(priority: 2)]
    public class PostBattle : BaseState
>>>>>>> VM_and_State_AutoLocate_and_AttackState_Refactor:EasyFarm/States/PostBattle.cs
    {
        public PostBattleComponent(FFACE fface) : base(fface) { }

        public override bool CheckComponent()
        {
<<<<<<< HEAD:EasyFarm/Components/PostBattleComponent.cs
            if(AttackComponent.TargetUnit == null) return true;

            if (AttackComponent.TargetUnit.Status.Equals(Status.Dead1 | Status.Dead2)) return true;
            
            return false;
=======
            return AttackState.fightStarted && AttackState.TargetUnit != null && AttackState.TargetUnit.IsDead;
>>>>>>> VM_and_State_AutoLocate_and_AttackState_Refactor:EasyFarm/States/PostBattle.cs
        }

        public override void EnterComponent() { }

        public override void RunComponent()
        {
<<<<<<< HEAD:EasyFarm/Components/PostBattleComponent.cs
            // Only execute post battle moves when we have a non-null target.
            if (AttackComponent.TargetUnit != null)
=======
            if (AttackState.fightStarted)
>>>>>>> VM_and_State_AutoLocate_and_AttackState_Refactor:EasyFarm/States/PostBattle.cs
            {
                var UsableEndingMoves = Config.Instance.ActionInfo.StartList
                    .Where(x => ActionFilters.AbilityFilter(FFACE)(x))
                    .ToList();

                // Cast all spells making sure they land. Keeping  
                ftools.AbilityExecutor.EnsureSpellsCast(UsableEndingMoves);
            }
<<<<<<< HEAD:EasyFarm/Components/PostBattleComponent.cs

            // Get the next target.
            var target = ftools.UnitService.GetTarget(UnitFilters.MobFilter(FFACE), x => x.Distance);            

            // Set our new target at the end so that we don't accidentaly cast on a 
            // new target. 
            AttackComponent.TargetUnit = target;

            // Set to false in order to use starting moves again in the 
            // attack Component. 
            AttackComponent.fightStarted = false;

        }

        public override void ExitComponent() { }
=======
           
            // Set to false in order to use starting moves again in the 
            // attack state. 
            AttackState.fightStarted = false;
        }

        public override void ExitState() 
        { 

        }
>>>>>>> VM_and_State_AutoLocate_and_AttackState_Refactor:EasyFarm/States/PostBattle.cs
    }
}
