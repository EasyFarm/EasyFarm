
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

﻿using EasyFarm.Engine;
using EasyFarm.PlayerTools;
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
        public PlayerData(ref GameEngine Engine)
        {
            this.Engine = Engine;
        }

        /// <summary>
        /// Is our players status equal to dead?
        /// </summary>
        public bool IsDead
        {
            get
            {
                return Engine.FFInstance.Instance.Player.Status == Status.Dead1 ||
                    Engine.FFInstance.Instance.Player.Status == Status.Dead2;
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
                return Engine.FFInstance.Instance.Player.HPPCurrent <= Engine.Config.LowHP;
            }
        }

        public bool IsMPLow
        {
            get
            {
                return Engine.FFInstance.Instance.Player.MPPCurrent <= Engine.Config.LowMP;
            }
        }

        public bool NeedsMoreHP
        {
            get
            {
                return IsResting && Engine.FFInstance.Instance.Player.HPPCurrent < Engine.Config.HighHP;
            }
        }

        public bool NeedsMoreMP
        {
            get
            {
                return IsResting && Engine.FFInstance.Instance.Player.MPPCurrent < Engine.Config.HighMP;
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
                return Engine.Config.IsRestingHPEnabled;
            }
        }

        public bool IsMPRestingEnabled
        {
            get
            {
                return Engine.Config.IsRestingMPEnabled;
            }
        }

        /// <summary>
        /// Should we rest by way of /heal?
        /// </summary>
        public bool shouldRest
        {
            get
            {
                return shouldRestForHP || shouldRestForMP;
            }
        }

        /// <summary>
        /// Should we heal by way of abilities and spells?
        /// </summary>
        public bool shouldHeal
        {
            get
            {
                return Engine.IsWorking && !IsDead && Engine.Combat.HealingList.Count > 0;
            }
        }

        /// <summary>
        /// Should we move to the next waypoint?
        /// </summary>
        public bool shouldTravel
        {
            get
            {
                return Engine.Config.Waypoints.Count > 0 && !shouldFight && !shouldRest && !shouldHeal && !IsUnable;
            }
        }

        public bool shouldFight
        {
            get
            {
                // Can we battle the unit?
                bool IsAttackable = Engine.TargetData.IsUnitBattleReady;
                // Should we attack?
                bool IsAttacking = !IsDead && (IsAttackable && (IsFighting || IsAggroed || (IsFighting || !shouldRest)));

                return IsAttacking && Engine.IsWorking;
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
                return Engine.FFInstance.Instance.Player.HPCurrent > 0;
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
                return Engine.FFInstance.Instance.Player.Status == Status.Healing;
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
                return Engine.FFInstance.Instance.Player.Status == Status.Fighting;
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
                return Engine.FFInstance.Instance.Player.TPCurrent >= 100 &&
                                    Engine.TargetData.TargetUnit.HPPCurrent <= Engine.Config.Weaponskill.HPTrigger &&
                                    IsFighting && Engine.TargetData.TargetUnit.Distance < Engine.Config.Weaponskill.DistanceTrigger &&
                                    Engine.Config.Weaponskill.Ability.IsValidName;
            }
        }

        /// <summary>
        /// Returns true if we can not cast a spell.
        /// </summary>
        /// <returns></returns>
        public bool IsCastingBlocked
        {
            get
            {
                StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Silence,
                StatusEffect.Mute
            };

                // If we have effects that block,
                // return true.
                bool unableToCast = effectsThatBlock
                    .Intersect(this.Engine.FFInstance.Instance.Player.StatusEffects)
                    .Count() != 0;

                // 
                bool unableToReact = IsUnable;

                return unableToCast || unableToReact;
            }
        }

        /// <summary>
        /// Returns true if we have effects that inhibit us
        /// from taking any kind of action.
        /// </summary>
        /// <returns></returns>
        public bool IsUnable
        {
            get
            {
                StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Charm1, StatusEffect.Charm2, 
                StatusEffect.Petrification, StatusEffect.Sleep, 
                StatusEffect.Sleep2, StatusEffect.Stun, 
                StatusEffect.Chocobo, StatusEffect.Terror, 
            };

                bool IsPlayerUnable = effectsThatBlock
                    .Intersect(Engine.FFInstance.Instance.Player.StatusEffects)
                    .Count() != 0;

                return IsPlayerUnable;
            }
        }

        /// <summary>
        /// Can we use job abilities?
        /// </summary>
        public bool IsAbilitiesBlocked
        {
            get
            {
                StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Amnesia
            };

                bool IsAbilitiesBlocked = effectsThatBlock
                    .Intersect(Engine.FFInstance.Instance.Player.StatusEffects)
                    .Count() != 0;

                return IsAbilitiesBlocked || IsUnable;
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
                return Engine.FFInstance.Instance.Player.HPPCurrent <= Engine.Config.LowHP;
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
                return Engine.Units.HasAggro;
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

                return RestBlockingDebuffs.Intersect(Engine.FFInstance.Instance.Player.StatusEffects).Count() != 0;
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
                return Engine.FFInstance.Instance.Player.Status == Status.Fighting;
            }
        }

        private GameEngine Engine { get; set; }
    }
}
