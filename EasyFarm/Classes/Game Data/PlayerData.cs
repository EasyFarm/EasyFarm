
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
        private GameEngine _engine;

        public PlayerData(ref GameEngine m_gameEngine)
        {
            this._engine = m_gameEngine;
        }

        /// <summary>
        /// Is our players status equal to dead?
        /// </summary>
        public bool IsDead
        {
            get
            {
                var PlayerTools = _engine.Session.Instance.Player;
                return PlayerTools.Status == Status.Dead1 ||
                    PlayerTools.Status == Status.Dead2;
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
                return !_engine.Units.HasAggro && (shouldRestForHP || shouldRestForMP);
            }
        }


        /// <summary>
        /// Should we heal by way of abilities and spells?
        /// </summary>
        public bool shouldHeal
        {
            get
            {
                return _engine.IsWorking && !IsDead && _engine.PlayerActions.HasHealingMoves;
            }
        }


        /// <summary>
        /// Should we move to the next waypoint?
        /// </summary>
        public bool shouldTravel
        {
            get
            {
                return _engine.UserSettings.Waypoints.Count > 0 && !shouldFight && !shouldRest && !shouldHeal && !_engine.ActionBlocked.IsUnable;
            }
        }


        public bool shouldFight
        {
            get
            {
                // Can we battle the unit?
                bool IsAttackable = _engine.TargetData.IsValid;
                // Should we attack?
                bool IsAttacking = !IsDead && (IsAttackable && (IsFighting || IsAggroed || (IsFighting || !shouldRest)));


                return IsAttacking && _engine.IsWorking;
            }
        }


        public bool IsHPLow
        {
            get
            {
                var PlayerTools = _engine.Session.Instance.Player;
                var Config = _engine.UserSettings;
                return PlayerTools.HPPCurrent < Config.RestingInfo.Health.Low;
            }
        }

        public bool IsMPLow
        {
            get
            {
                var PlayerTools = _engine.Session.Instance.Player;
                var Config = _engine.UserSettings;
                return PlayerTools.MPPCurrent < Config.RestingInfo.Magic.Low;
            }
        }

        public bool NeedsMoreHP
        {
            get
            {
                var PlayerTools = _engine.Session.Instance.Player;
                var Config = _engine.UserSettings;
                return IsResting && PlayerTools.HPPCurrent < Config.RestingInfo.Health.High;
            }
        }

        public bool NeedsMoreMP
        {
            get
            {
                var PlayerTools = _engine.Session.Instance.Player;
                var Config = _engine.UserSettings;
                return IsResting && PlayerTools.MPPCurrent < Config.RestingInfo.Magic.High;
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
                var Config = _engine.UserSettings;
                return Config.RestingInfo.Health.Enabled;
            }
        }

        public bool IsMPRestingEnabled
        {
            get
            {
                var Config = _engine.UserSettings;
                return Config.RestingInfo.Magic.Enabled;
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
                var PlayerTools = _engine.Session.Instance.Player;
                return PlayerTools.HPCurrent > 0;
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
                var PlayerTools = _engine.Session.Instance.Player;
                return PlayerTools.Status == Status.Healing;
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
                var PlayerTools = _engine.Session.Instance.Player;
                return PlayerTools.Status == Status.Fighting;
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
                var TargetData = _engine.TargetData;
                var PlayerTools = _engine.Session.Instance.Player;
                var Config = _engine.UserSettings;
                return PlayerTools.TPCurrent >= 1000 &&
                                    TargetData.TargetUnit.HPPCurrent <= Config.WeaponInfo.Health &&
                                    IsFighting && TargetData.TargetUnit.Distance < Config.WeaponInfo.Distance &&
                                    Config.WeaponInfo.Ability.IsValidName;
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
                var PlayerTools = _engine.Session.Instance.Player;
                var Config = _engine.UserSettings;
                return PlayerTools.HPPCurrent <= Config.RestingInfo.Health.Low;
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
                var Units = _engine.Units;
                return Units.HasAggro;
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
                var PlayerTools = _engine.Session.Instance.Player;
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

                return RestBlockingDebuffs.Intersect(PlayerTools.StatusEffects).Count() != 0;
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
                var PlayerTools = _engine.Session.Instance.Player;
                return PlayerTools.Status == Status.Fighting;
            }
        }
    }
}
