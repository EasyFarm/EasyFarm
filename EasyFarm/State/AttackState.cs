
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


using FFACETools;
using System.Threading;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITools;
using System.Linq;

namespace EasyFarm.State
{
    /// <summary>
    /// A class for defeating monsters.
    /// </summary>
    class AttackState : BaseState
    {
        public static bool fightStarted = false;

        public AttackState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return ftools.PlayerData.shouldFight;
        }

        public override void EnterState()
        {
            ftools.RestingService.EndResting();
        }

        public override void RunState()
        {
            // Get the target
            var target = ftools.TargetData.TargetUnit;

            /* 
             * I've replaced the for loops with the linq queries.
             * It combines the two lists and then checks for the requirements the same as before. 
             */ 
            
            // True when there exists an ability that is an offensive spell.
            bool offensiveInLists = ftools.PlayerActions.StartList
                .Union(ftools.PlayerActions.PullList)
                .Any(x => x.IsSpell && x.Postfix == "<t>");
            
            // True when there exists an ability that is a ranged attack. 
            bool rangedInLists = ftools.PlayerActions.StartList
                .Union(ftools.PlayerActions.PullList)
                .Any(x => x.Prefix == "/range");

            // The our new pull distance determined by what moves 
            // we have in the list.
            double startPullDistance;

            // If we have a spell in the lists, set pull distance to spell distance. 
            if (offensiveInLists) { startPullDistance = Constants.SPELL_CAST_DISTANCE; }
            // If we have a ranged attack in the lists, set pull distance to ranged distance. 
            else if (rangedInLists) { startPullDistance = Constants.RANGED_ATTACK_MAX_DISTANCE; }
            // Set pull distance to target's distance. 
            else { startPullDistance = fface.Navigator.DistanceTo(target.Position); }

            // If the target is out of range move into range.
            if (fface.Navigator.DistanceTo(target.Position) > startPullDistance)
            {
                // Save old tolerance
                var old = fface.Navigator.DistanceTolerance;

                // Set to max engagement distance.
                fface.Navigator.DistanceTolerance = startPullDistance;

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

            /* 
             * Cast the starting moves. Used to buff the character. 
             * Will cast until all spells are successfully casted.  
             *
             * IsDead check added to prevent casting protect when the mobs dies and 
             * PostBattle state sets fightStarted back to true. *
             */

            if (!fightStarted && !target.IsDead)
            {
                ftools.AbilityExecutor.EnsureSpellsCast(target, ftools.PlayerActions.StartList,
                    Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN, 0);
            }

            // set to true so that we do not cast starting spells again. 
            fightStarted = true;

            // If the pull list is not empty and we have a ranged or offensive move to use (prefix = <t>)
            if (ftools.PlayerActions.PullList.Count > 0 && (rangedInLists|| offensiveInLists))
            {
                // Pull the target casting each spell once until the target is claimed
                while (!target.Status.Equals(Status.Fighting) && !target.IsDead)
                {
                    // If out of range, stop trying to pull
                    if (fface.Navigator.DistanceTo(target.Position) > startPullDistance) { return; }
                    else
                    {
                        ftools.AbilityExecutor.ExecuteActions(target, ftools.PlayerActions.PullList,
                            Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN);
                    }
                }
            }

            else if (!target.Status.Equals(Status.Fighting))
            {
                ftools.AbilityExecutor.ExecuteActions(target, ftools.PlayerActions.PullList, 
                    Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN);
            }

            // *Removed: target does not have to be engaged for us to fight it. *
            // Check pull
            // if (!target.Status.Equals(Status.Fighting)) return;

            // Engage the target
            if (!fface.Player.Status.Equals(Status.Fighting))
            {
                ftools.CombatService.Engage();
            }

            // Check engaged
            if (!fface.Player.Status.Equals(Status.Fighting)) return;

            // Move to the target
            /*
             * Fixed bug that used Constants.MeleeDistance instead of the user 
             * settings for distance tolerance. 
             * 
             * Fixed bug were the old tolerance was being overwritten by the new tolerance
             * before it was properly saved. 
             */
            if (fface.Navigator.DistanceTo(target.Position) > ftools.UserSettings.MiscSettings.MeleeDistance)
            {
                var old = fface.Navigator.DistanceTolerance;
                fface.Navigator.DistanceTolerance = ftools.UserSettings.MiscSettings.MeleeDistance;
                fface.Navigator.GotoNPC(target.ID);
                fface.Navigator.DistanceTolerance = old;
            }

            // Weaponskill
            if (ftools.PlayerData.CanWeaponskill)
            {
                // Not sure if weapon skills or job abilities endure the same penalties that 
                // spell do in regards to wait times. So I'm using zero's here. 
                ftools.AbilityExecutor.UseAbility(ftools.UserSettings.WeaponInfo.Ability, 0, 0);
            }

            // Cast all battle moves
            ftools.AbilityExecutor.ExecuteActions(target, ftools.PlayerActions.BattleList,
                    Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN);
        }

        public override void ExitState()
        {

        }
    }
}
