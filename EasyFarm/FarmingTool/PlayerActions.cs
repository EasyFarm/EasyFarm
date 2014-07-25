
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
using EasyFarm.GameData;
using ZeroLimits.XITools;

namespace ZeroLimits.FarmingTool
{
    /// <summary>
    /// This class is responsible for holding all of the abilities our character may use in battle.
    /// </summary>
    public class PlayerActions
    {
        private FFACE _fface;
        private FarmingTools _ftools { get { return FarmingTools.GetInstance(_fface); } }

        public PlayerActions(FFACE fface)
        {
            this._fface = fface;
        }

        /// <summary>
        /// Returns the list of usable pulling abilities and spells.
        /// </summary>
        public List<Ability> StartList
        {
            get { return FilterValidActions(_ftools.UserSettings.ActionInfo.StartList); }
        }

        /// <summary>
        /// Returns the list of usable ending abilities and spells.
        /// </summary>
        public List<Ability> EndList
        {
            get { return FilterValidActions(_ftools.UserSettings.ActionInfo.EndList); }
        }

        /// <summary>
        /// Returns the list of usable battle abilities and spells.
        /// </summary>
        public List<Ability> BattleList
        {
            get { return FilterValidActions(_ftools.UserSettings.ActionInfo.BattleList); }
        }

        /// <summary>
        /// Returns the list of usable pulling abilities and spells. 
        /// </summary>
        public List<Ability> PullList
        {
            get { return FilterValidActions(_ftools.UserSettings.ActionInfo.PullList); }
        }

        /// <summary>
        /// Returns the list of currently usuable Healing Abilities and Spells.
        /// </summary>
        public List<Ability> HealingList
        {
            get
            {
                return FilterValidActions(
                        _ftools.UserSettings.ActionInfo.HealingList
                            .Where(x => x.IsEnabled)
                            .Where(x => x.TriggerLevel >= _fface.Player.HPPCurrent)
                            .Select(x => _ftools.AbilityService.CreateAbility(x.Name))
                            .Where(x => _ftools.AbilityExecutor.IsActionValid(x))
                            .ToList()
                    );
            }
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
            return Actions
                .Where(x => _ftools.AbilityExecutor.IsActionValid(x))
                    .ToList();
        }
    }
}
