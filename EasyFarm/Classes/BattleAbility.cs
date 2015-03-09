
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

using EasyFarm.ViewModels;
using EasyFarm.Views;
using FFACETools;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

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

        private string m_name = String.Empty;

        private string m_buff = String.Empty;

        private double m_distance = Constants.MELEE_DISTANCE;

        private Ability m_ability = new Ability();
        
        public BattleAbility()
        {
            SetCommand = new DelegateCommand(SetAbility);
        }        
              
        /// <summary>
        /// Sets the ability field. 
        /// </summary>
        public void SetAbility()
        {            
            // We've already parsed the ability. 
            if (Ability.Name.Equals(Name,
                StringComparison.CurrentCultureIgnoreCase))
            {
                ViewModelBase.InformUser(Name + " set successfully. ");
                return;
            }

            // Get the ability by name. 
            var ability = FindAbility(Name);

            // Attempt to set the ability and inform the 
            // user of its sucess. 
            if (ability == null)
            {
                ViewModelBase.InformUser(Name + " could not be set. ");
            }
            else 
            {
                this.Ability = ability;
                ViewModelBase.InformUser(Name + " set successfully. ");
            }
        }

        /// <summary>
        /// Locates an ability by name and prompting the user if 
        /// more than one ability has been found. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Ability FindAbility(String name)
        {
            // Our service for locating abilities. 
            AbilityService Fetcher = new AbilityService();

            // Retriever all moves with the specified name. 
            var moves = Fetcher.GetAbilitiesWithName(name);

            // Prompt user to select a move if more 
            // than one are found with the same name. 
            // Otherwise, return the first occurence or null. 
            if (moves.Count > 1)
                return new AbilitySelectionBox(this.Name).SelectedAbility;
            else 
                return moves.FirstOrDefault();
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

        /// <summary>
        /// Sets an BattleAbility's ability object using the ability service object
        /// for lookups. 
        /// </summary>
        // Delegate Commands cannot be serialized. 
        [XmlIgnore] 
        public DelegateCommand SetCommand { get; set; }

        /// <summary>
        /// Is the move enabled?
        /// </summary>
        public bool Enabled
        {
            get { return this.m_enabled; }
            set { SetProperty(ref this.m_enabled, value); }
        }

        /// <summary>
        /// The ability's name. I'm using NameX since it seems to 
        /// bind and Name and AbilityName do not. Will fix when I locate 
        /// the problem. 
        /// </summary>
        public String Name
        {
            get { return this.m_name; }
            set
            {
                SetProperty(ref this.m_name, value);
            }
        }

        /// <summary>
        /// Name of the buff that will be applied when using it. 
        /// </summary>
        public String Buff
        {
            get { return this.m_buff; }
            set { SetProperty(ref this.m_buff, value); }
        }

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

        /// <summary>
        /// The other ability's attributes. 
        /// </summary>
        public Ability Ability
        {
            get { return this.m_ability; }
            set { SetProperty(ref this.m_ability, value); }
        }
    }
}