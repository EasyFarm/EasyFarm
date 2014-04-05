
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
        private FFACE.PlayerTools PlayerTools;
        private Config Config;
        private TargetData TargetData;
        private UnitService Units;
        private Classes.GameEngine m_gameEngine;

        public PlayerData(ref Classes.GameEngine m_gameEngine)
        {
            this.m_gameEngine = m_gameEngine;

            this.PlayerTools = m_gameEngine.FFInstance.Instance.Player;
            this.Config = m_gameEngine.Config;
            this.TargetData = m_gameEngine.TargetData;
            this.Units = m_gameEngine.Units;
        }

        /// <summary>
        /// Is our players status equal to dead?
        /// </summary>
        public bool IsDead
        {
            get
            {
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

        public bool IsHPLow
        {
            get
            {
                return PlayerTools.HPPCurrent <= Config.RestingInfo.LowHP;
            }
        }

        public bool IsMPLow
        {
            get
            {
                return PlayerTools.MPPCurrent <= Config.RestingInfo.LowMP;
            }
        }

        public bool NeedsMoreHP
        {
            get
            {
                return IsResting && PlayerTools.HPPCurrent < Config.RestingInfo.HighHP;
            }
        }

        public bool NeedsMoreMP
        {
            get
            {
                return IsResting && PlayerTools.MPPCurrent < Config.RestingInfo.HighMP;
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
                return Config.RestingInfo.IsRestingHPEnabled;
            }
        }

        public bool IsMPRestingEnabled
        {
            get
            {
                return Config.RestingInfo.IsRestingMPEnabled;
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
                return PlayerTools.TPCurrent >= 100 &&
                                    TargetData.TargetUnit.HPPCurrent <= Config.WeaponInfo.WeaponSkill.HPTrigger &&
                                    IsFighting && TargetData.TargetUnit.Distance < Config.WeaponInfo.WeaponSkill.DistanceTrigger &&
                                    Config.WeaponInfo.WeaponSkill.Ability.IsValidName;
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
                return PlayerTools.HPPCurrent <= Config.RestingInfo.LowHP;
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
                return PlayerTools.Status == Status.Fighting;
            }
        }
    }
}
