
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


using FFACETools;
using System.Threading;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITools;
using System.Linq;
using System.Collections.Generic;
using System;

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

            // True when there exists an ability that is an offensive spell.
            bool offensiveInLists = ftools.PlayerActions.PullList.Any(x => x.IsSpell && x.Postfix == "<t>");

            // True when there exists an ability that is a ranged attack. 
            bool rangedInLists = ftools.PlayerActions.PullList.Any(x => x.Prefix == "/range");

            // List of pulling moves + distances.
            var pullMoves = SetDistances(ftools.PlayerActions.PullList, target);

            // List of start moves + distances. 
            var startMoves = SetDistances(ftools.PlayerActions.StartList, target);

            // Face the target
            fface.Navigator.FaceHeading(target.ID);

            // Target the target
            if (target.ID != fface.Target.ID) ftools.CombatService.TargetUnit(target);

            // Check correct target
            if (target.ID != fface.Target.ID) return;

            /* 
             * Cast the starting moves. Used to buff the character. 
             * Will cast until all spells are successfully casted.  
             *
             * IsDead check added to prevent casting protect when the mobs dies and 
             * PostBattle state sets fightStarted back to true. *
             */

            // Cast only when there is a move ready. 
            if (startMoves.Any(x => ftools.AbilityExecutor.IsActionValid(x.Item1)))
            {
                if (!fightStarted && !target.IsDead)
                {
                    ftools.AbilityExecutor.EnsureSpellsCast(target, startMoves,
                        Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN, 0);
                }
            }

            // set to true so that we do not cast starting spells again. 
            fightStarted = true;

            // Cast only when there is a move ready. 
            if (pullMoves.Any(x => ftools.AbilityExecutor.IsActionValid(x.Item1)))
            {
                // If the pull list is not empty and we have a ranged or offensive move to use (prefix = <t>)
                if (pullMoves.Count > 0 && (rangedInLists || offensiveInLists))
                {
                    // Pull the target casting each spell once until the target is claimed
                    if (!target.Status.Equals(Status.Fighting) && !target.IsDead)
                    {
                        // If out of range, stop trying to pull
                        // if (fface.Navigator.DistanceTo(target.Position) > startPullDistance) return;

                        ftools.AbilityExecutor.ExecuteActions(target, pullMoves, 
                            Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN);
                    }
                }
                else if (!target.Status.Equals(Status.Fighting))
                {
                    ftools.AbilityExecutor.ExecuteActions(target, pullMoves,
                        Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN);
                }
            }

            // Engage the target
            int engageCount = 0;
            while (!fface.Player.Status.Equals(Status.Fighting) && engageCount++ < 3)
            {
                ftools.CombatService.Engage();
            }

            // Check engaged
            if (!fface.Player.Status.Equals(Status.Fighting)) return;

            // Move to the target
            ftools.CombatService.MoveToUnit(target, ftools.UserSettings.MiscSettings.MeleeDistance);

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

        public List<Tuple<Ability, double>> SetDistances(List<Ability> moves, Unit unit)
        {
            var temp = new List<Tuple<Ability, double>>();

            foreach (var action in moves)
            {
                temp.Add(new Tuple<Ability, double>(action, GetApproachDistance(action, unit)));
            }

            return temp;
        }

        public double GetApproachDistance(Ability move, Unit unit)
        {
            // The distance we should approach from. 
            double approach = 0;

            // If we have a spell in the lists, set pull distance to spell distance. 
            if (move.IsSpell) { approach = Constants.SPELL_CAST_DISTANCE; }
            // If we have a ranged attack in the lists, set pull distance to ranged distance. 
            else if (move.Prefix == "/range") { approach = Constants.RANGED_ATTACK_MAX_DISTANCE; }
            // Set pull distance to target's distance. 
            //FIXED: We want to pull at the melee distance on fail, not at the distance 
            // we are currently standing at. 
            else { approach = fface.Navigator.DistanceTo(unit.Position); }

            return approach;
        }

        public override void ExitState() { }
    }
}
