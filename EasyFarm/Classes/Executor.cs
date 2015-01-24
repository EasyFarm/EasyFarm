
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

using EasyFarm.Components;
using EasyFarm.UserSettings;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyFarm.Classes;
using ZeroLimits.XITool.Classes;
using ZeroLimits.XITool.Enums;

namespace EasyFarm.Classes
{
    public class Executor
    {
        private readonly FFACE FFACE;

        private readonly Caster Caster;

        /// <summary>
        /// Must be set by caller. 
        /// </summary>
        public Unit Target { get; set; }

        public Executor(FFACE fface)
        {
            this.FFACE = fface;
            this.Caster = new Caster(fface);
        }

        /// <summary>
        /// Executes moves without the need for a target. 
        /// </summary>
        /// <param name="actions"></param>
        public void ExecuteBuffs(IEnumerable<BattleAbility> actions)
        {
            foreach (var action in actions.ToList())
            {
                // Cast the spell. 
                Caster.CastSpell(action.Ability);

                // Sleep until a spell is recastable. 
                Thread.Sleep(Config.Instance.GlobalCooldown);
            }
        }

        /// <summary>
        /// Execute actions by 
        /// </summary>
        /// <param name="actions"></param>
        public void ExecuteActions(IEnumerable<BattleAbility> actions)
        {
            // Gaurd against null targets. 
            if (this.Target == null) return;

            foreach (var action in actions.ToList())
            {
                // Move to target if out of distance. 
                if (Target.Distance > action.Distance)
                {
                    // Move to unit at max buff distance. 
                    var oldTolerance = FFACE.Navigator.DistanceTolerance;
                    FFACE.Navigator.DistanceTolerance = action.Distance;
                    FFACE.Navigator.GotoNPC(Target.ID);
                    FFACE.Navigator.DistanceTolerance = action.Distance;
                }

                // Face unit
                FFACE.Navigator.FaceHeading(Target.Position);

                // Target mob if not currently targeted. 
                if (Target.ID != FFACE.Target.ID)
                {
                    FFACE.Target.SetNPCTarget(Target.ID);
                    FFACE.Windower.SendString("/ta <t>");
                }

                if (action.Ability.ActionType == ActionType.Spell)
                {
                    Caster.CastSpell(action.Ability);
                    Thread.Sleep(Config.Instance.GlobalCooldown);
                }
                else
                { 
                    Caster.CastAbility(action.Ability);
                }                    
            }
        }        

        /// <summary>
        /// Executes one ability. 
        /// </summary>
        /// <param name="action"></param>
        public void ExecuteAction(BattleAbility action)
        {
            ExecuteActions(new List<BattleAbility>() { action });
        }

        /// <summary>
        /// Executes one buff. 
        /// </summary>
        /// <param name="action"></param>
        public void ExecuteBuff(BattleAbility action)
        {
            ExecuteBuffs(new List<BattleAbility>() { action });
        }

        /// <summary>
        /// Ensures the spells in the list are cast. 
        /// </summary>
        /// <param name="actions"></param>
        public void EnsureSpellsCast(IEnumerable<Ability> actions, int recastAttempts = 3, double recastDelay = 2)
        {
            // Contains the moves for casting. DateTime field prevents 
            var castable = new Dictionary<Ability, DateTime>();

            // contains the list of moves to update in castables.
            Dictionary<Ability, DateTime> updates = new Dictionary<Ability, DateTime>();

            // contains the list of moves that have been completed and will be deleted
            List<Ability> discards = new List<Ability>();

            int recastCount = 0;

            // Add all starting moves to the castable dictionary. 
            foreach (var action in actions)
            {
                if (!castable.ContainsKey(action)) castable.Add(action, DateTime.Now);
            }

            // Loop until all abilities have been casted. 
            while (castable.Count > 0 && recastCount++ < recastAttempts)
            {
                // Loop through all remaining abilities. 
                foreach (var action in castable.Keys)
                {
                    // If we don't meet the mp/tp/recast requirements don't process the action. 
                    // If we did we'd be adding unneccessary wait time.
                    if (!Helpers.IsActionValid(FFACE, action)) continue;

                    // Continue looping if we can't cast the spell. 
                    if (DateTime.Now <= castable[action]) continue;

                    var success = false;

                    // Cast spells and abilities and return their success. 
                    if (action.ActionType == ActionType.Spell)
                    {
                        success = this.Caster.CastSpell(action);
                    }
                    else
                    {
                        FFACE.Windower.SendString(action.ToString());
                        success = true;
                    }

                    //  5 seconds wait after cast but skip the wait on the last action. 
                    if (castable.Count > 1)
                    {
                        Thread.Sleep(Config.Instance.GlobalCooldown);
                    }

                    // On failure add action to updates for recasting.  
                    if (!success)
                    {
                        var waitPeriod = DateTime.Now.AddSeconds(recastDelay);
                        if (updates.ContainsKey(action)) updates[action] = waitPeriod;
                        else updates.Add(action, waitPeriod);
                    }

                    // Delete actions that have succeeded. 
                    else discards.Add(action);
                }

                // Remove the key and re-add it to update the recast times. 
                foreach (var update in updates)
                {
                    castable.Remove(update.Key);
                    castable.Add(update.Key, update.Value);
                }

                // Remove the key so we can't cast that spell again. 
                foreach (var discard in discards)
                {
                    castable.Remove(discard);
                }
            }
        }
    }
}
