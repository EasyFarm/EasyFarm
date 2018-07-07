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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using EasyFarm.Handlers;
using EasyFarm.Parsing;
using EasyFarm.UserSettings;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using MahApps.Metro.Controls;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     An ability that should be used in battle. It represents both
    ///     targeted and buffing abilities allowing the player to choose
    ///     to cast abilities when a buff has worn.
    /// </summary>
    public class BattleAbility : ObservableObject
    {
        /// <summary>
        ///     Maps ability type objects to their commands.
        /// </summary>
        private static readonly Dictionary<AbilityType, string> CommandMapper
            = new Dictionary<AbilityType, string>();

        private AbilityType _abilityType = AbilityType.Unknown;

        /// <summary>
        ///     The move's max distance.
        /// </summary>
        private double _distance = Constants.MeleeDistance;

        /// <summary>
        ///     Is this move ready for use?
        /// </summary>
        private bool _isEnabled;

        /// <summary>
        ///     The move's name.
        /// </summary>
        private string _name;

        /// <summary>
        ///     The upper limit of the player's health.
        /// </summary>
        private int _playerLowerHealth;

        /// <summary>
        ///     The lower limit of the player's health.
        /// </summary>
        private int _playerUpperHealth;

        /// <summary>
        /// Private backing for recast in seconds.
        /// </summary>
        private int _recast;

        /// <summary>
        ///     The status effect to check for.
        /// </summary>
        private string _statusEffect = string.Empty;

        /// <summary>
        ///     Target's lower health threshold.
        /// </summary>
        private int _targetLowerHealth;

        /// <summary>
        ///     Regular expression used for filtering target names.
        /// </summary>
        private string _targetName = string.Empty;

        /// <summary>
        ///     Target's upper health threshold.
        /// </summary>
        private int _targetUpperHealth;

        /// <summary>
        ///     Check for the presense or absents of a status effect.
        ///     Unchecked means we use moves in absence of the status effect.
        ///     Checked means we use moves when the status effect is present.
        /// </summary>
        private bool _triggerOnEffectPresent;

        /// <summary>
        ///     Private backing for UsageLimit.
        /// </summary>
        private int _usageLimit;

        /// <summary>
        ///     Private backing for usages.
        /// </summary>
        private int _usages;

        private int _mpReserveLow;
        private int _mpReserveHigh;
        private int _tpReserveLow;
        private int _tpReserveHigh;

        /// <summary>
        /// Whether or not the trust should be resummoned if their HP is in a given range
        /// </summary>
        private bool _resummonIfHpLow = false;

        /// <summary>
        /// Whether or not the trust should be resummoned if their MP is in a given range
        /// </summary>
        private bool _resummonIfMpLow = false;

        /// <summary>
        /// Resummon HP Low End
        /// </summary>
        private int _resummonHpLow;

        /// <summary>
        /// Resummon HP High End
        /// </summary>
        private int _resummonHpHigh;

        /// <summary>
        /// Resummon MP Low End
        /// </summary>
        private int _resummonMpLow;

        /// <summary>
        /// Resummon MP High End
        /// </summary>
        private int _resummonMpHigh;

        private int _index;
        private int _id;
        private int _mpCost;
        private int _tpCost;
        private string _command;
        private TargetType _targetType;
        private string _chatEvent;
        private TimeSpan _chatEventPeriod;


        static BattleAbility()
        {
            var commandTypes = new ObservableCollection<AbilityType>
            {
                AbilityType.Unknown,
                AbilityType.Jobability,
                AbilityType.Magic,
                AbilityType.Trust,
                AbilityType.Monsterskill,
                AbilityType.Ninjutsu,
                AbilityType.Pet,
                AbilityType.Range,
                AbilityType.Song,
                AbilityType.Weaponskill,
                AbilityType.Item
            };

            // Load valid prefixes.
            CommandPrefixes = new ReadOnlyObservableCollection<AbilityType>(commandTypes);

            var commandPrefix = new ObservableCollection<TargetType>
            {
                TargetType.Unknown,
                TargetType.Self,
                TargetType.Enemy
            };

            // Load valid targets.
            CommandTargets = new ReadOnlyObservableCollection<TargetType>(commandPrefix);

            // Ability type objects to their commands.
            CommandMapper.Add(AbilityType.Jobability, "/jobability");
            CommandMapper.Add(AbilityType.Magic, "/magic");
            CommandMapper.Add(AbilityType.Monsterskill, "/monsterskill");
            CommandMapper.Add(AbilityType.Ninjutsu, "/ninjutsu");
            CommandMapper.Add(AbilityType.Pet, "/pet");
            CommandMapper.Add(AbilityType.Range, "/range");
            CommandMapper.Add(AbilityType.Song, "/song");
            CommandMapper.Add(AbilityType.Weaponskill, "/weaponskill");
            CommandMapper.Add(AbilityType.Item, "/item");
            CommandMapper.Add(AbilityType.Trust, "/magic");
        }

        /// <summary>
        ///     Create our command binds and initialize our user's
        ///     move usage conditions.
        /// </summary>
        public BattleAbility()
        {
            AutoFillCommand = new RelayCommand(async () => await AutoFill());
        }

        /// <summary>
        ///     List of usable command prefixes.
        /// </summary>
        public static ReadOnlyObservableCollection<AbilityType> CommandPrefixes { get; set; }

        /// <summary>
        ///     List of usable command targets.
        /// </summary>
        public static ReadOnlyObservableCollection<TargetType> CommandTargets { get; set; }

        public AbilityType AbilityType
        {
            get { return _abilityType; }
            set { Set(ref _abilityType, value); }
        }

        /// <summary>
        ///     Sets an BattleAbility's ability object using the ability service object
        ///     for lookups.
        /// </summary>
        // Delegate Commands cannot be serialized.
        [XmlIgnore]
        public RelayCommand AutoFillCommand { get; set; }

        public double Distance
        {
            get { return _distance; }
            set
            {
                Set(ref _distance, (int)value);
                AppServices.InformUser("Distance set to {0}.", _distance);
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { Set(ref _isEnabled, value); }
        }

        /// <summary>
        /// When this move was last used. 
        /// </summary>
        public DateTime LastCast { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                Set(ref _name, value);
                RaisePropertyChanged(nameof(AvailableAbilities));
            }
        }
        public int PlayerLowerHealth
        {
            get { return _playerLowerHealth; }
            set
            {
                Set(ref _playerLowerHealth, value);
                AppServices.InformUser("Lower health set to {0}.", _playerLowerHealth);
            }
        }

        public int PlayerUpperHealth
        {
            get { return _playerUpperHealth; }
            set
            {
                Set(ref _playerUpperHealth, value);
                AppServices.InformUser("Upper health set to {0}.", _playerUpperHealth);
            }
        }

        /// <summary>
        /// The recast in seconds when this action should be used again.
        /// </summary>
        public int Recast
        {
            get { return _recast; }
            set { Set(ref _recast, value); }
        }
        public string StatusEffect
        {
            get { return _statusEffect; }
            set { Set(ref _statusEffect, value); }
        }

        public int TargetLowerHealth
        {
            get { return _targetLowerHealth; }
            set
            {
                Set(ref _targetLowerHealth, value);
                AppServices.InformUser("Lower health set to {0}.", _targetLowerHealth);
            }
        }

        public string TargetName
        {
            get { return _targetName; }
            set { Set(ref _targetName, value); }
        }

        public int TargetUpperHealth
        {
            get { return _targetUpperHealth; }
            set
            {
                Set(ref _targetUpperHealth, value);
                AppServices.InformUser("Upper health set to {0}.", _targetUpperHealth);
            }
        }

        public int MPReserveLow
        {
            get { return _mpReserveLow; }
            set
            {
                Set(ref _mpReserveLow, value);
                AppServices.InformUser($"MP range set {_mpReserveLow} TO {_mpReserveHigh}.");
            }
        }

        public int MPReserveHigh
        {
            get { return _mpReserveHigh; }
            set
            {
                Set(ref _mpReserveHigh, value);
                AppServices.InformUser($"MP range set {_mpReserveLow} TO {_mpReserveHigh}.");
            }
        }

        public int TPReserveLow
        {
            get { return _tpReserveLow; }
            set
            {
                Set(ref _tpReserveLow, value);
                AppServices.InformUser($"TP range set {_tpReserveLow} TO {_tpReserveHigh}.");
            }
        }

        public int TPReserveHigh
        {
            get { return _tpReserveHigh; }
            set
            {
                Set(ref _tpReserveHigh, value);
                AppServices.InformUser($"TP range set {_tpReserveLow} TO {_tpReserveHigh}.");
            }
        }
        
        public bool ResummonOnLowHP
        {
            get { return _resummonIfHpLow; }
            set
            {
                Set(ref _resummonIfHpLow, value);
                if (value)
                {
                    AppServices.InformUser($"Resummon if HP is low turned on.");
                }
                else
                {
                    AppServices.InformUser($"Resummon if HP is low turned off.");
                }
            }
        }

        public bool ResummonOnLowMP
        {
            get { return _resummonIfMpLow; }
            set
            {
                Set(ref _resummonIfMpLow, value);
                if (value)
                {
                    AppServices.InformUser($"Resummon if MP is low turned on.");
                }
                else
                {
                    AppServices.InformUser($"Resummon if MP is low turned off.");
                }
            }
        }


        public int ResummonHPLow
        {
            get { return _resummonHpLow; }
            set
            {
                Set(ref _resummonHpLow, value);
                AppServices.InformUser($"Resummon HP Range set {_resummonHpLow} TO {_resummonHpHigh}.");
            }
        }

        public int ResummonHPHigh
        {
            get { return _resummonHpHigh; }
            set
            {
                Set(ref _resummonHpHigh, value);
                AppServices.InformUser($"Resummon HP Range set {_resummonHpLow} TO {_resummonHpHigh}.");
            }
        }

        public int ResummonMPLow
        {
            get { return _resummonMpLow; }
            set
            {
                Set(ref _resummonMpLow, value);
                AppServices.InformUser($"Resummon MP Range set {_resummonMpLow} TO {_resummonMpHigh}.");
            }
        }

        public int ResummonMPHigh
        {
            get { return _resummonMpHigh; }
            set
            {
                Set(ref _resummonMpHigh, value);
                AppServices.InformUser($"Resummon MP Range set {_resummonMpLow} TO {_resummonMpHigh}.");
            }
        }
        
        public bool TriggerOnEffectPresent
        {
            get { return _triggerOnEffectPresent; }
            set { Set(ref _triggerOnEffectPresent, value); }
        }
        /// <summary>
        ///     The maximum limit of times this move can be used in battle.
        /// </summary>
        public int UsageLimit
        {
            get { return _usageLimit; }
            set { Set(ref _usageLimit, value); }
        }
        /// <summary>
        ///     The number of times this move has been used.
        /// </summary>
        public int Usages
        {
            get { return _usages; }
            set { Set(ref _usages, value); }
        }

        public int Index
        {
            get { return _index; }
            set { Set(ref _index, value); }
        }

        public int Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        public int MpCost
        {
            get { return _mpCost; }
            set { Set(ref _mpCost, value); }
        }

        public int TpCost
        {
            get { return _tpCost; }
            set { Set(ref _tpCost, value); }
        }

        public string Command
        {
            get { return _command; }
            set { Set(ref _command, value); }
        }

        public TargetType TargetType
        {
            get { return _targetType; }
            set { Set(ref _targetType, value); }
        }

        public ObservableCollection<Ability> AvailableAbilities
        {
            get
            {
                ObservableCollection<Ability> availableAbilities = new ObservableCollection<Ability>(
                    Infrastructure.ViewModelBase.AbilityService?.Resources
                        .Where(x => x.English.ToLower().Contains(Name?.ToLower() ?? ""))
                        .Where(x => !string.IsNullOrWhiteSpace(x.English))
                        .Take(3)?
                        .ToList() ?? new List<Ability>());

                return availableAbilities;
            }
        }

        public string ChatEvent
        {
            get { return _chatEvent; }
            set { Set(ref _chatEvent, value); }
        }

        public TimeSpan? ChatEventPeriod
        {
            get { return _chatEventPeriod; }
            set { Set(ref _chatEventPeriod, value ?? default(TimeSpan)); }
        }

        /// <summary>
        ///     Sets the ability field.
        /// </summary>
        public async Task AutoFill()
        {
            // Return if string null or empty.
            if (string.IsNullOrWhiteSpace(Name)) return;

            // Get the ability by name.
            Ability ability = await new SelectAbilityRequestHandler(Application.Current.MainWindow as MetroWindow).Handle(Name);

            // Attempt to set the ability and inform the
            // user of its sucess.
            if (ability == null)
            {
                AppServices.InformUser("Auto-Fill failed to find {0} in resources. ", Name);
            }
            else
            {
                AppServices.InformUser("Auto-Filling for {0} complete. ", Name);

                Command = ability.Command;
                Index = ability.Index;
                Id = ability.Id;
                MpCost = ability.MpCost;
                TpCost = ability.TpCost;
                Command = ability.Command;
                AbilityType = ability.AbilityType;
                Name = ability.English;
                TargetType = ability.TargetType.HasFlag(TargetType.Self) ? TargetType.Self : TargetType.Enemy;
            }
        }

        public override string ToString()
        {
            return Command;
        }
    }
}