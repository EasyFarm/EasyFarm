
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
using Parsing.Abilities;
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
        #region Basic Conditions

        /// <summary>
        /// Is this move ready for use?
        /// </summary>
        private bool _isEnabled = false;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        /// <summary>
        /// The move's name. 
        /// </summary>
        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// The move's max distance. 
        /// </summary>
        private double _distance = 0;

        public double Distance
        {
            get { return _distance; }
            set { SetProperty(ref _distance, value); }
        }

        /// <summary>
        /// Resource information about the move. 
        /// </summary>
        private Ability _ability;

        /// <summary>
        /// Holds the resource file information for the move. 
        /// </summary>
        public Ability Ability
        {
            get { return this._ability; }
            set { SetProperty(ref this._ability, value); }
        }
        #endregion

        #region Player Conditions
        /// <summary>
        /// Check for the presense or absents of a status effect. 
        /// Unchecked means we use moves in absence of the status effect. 
        /// Checked means we use moves when the status effect is present. 
        /// </summary>
        private bool _triggerOnEffectPresent = false;

        public bool TriggerOnEffectPresent
        {
            get { return _triggerOnEffectPresent; }
            set { SetProperty(ref _triggerOnEffectPresent, value); }
        }

        /// <summary>
        /// The status effect to check for. 
        /// </summary>
        private string _statusEffect = string.Empty;

        public string StatusEffect
        {
            get { return _statusEffect; }
            set { SetProperty(ref _statusEffect, value); }
        }

        /// <summary>
        /// The upper limit of the player's health. 
        /// </summary>
        private int _playerLowerHealth = 0;

        public int PlayerLowerHealth
        {
            get { return _playerLowerHealth; }
            set
            {
                SetProperty(ref _playerLowerHealth, value);
                ViewModelBase.InformUser("Lower health set to {0}.", _playerLowerHealth);
            }
        }

        /// <summary>
        /// The lower limit of the player's health. 
        /// </summary>
        private int _playerUpperHealth = 0;

        public int PlayerUpperHealth
        {
            get { return _playerUpperHealth; }
            set
            {
                SetProperty(ref _playerUpperHealth, value);
                ViewModelBase.InformUser("Upper health set to {0}.", _playerUpperHealth);
            }
        }
        #endregion

        #region Target Conditions
        /// <summary>
        /// Regular expression used for filtering target names. 
        /// </summary>
        private string _targetName = string.Empty;

        public string TargetName
        {
            get { return _targetName; }
            set { SetProperty(ref _targetName, value); }
        }

        /// <summary>
        /// Target's lower health threshold. 
        /// </summary>
        private int _targetLowerHealth = 0;

        public int TargetLowerHealth
        {
            get { return _targetLowerHealth; }
            set
            {
                SetProperty(ref _targetLowerHealth, value);
                ViewModelBase.InformUser("Lower health set to {0}.", _targetLowerHealth);
            }
        }

        /// <summary>
        /// Target's upper health threshold. 
        /// </summary>
        private int _targetUpperHealth = 100;

        public int TargetUpperHealth
        {
            get { return _targetUpperHealth; }
            set
            {
                SetProperty(ref _targetUpperHealth, value);
                ViewModelBase.InformUser("Upper health set to {0}.", _targetUpperHealth);
            }
        }
        #endregion

        /// <summary>
        /// Create our command binds and initialize our user's
        /// move usage conditions. 
        /// </summary>
        public BattleAbility()
        {
            AutoFillCommand = new DelegateCommand(AutoFill);
            _ability = new Ability();
        }

        /// <summary>
        /// Sets the ability field. 
        /// </summary>
        public void AutoFill()
        {
            // We've already parsed the ability. 
            if (Ability.English.Equals(Name, StringComparison.CurrentCultureIgnoreCase))
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
            // Retriever all moves with the specified name. 
            var moves = App.AbilityService.GetAbilitiesWithName(name);

            // Prompt user to select a move if more 
            // than one are found with the same name. 
            // Otherwise, return the first occurence or null. 
            if (moves.Count() > 1)
                return new AbilitySelectionBox(name).SelectedAbility;
            else
                return moves.FirstOrDefault();
        }

        /// <summary>
        /// Sets an BattleAbility's ability object using the ability service object
        /// for lookups. 
        /// </summary>
        // Delegate Commands cannot be serialized. 
        [XmlIgnore]
        public DelegateCommand AutoFillCommand { get; set; }
    }
}