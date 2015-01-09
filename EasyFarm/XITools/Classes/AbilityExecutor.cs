
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZeroLimits.XITool.Enums;

namespace ZeroLimits.XITool.Classes
{
    /// <summary>
    /// Hold methods that are used for casting spells / abilities. 
    /// Spell / ability validation, recast time checks and casting methods implemented here.
    /// </summary>
    public class AbilityExecutor
    {
        private FFACE m_fface;

        private short m_priorPercentEx;

        private CombatService m_combatService;

        private ActionBlocked m_actionBlocked;


        public static int CastLatency { get; set; }

        public static int GlobalCooldown { get; set; }

        public int RecastAttempts { get; set; }

        public double RecastDelay { get; set; }

        static AbilityExecutor()
        {
            CastLatency = Constants.SPELL_CAST_LATENCY;
            GlobalCooldown = Constants.GLOBAL_SPELL_COOLDOWN;
        }

        public AbilityExecutor(FFACE fface)
        {
            this.m_fface = fface;
            this.m_combatService = new CombatService(fface);
            this.m_actionBlocked = new ActionBlocked(fface);

            this.RecastAttempts = EnsureCastConstants.SPELL_RECAST_ATTEMPTS;
            this.RecastDelay = EnsureCastConstants.SPELL_RECAST_DELAY;
        }

        /// <summary>
        /// Tries to cast spells but does not ensure the succeed. 
        /// </summary>
        /// <param name="target">
        /// The Target use the spells on. 
        /// </param>
        /// <param name="actions">
        /// The list of spells to use. 
        /// </param>
        /// <param name="spellCastLatency">
        /// Time to wait for spells to start casting. The more laggy the server
        /// the higher this value should be. 
        /// </param>
        /// <param name="globalSpellCoolDown">
        /// The time to wait after each spell is casted before another spell may 
        /// be casted.
        /// </param>
        public void ExecuteActions(IList<Ability> actions)
        {
            // Try to cast all spells / abilities. 
            foreach (var action in actions)
            {
                // Use the spell / ability
                UseAbility(action);

                // Sleep global cooldown if its not the last action. 
                if (actions.IndexOf(action) < actions.Count - 1) Thread.Sleep(GlobalCooldown);
            }
        }

        /// <summary>
        /// Ensures the casting of all spells. 
        /// </summary>
        /// <param name="target">
        /// The target we are using the moves on. 
        /// </param>
        /// <param name="actions">
        /// List of moves to execute
        /// </param>
        /// <param name="spellCastLatency">
        /// Time to wait for spells to start casting. The more laggy the server
        /// the higher this value should be. 
        /// </param>
        /// <param name="globalSpellCoolDown">
        /// The time to wait after each spell is casted before another spell may 
        /// be casted.
        /// </param>
        /// <param name="spellRecastDelay">
        /// How long to wait before casting a spell that has failed to cast. 
        /// </param>
        public void EnsureSpellsCast(List<Ability> actions)
        {
            // Contains the moves for casting. DateTime field prevents 
            Dictionary<Ability, DateTime> castable = new Dictionary<Ability, DateTime>();

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
            while (castable.Count > 0 && recastCount++ < RecastAttempts)
            {
                // Loop through all remaining abilities. 
                foreach (var action in castable.Keys)
                {
                    // If we don't meet the mp/tp/recast requirements don't process the action. 
                    // If we did we'd be adding unneccessary wait time.
                    if (!this.IsActionValid(action)) continue;

                    // Continue looping if we can't cast the spell. 
                    if (DateTime.Now <= castable[action]) continue;

                    // Cast the spell. 1 second wait for spells to start casting
                    bool success = this.UseAbility(action);

                    //  5 seconds wait after cast but skip the wait on the last action. 
                    if (castable.Count > 1) Thread.Sleep(GlobalCooldown);

                    // On failure add action to updates for recasting.  
                    if (!success)
                    {
                        // Wait for three seconds for next attempt.
                        var waitPeriod = DateTime.Now.AddSeconds(RecastDelay);

                        // If the action already queued for update just reassign its time used. 
                        if (updates.ContainsKey(action)) updates[action] = waitPeriod;

                        // Add action to updates list for reuse. 
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

        /// <summary>
        /// Uses an ability and returns whether it suceeded or not. 
        /// </summary>
        /// <param name="ability">
        /// The move to use. 
        /// </param>
        /// <param name="spellCastLatency">
        /// Time to wait for spells to start casting. The more laggy the server
        /// the higher this value should be. 
        /// </param>
        /// <param name="globalSpellCoolDown">
        /// The time to wait after each spell is casted before another spell may 
        /// be casted.
        /// </param>
        /// <returns>True on success</returns>
        public bool UseAbility(Ability ability)
        {
            bool success = false;

            // If the ability can't be used
            if (!IsActionValid(ability)) return false;

            // Stop the bot from running so that we can cast. 
            m_fface.Navigator.Reset();

            if (ability.ActionType.Equals(ActionType.Spell))
            {
                // Casts a spell and determines if it casted correctly. 
                success = CastSpell(ability);
            }
            else if (ability.ActionType.Equals(ActionType.Item))
            {
                success = UseItem(ability);
            }
            else
            {
                // Cast the ability and assume it worked... 
                // Ranged attacks could fail here. 
                m_fface.Windower.SendString(ability.ToString());
                success = true;
            }

            return success;
        }

        private bool UseItem(Ability ability)
        {
            var ID = FFACE.ParseResources.GetItemID(ability.Name);
            var Count = m_fface.Item.GetItemCount(ID, InventoryType.Inventory);
            if (Count <= 0) return false;
            var Item = m_fface.Item.GetItem(ID, InventoryType.Inventory);

            return true;
        }

        /// <summary>
        /// Casts a spell and monitors the spell's percent to see 
        /// if it casted properly. 
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public bool CastSpell(Ability spell)
        {
            var success = false;

            // If the ability can't be used
            if (!IsActionValid(spell)) return false;

            // Stop the bot from running so that we can cast. 
            m_fface.Navigator.Reset();

            // Send it to the game
            m_fface.Windower.SendString(spell.ToString());

            Thread.Sleep(CastLatency);

            // While we haven't been interrupted or we haven't completed casting. 
            while (true)
            {
                // We've completed the cast.
                if (m_fface.Player.CastPercentEx == 100)
                {
                    success = true;
                    break;
                }

                // We've been interrupted. 
                if (m_fface.Player.CastPercentEx == m_priorPercentEx)
                {
                    success = false;
                    break;
                }

                // Set prior cast to castcountdown.
                m_priorPercentEx = m_fface.Player.CastPercentEx;

                // Needed for correct testing of CastPercentEx == Prior
                Thread.Sleep(50);
            }

            Thread.Sleep(GlobalCooldown);

            m_priorPercentEx = -1;

            return success;
        }

        /// <summary>
        /// Checks to  see if we can cast/use 
        /// a job ability or spell.
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool IsRecastable(Ability ability)
        {
            int recast = -1;

            // If a spell get spell recast
            if (ability.ActionType == ActionType.Spell) recast =
                m_fface.Timer.GetSpellRecast((SpellList)ability.Index);

            // if ability get ability recast. 
            if (ability.ActionType == ActionType.Ability) recast =
                m_fface.Timer.GetAbilityRecast((AbilityList)ability.Index);

            //Fix: If the action is a ranged attack, 
            // it will return something even when it's recastable. 
            if (ability.ActionType.Equals(ActionType.Ranged))
            {
                return true;
            }

            /*
             * Fixed bug: recast for weaponskills returns -1 not zero. 
             * Check for <= to zero instead of strictly == zero. 
             */
            return recast <= 0;
        }

        /// <summary>
        /// Returns the list of usable abilities
        /// </summary>
        /// <param name="Actions"></param>
        /// <returns></returns>
        public List<Ability> FilterValidActions(IList<Ability> Actions)
        {
            return Actions.Where(x => IsActionValid(x)).ToList();
        }

        /// <summary>
        /// Determines whether a spell or ability can be used based on...
        /// 1) It retrieved a non-null ability/spell from the resource files.
        /// 2) The ability is recastable.
        /// 3) The user has the mp or tp for the move.
        /// 4) We don't have a debuff like amnesia that stops us from using it. 
        /// </summary>
        /// <param name="action"></param>
        /// <returns>True for usable, False for unusable</returns>
        public bool IsActionValid(Ability action)
        {
            // Ability valid check
            if (!action.IsValidName) return false;

            // Recast Check
            if (!IsRecastable(action)) return false;

            // MP Check
            if (action.MPCost > m_fface.Player.MPCurrent) return false;

            // TP Check
            if (action.TPCost > m_fface.Player.TPCurrent) return false;

            // Determine whether we have an debuff that blocks us from casting spells. 
            if (action.ActionType.Equals(ActionType.Spell))
            {
                if (m_actionBlocked.AreSpellsBlocked) return false;
            }

            // Determines if we have a debuff that blocks us from casting abilities. 
            if (action.ActionType.Equals(ActionType.Ability))
            {
                if (m_actionBlocked.AreAbilitiesBlocked) return false;
            }

            return true;
        }
    }
}
