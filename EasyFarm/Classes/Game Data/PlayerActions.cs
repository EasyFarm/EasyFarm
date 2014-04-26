
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
    /// This class is responsible for holding all of the abilities our character may use in battle.
    /// </summary>
    public class PlayerActions
    {
        private GameEngine m_gameEngine;


        public PlayerActions(ref GameEngine m_gameEngine)
        {
            this.m_gameEngine = m_gameEngine;
        }

        /// <summary>
        /// Returns the list of usable pulling abilities and spells.
        /// </summary>
        public List<Ability> StartList
        {
            get { return FilterValidActions(m_gameEngine.UserSettings.ActionInfo.StartList); }
        }

        /// <summary>
        /// Returns the list of usable ending abilities and spells.
        /// </summary>
        public List<Ability> EndList
        {
            get { return FilterValidActions(m_gameEngine.UserSettings.ActionInfo.EndList); }
        }

        /// <summary>
        /// Returns the list of usable battle abilities and spells.
        /// </summary>
        public List<Ability> BattleList
        {
            get { return FilterValidActions(m_gameEngine.UserSettings.ActionInfo.BattleList); }
        }

        /// <summary>
        /// Returns the list of currently usuable Healing Abilities and Spells.
        /// </summary>
        public List<Ability> HealingList
        {
            get
            {
                return FilterValidActions(
                        m_gameEngine.UserSettings.ActionInfo.HealingList
                            .Where(x => x.Item.IsEnabled)
                            .Where(x => x.Item.TriggerLevel >= m_gameEngine.Session.Instance.Player.HPPCurrent)
                            .Select(x => m_gameEngine.AbilityService.CreateAbility(x.Item.Name))
                            .ToList()
                    );
            }
        }

        public WeaponAbility WeaponSkill
        {
            get;
            set;
        }

        /// <summary>
        /// Do we have moves we can use in battle again the creature?
        /// </summary>
        public bool HasBattleMoves
        {
            get { return BattleList.Count > 0; }
        }

        /// <summary>
        /// Do we have any moves we can pull the enemy with?
        /// </summary>
        public bool HasStartMoves
        {
            get { return StartList.Count > 0; }
        }

        /// <summary>
        /// Do we have instruction on what to do when the creature is dead?
        /// </summary>
        public bool HasEndMoves
        {
            get { return EndList.Count > 0; }
        }

        /// <summary>
        /// Do we have any moves that can heal the player.
        /// </summary>
        public bool HasHealingMoves
        {
            get { return HealingList.Count > 0; }
        }

        /// <summary>
        /// Returns the list of usable abilities
        /// </summary>
        /// <param name="Actions"></param>
        /// <returns></returns>
        public List<Ability> FilterValidActions(IList<Ability> Actions)
        {
            var a = Actions.Where(x => x.IsValidName).ToList();
            var b = a.Where(x => AbilityRecastable(x));
            var c = b.Where(x => x.MPCost <= m_gameEngine.Session.Instance.Player.MPCurrent && x.TPCost <= m_gameEngine.Session.Instance.Player.TPCurrent).ToList();
            var d = c.Where(x => (x.IsSpell && !m_gameEngine.ActionBlocked.IsCastingBlocked) || (x.IsAbility && !m_gameEngine.ActionBlocked.IsAbilitiesBlocked)).ToList();
            var e = d.ToList();

            return Actions
                    .Where(x => x.IsValidName)
                    .Where(x => AbilityRecastable(x))
                    .Where(x => x.MPCost <= m_gameEngine.Session.Instance.Player.MPCurrent && x.TPCost <= m_gameEngine.Session.Instance.Player.TPCurrent)
                    .Where(x => (x.IsSpell && !m_gameEngine.ActionBlocked.IsCastingBlocked) || (x.IsAbility && !m_gameEngine.ActionBlocked.IsAbilitiesBlocked))
                    .ToList();
        }

        /// <summary>
        /// Checks to  see if we can cast/use 
        /// a job ability or spell.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AbilityRecastable(Ability value)
        {
            int Recast = -1;

            if (value.IsSpell)
            {
                Recast = m_gameEngine.Session.Instance.Timer.GetSpellRecast((SpellList)value.Index);
            }
            else
            {
                Recast = m_gameEngine.Session.Instance.Timer.GetAbilityRecast((AbilityList)value.Index);
            }

            return 0 == Recast;
        }
    }
}
