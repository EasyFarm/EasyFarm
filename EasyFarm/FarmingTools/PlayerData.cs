
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

using EasyFarm.Classes.Services;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    /// <summary>
    /// A class that contains the data specific to the player.
    /// </summary>
    public class PlayerData
    {
        private FFACE _fface;

        public PlayerData(FFACE fface)
        {
            this._fface = fface;
        }

        /// <summary>
        /// Is our players status equal to dead?
        /// </summary>
        public bool IsDead
        {
            get
            {
                return _fface.Player.Status == Status.Dead1 ||
                    _fface.Player.Status == Status.Dead2;
            }
        }

        public bool shouldRestForMP
        {
            get
            {
                return IsRestingPossible && (IsMPRestingEnabled && (IsMPLow || NeedsMoreMP));
            }
        }

        public bool shouldRestForHP
        {
            get
            {
                return IsRestingPossible && (IsHPRestingEnabled && (IsHPLow || NeedsMoreHP));
            }
        }

        /// <summary>
        /// Should we rest by way of /heal?
        /// </summary>
        public bool shouldRest
        {
            get
            {
                return !FarmingTools.GetInstance(_fface).UnitService.HasAggro && (shouldRestForHP || shouldRestForMP);
            }
        }


        /// <summary>
        /// Should we heal by way of abilities and spells?
        /// </summary>
        public bool shouldHeal
        {
            get
            {
                return FarmingTools.GetInstance(_fface).GameEngine.IsWorking && !IsDead &&
                    FarmingTools.GetInstance(_fface).PlayerActions.HasHealingMoves;
            }
        }


        /// <summary>
        /// Should we move to the next waypoint?
        /// </summary>
        public bool shouldTravel
        {
            get
            {
                return FarmingTools.GetInstance(_fface).UserSettings.Waypoints.Count > 0 &&
                    !shouldFight && !shouldRest && !shouldHeal &&
                    !FarmingTools.GetInstance(_fface).ActionBlocked.IsUnable;
            }
        }


        public bool shouldFight
        {
            get
            {
                // Can we battle the unit?
                bool IsAttackable = FarmingTools.GetInstance(_fface).TargetData.IsValid;
                // Should we attack?
                bool IsAttacking = !IsDead && (IsAttackable && (IsFighting || IsAggroed || (IsFighting || !shouldRest)));


                return IsAttacking && FarmingTools.GetInstance(_fface).GameEngine.IsWorking;
            }
        }


        public bool IsHPLow
        {
            get
            {
                return _fface.Player.HPPCurrent <
                    FarmingTools.GetInstance(_fface).UserSettings.RestingInfo.Health.Low;
            }
        }

        public bool IsMPLow
        {
            get
            {
                return _fface.Player.MPPCurrent <
                    FarmingTools.GetInstance(_fface).UserSettings.RestingInfo.Magic.Low;
            }
        }

        public bool NeedsMoreHP
        {
            get
            {
                return IsResting && _fface.Player.HPPCurrent <
                    FarmingTools.GetInstance(_fface).UserSettings.RestingInfo.Health.High;
            }
        }

        public bool NeedsMoreMP
        {
            get
            {
                return IsResting && _fface.Player.MPPCurrent <
                    FarmingTools.GetInstance(_fface).UserSettings.RestingInfo.Magic.High;
            }
        }

        public bool IsRestingPossible
        {
            get
            {
                return !IsRestingBlocked && !IsAggroed && !IsFighting && !IsDead;
            }
        }

        public bool IsHPRestingEnabled
        {
            get
            {
                return FarmingTools.GetInstance(_fface).UserSettings.RestingInfo.Health.Enabled;
            }
        }

        public bool IsMPRestingEnabled
        {
            get
            {
                return FarmingTools.GetInstance(_fface).UserSettings.RestingInfo.Magic.Enabled;
            }
        }

        /// <summary>
        /// If we have more than zero hp,
        /// return true
        /// </summary>
        /// <returns></returns>
        public bool HasHitpoints
        {
            get
            {
                return _fface.Player.HPCurrent > 0;
            }
        }

        /// <summary>
        /// Returns true if our player is healing.
        /// </summary>
        /// <returns></returns>
        public bool IsResting
        {
            get
            {
                return FarmingTools.GetInstance(_fface).RestingService.IsResting;
            }
        }

        /// <summary>
        /// Is our status == fighting
        /// </summary>
        /// <returns></returns>
        public bool IsFighting
        {
            get
            {
                return _fface.Player.Status == Status.Fighting;
            }
        }

        /// <summary>
        /// Can we perform our weaponskill on the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool CanWeaponskill
        {
            get
            {
                var WeaponSkillTP = 1000;

                return _fface.Player.TPCurrent >= WeaponSkillTP &&
                    FarmingTools.GetInstance(_fface).TargetData.TargetUnit.HPPCurrent <=
                    FarmingTools.GetInstance(_fface).UserSettings.WeaponInfo.Health &&
                    IsFighting && FarmingTools.GetInstance(_fface).TargetData.TargetUnit.Distance <
                    FarmingTools.GetInstance(_fface).UserSettings.WeaponInfo.Distance &&
                    FarmingTools.GetInstance(_fface).UserSettings.WeaponInfo.Ability.IsValidName;
            }
        }

        /// <summary>
        /// Determines low hp status.
        /// </summary>
        /// <returns></returns>
        public bool IsInjured
        {
            get
            {
                return _fface.Player.HPPCurrent <= 
                    FarmingTools.GetInstance(_fface).UserSettings.RestingInfo.Health.Low;
            }
        }

        /// <summary>
        /// Does our character have aggro
        /// </summary>
        /// <returns></returns>
        public bool IsAggroed
        {
            get
            {
                return FarmingTools.GetInstance(_fface).UnitService.HasAggro;
            }
        }

        /// <summary>
        /// Does our player have a status effect that prevents him
        /// </summary>
        /// <param name="playerStatusEffects"></param>
        /// <returns></returns>
        public bool IsRestingBlocked
        {
            get
            {
                var RestBlockingDebuffs = new List<StatusEffect>() 
            { 
                StatusEffect.Poison, StatusEffect.Bio, StatusEffect.Sleep, 
                StatusEffect.Sleep2, StatusEffect.Poison, StatusEffect.Petrification,
                StatusEffect.Stun, StatusEffect.Charm1, StatusEffect.Charm2, 
                StatusEffect.Terror, StatusEffect.Frost, StatusEffect.Burn, 
                StatusEffect.Choke, StatusEffect.Rasp, StatusEffect.Shock, 
                StatusEffect.Drown, StatusEffect.Dia, StatusEffect.Requiem, 
                StatusEffect.Lullaby
            };

                return RestBlockingDebuffs.Intersect(_fface.Player.StatusEffects).Count() != 0;
            }
        }

        /// <summary>
        /// Returns true if our player is able to
        /// safely rest (/heal).
        /// </summary>
        /// <returns></returns>
        public bool CanPlayerRest
        {
            get
            {
                return (IsInjured && !IsAggroed && HasHitpoints && !IsRestingBlocked);
            }
        }

        /// <summary>
        /// Same as IsFighting. Returns true if we are fighting the target.
        /// </summary>
        /// <returns></returns>
        public bool IsEngaged
        {
            get
            {
                return _fface.Player.Status == Status.Fighting;
            }
        }
    }
}
