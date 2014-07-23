
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
*////////////////////////////////////////////////////////////////////


using FFACETools;
using System.Threading;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITools;

namespace EasyFarm.State
{
    /// <summary>
    /// A class for defeating monsters.
    /// </summary>
    class AttackState : BaseState
    {
        public AttackState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return ftools
                .PlayerData.shouldFight;
        }

        public override void EnterState()
        {
            ftools.RestingService.EndResting();
        }

        public override void RunState()
        {
            // Get the target
            var target = ftools.TargetData.TargetUnit;

            // Ensure third person. 
            fface.Navigator.SetViewMode(ViewMode.ThirdPerson);

            if (fface.Navigator.DistanceTo(target.Position) > 21)
            {
                // Save old tolerance
                var old = fface.Navigator.DistanceTolerance;
                
                // Set to max engagement distance. 
                fface.Navigator.DistanceTolerance = 21;

                // Goto target at max engagement distance.
                fface.Navigator.Goto(target.Position, false);
                
                // Restore old tolerance. 
                fface.Navigator.DistanceTolerance = old;
            }
            
            // Face the target
            fface.Navigator.FaceHeading(target.ID);
            
            // Target the target
            if (target.ID != fface.Target.ID)
                ftools.CombatService.TargetUnit(target);

            // Check correct target
            if (target.ID != fface.Target.ID) return;
            
            // Pull the target casting each spell only once. 
            if (!target.Status.Equals(Status.Fighting)) 
            {
                ftools.AbilityExecutor.ExecuteActions(target, ftools.PlayerActions.StartList, 
                    Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN);
            }

            // Check pull
            if (!target.Status.Equals(Status.Fighting)) return;
            
            // Engage the target
            if (!fface.Player.Status.Equals(Status.Fighting))
            {
                ftools.CombatService.Engage();
            }

            // Check engaged
            if (!fface.Player.Status.Equals(Status.Fighting)) return; 
            
            // Move to the target
            if (fface.Navigator.DistanceTo(target.Position) > Constants.MELEE_DISTANCE)
            {
                fface.Navigator.DistanceTolerance = Constants.MELEE_DISTANCE;
                var old = fface.Navigator.DistanceTolerance;
                fface.Navigator.Goto(target.Position, false);
                fface.Navigator.DistanceTolerance = old;
            }

            // Cast all battle moves
            ftools.AbilityExecutor.ExecuteActions(target, ftools.PlayerActions.BattleList,
                    Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN);

            // Weaponskill
            if (ftools.PlayerData.CanWeaponskill)
            {
                // Not sure if weapon skills or job abilities endure the same penalties that 
                // spell do in regards to wait times. So I'm using zero's here. 
                ftools.AbilityExecutor.UseAbility(ftools.UserSettings.WeaponInfo.Ability, 0, 0);
            }
        }

        public override void ExitState()
        {

        }
    }
}
