
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

using EasyFarm.Views;
using FFACETools;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace EasyFarm.Classes
{
    /// <summary>
    /// An ability that should be used in battle. It represents both
    /// targeted and buffing abilities allowing the player to choose
    /// to cast abilities when a buff has worn. 
    /// </summary>
    public class BattleAbility : BindableBase
    {
        private bool m_enabled = false;

        /// <summary>
        /// Is the move enabled?
        /// </summary>
        public bool Enabled
        {
            get { return this.m_enabled; }
            set { SetProperty(ref this.m_enabled, value); }
        }

        private string m_name = String.Empty;

        /// <summary>
        /// The ability's name. I'm using NameX since it seems to 
        /// bind and Name and AbilityName do not. Will fix when I locate 
        /// the problem. 
        /// </summary>
        public String NameX
        {
            get { return this.m_name; }
            set
            {
                SetProperty(ref this.m_name, value);
            }
        }

        private string m_buff = String.Empty;

        /// <summary>
        /// Name of the buff that will be applied when using it. 
        /// </summary>
        public String Buff
        {
            get { return this.m_buff; }
            set { SetProperty(ref this.m_buff, value); }
        }

        private double m_distance = Constants.MELEE_DISTANCE;

        /// <summary>
        /// The distance the move should be used at. 
        /// </summary>
        public double Distance
        {
            get { return this.m_distance; }
            set
            {
                SetProperty(ref this.m_distance, (int)value);
            }
        }

        public Ability m_ability = new Ability();

        /// <summary>
        /// The other ability's attributes. 
        /// </summary>
        public Ability Ability
        {
            get { return this.m_ability; }
            set { SetProperty(ref this.m_ability, value); }
        }

        public BattleAbility() { }

        /// <summary>
        /// Sets the ability field. 
        /// </summary>
        public bool SetAbility()
        {
            // We've already parsed the ability. 
            if (this.Ability.Name.Equals(this.NameX,
                StringComparison.CurrentCultureIgnoreCase)) return true;

            AbilityService Fetcher = new AbilityService();

            // Retriever all moves with the specified name. 
            var moves = Fetcher.GetAbilitiesWithName(this.NameX);

            if (moves.Any())
            {
                if (moves.Count > 1)
                {
                    // Let user pick one. 
                    this.Ability = new AbilitySelectionBox(this.NameX).SelectedAbility;
                }
                else
                {
                    // Grab the only move. 
                    this.Ability = moves.FirstOrDefault();
                }
            }

            // Return true if we've found and stored any ability. 
            return this.Ability.IsValidName;
        }

        /// <summary>
        /// Check to see if the buff has wore on the player. 
        /// </summary>
        /// <param name="fface"></param>
        /// <returns></returns>
        public bool HasEffectWore(FFACE fface)
        {
            // Use matching to determine if a buff has wore. 
            // We remove '_' characters so buffs can be properly
            // identified. 
            return !fface.Player.StatusEffects.Any(x => 
                Regex.IsMatch(x.ToString(), 
                Buff.Replace(" ", "_"), 
                RegexOptions.IgnoreCase));
        }

        public bool IsCastable(FFACE fface)
        {
            // Check if the ability is valid. 
            return Helpers.IsActionValid(fface, this.Ability);
        }

        /// <summary>
        /// Determines whether this is a buffing ability. 
        /// </summary>
        /// <returns></returns>
        public bool IsBuff()
        {
            return !String.IsNullOrWhiteSpace(this.Buff);
        }
    }
}