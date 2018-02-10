// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using EasyFarm.Parsing;
using MemoryAPI;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     This class is responsible for holding all of the abilities our character may use in battle.
    /// </summary>
    public class ActionFilters
    {
        public static bool BuffingFilter(IMemoryAPI fface, BattleAbility action)
        {
            var units = CreateUnitService(fface);
            var results = ValidateBuffingAction(fface, action, units.MobArray).ToList();
            var usable = !results.Any(x => x.IsFailure);
            return usable;
        }        

        /// <summary>
        ///     Filters out unusable buffing abilities.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="action"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        public static IEnumerable<Result> ValidateBuffingAction(
            IMemoryAPI fface,
            BattleAbility action,
            ICollection<IUnit> units)
        {
            // Return if not enabled.
            if (!action.IsEnabled)
                yield return Result.Fail("Must be enabled");

            // Name Check
            if (string.IsNullOrWhiteSpace(action.Name))
                yield return Result.Fail("Must have name");

            // MP Check
            if (action.Ability.MpCost > fface.Player.MPCurrent)
                yield return Result.Fail("Not enough mp");

            // TP Check
            if (action.Ability.TpCost > fface.Player.TPCurrent)
                yield return Result.Fail("Not enough tp");

            // MP Range
            var mpReserve = new Range(action.MPReserveLow, action.MPReserveHigh);
            if (!mpReserve.InRange(fface.Player.MPPCurrent) && !mpReserve.NotSet())
                yield return Result.Fail("Outside mp reserve range");

            // TP Range
            var tpReserve = new Range(action.TPReserveLow, action.TPReserveHigh);
            if (!tpReserve.InRange(fface.Player.TPCurrent) && !tpReserve.NotSet())
                yield return Result.Fail("Outside tp reserve range");

            // Usage Limit Check. 
            if (action.UsageLimit != 0)
            {
                if (action.Usages > action.UsageLimit)
                    yield return Result.Fail("Max uses reached");
            }

            // Recast Check
            if (!AbilityUtils.IsRecastable(fface, action.Ability))
                yield return Result.Fail("Not recastable");

            // Limiting Status Effect Check for Spells. 
            if (ResourceHelper.IsSpell(action.Ability.AbilityType))
            {
                if (ProhibitEffects.ProhibitEffectsSpell.Intersect(fface.Player.StatusEffects).Any())
                {
                    yield return Result.Fail("Status effect blocking spell");
                }
            }

            // Limiting Status Effect Check for Abilities. 
            if (ResourceHelper.IsAbility(action.Ability.AbilityType))
            {
                if (ProhibitEffects.ProhibitEffectsAbility.Intersect(fface.Player.StatusEffects).Any())
                {
                    yield return Result.Fail("Status effect blocking ability");
                }
            }

            // Player HP Checks Enabled. 
            if (action.PlayerLowerHealth != 0 || action.PlayerUpperHealth != 0)
            {
                // Player Upper HP Check
                if (fface.Player.HPPCurrent > action.PlayerUpperHealth)
                    yield return Result.Fail("Player health too high");

                // Player Lower HP Check
                if (fface.Player.HPPCurrent < action.PlayerLowerHealth)
                    yield return Result.Fail("Player health too low");
            }

            // Status Effect Checks Enabled
            if (!string.IsNullOrWhiteSpace(action.StatusEffect))
            {
                var hasEffect = fface.Player.StatusEffects.Any(effect =>
                    Regex.IsMatch(effect.ToString(),
                        action.StatusEffect.Replace(" ", "_"),
                        RegexOptions.IgnoreCase));

                // Contains Effect Check
                if (hasEffect && !action.TriggerOnEffectPresent)
                    yield return Result.Fail("Player missing status effect");

                // Missing EFfect Check
                if (!hasEffect && action.TriggerOnEffectPresent)
                    yield return Result.Fail("Player contains status effect");
            }

            // Check if action's recast period has passed.
            if (action.Recast != 0)
            {
                if (action.LastCast > DateTime.Now)
                    yield return Result.Fail("Recast period not reached");
            }

            // Do not cast trust magic with aggro.
            var isTrustMagic = action.Ability.Type == "Trust";
            var hasAggro = units.Any(x => x.HasAggroed);
            if (isTrustMagic && hasAggro)
                yield return Result.Fail("Cannot use trust magic with aggro");
        }

        /// <summary>
        ///     Filters out unusable targeted abilities.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="action"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool TargetedFilter(IMemoryAPI fface, BattleAbility action, IUnit unit)
        {
            // Does not pass the base criteria for casting.
            if (!BuffingFilter(fface, action)) return false;

            // Target HP Checks Enabled.
            if (action.TargetLowerHealth != 0 || action.TargetUpperHealth != 0)
            {
                // Target Upper Health Check
                if (unit.HppCurrent > action.TargetUpperHealth) return false;

                // Target Lower Health Check
                if (unit.HppCurrent < action.TargetLowerHealth) return false;
            }

            // Target Name Checks Enabled.
            if (!string.IsNullOrWhiteSpace(action.TargetName))
            {
                // Target Name Check.
                if (!Regex.IsMatch(unit.Name, action.TargetName, RegexOptions.IgnoreCase)) return false;
            }

            return true;
        }

        private static IUnitService CreateUnitService(IMemoryAPI fface)
        {
            return GlobalFactory.CreateUnitService(fface);
        }
    }
}
