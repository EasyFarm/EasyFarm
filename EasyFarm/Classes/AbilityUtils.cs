/*///////////////////////////////////////////////////////////////////
Copyright (C) <Zerolimits>

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
using System.Text.RegularExpressions;
using Parsing.Abilities;
using Parsing.Types;
using MemoryAPI;

namespace EasyFarm.Classes
{
    public class AbilityUtils
    {
        /// <summary>
        ///     Checks whether a spell or ability can be recasted.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        public static bool IsRecastable(FFACE fface, Ability ability)
        {
            var recast = -1;

            /* 
             * Fix: If the action is a ranged attack,                
             * it will return something even when it's recastable. 
             *                
             * This if statement must be above process abilities since               
             * AbilityType.Range is in AbilityType.IsAbility
             */
            if (AbilityType.Range.HasFlag(ability.AbilityType))
            {
                return true;
            }

            // If a spell get spell recast
            if (CompositeAbilityTypes.IsSpell.HasFlag(ability.AbilityType))
            {
                recast = fface.Timer.GetSpellRecast(ToSpellList(ability));
            }

            // if ability get ability recast. 
            if (CompositeAbilityTypes.IsAbility.HasFlag(ability.AbilityType))
            {
                recast = fface.Timer.GetAbilityRecast(ToAbilityList(ability));
            }

            /*
             * Fixed bug: recast for weaponskills returns -1 not zero. 
             * Check for <= to zero instead of strictly == zero. 
             */
            return recast <= 0;
        }

        /// <summary>
        ///     Adjusts the name of abilities to fit the
        ///     ablity and spell list enum format.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string AdjustName(string name)
        {
            // Strip all characters that are not words of the 
            // ability's name and convert spaces to underscores. 
            return Regex.Replace(name, @"([^a-zA-Z ])", "")
                .Replace(" ", "_");
        }

        private static AbilityList ToAbilityList(Ability ability)
        {
            var name = AdjustName(ability.English);

            AbilityList value;

            // Fixes for summoner's blood pact recast times. 
            if (ability.CategoryType.HasFlag(CategoryType.BloodPactWard))
                return AbilityList.Blood_Pact_Ward;
            if (ability.CategoryType.HasFlag(CategoryType.BloodPactRage))
                return AbilityList.Blood_Pact_Rage;

            Enum.TryParse(name, out value);

            return value;
        }

        private static SpellList ToSpellList(Ability ability)
        {
            var name = AdjustName(ability.English);
            SpellList value;
            Enum.TryParse(name, out value);
            return value;
        }
    }
}