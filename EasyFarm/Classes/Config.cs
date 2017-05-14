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

using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using EasyFarm.States;
using Prism.Mvvm;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     A configuration file for the user to edit through his GUI.
    ///     Gives the bot access to all of his decisions.
    /// </summary>
    public class Config : BindableBase
    {
        [XmlIgnore] private static Lazy<Config> _lazy = new Lazy<Config>(() => new Config());

        /// <summary>
        ///     Used to filter out aggroed mobs.
        /// </summary>
        public bool AggroFilter = true;

        /// <summary>
        ///     Contains all the lists that contain the moves used by
        ///     our player.
        /// </summary>
        public BattleLists BattleLists = new BattleLists();

        /// <summary>
        ///     Used to include claimed mobs in the filter.
        /// </summary>
        public bool ClaimedFilter = false;

        /// <summary>
        ///     How far a player should detect a creature.
        /// </summary>
        public double DetectionDistance = Constants.DetectionDistance;

        /// <summary>
        ///     Cast delay before casting next spell
        ///     (stops cannot use ability spam)
        /// </summary>
        public int GlobalCooldown = Constants.GlobalSpellCooldown;

        /// <summary>
        ///     How high or low a player should detect a creature.
        /// </summary>
        public double HeightThreshold = Constants.HeightThreshold;

        /// <summary>
        ///     The high value to stand up from resting health.
        /// </summary>
        public int HighHealth = 100;

        /// <summary>
        ///     The upper value to stand up from mp resting.
        /// </summary>
        public int HighMagic = 100;

        /// <summary>
        ///     A list of mobs that we should ignore.
        /// </summary>
        public ObservableCollection<string> IgnoredMobs = new ObservableCollection<string>();

        /// <summary>
        ///     Name of the mob to be ignored
        /// </summary>
        public string IgnoredName = string.Empty;

        /// <summary>
        ///     Should we move to the target once pulled.
        /// </summary>
        public bool IsApproachEnabled = true;

        /// <summary>
        ///     Should we engage our target when in battle.
        /// </summary>
        public bool IsEngageEnabled = true;

        /// <summary>
        ///     Whether health resting is enabled.
        /// </summary>
        public bool IsHealthEnabled = false;

        /// <summary>
        ///     Whether mp resting is enabled.
        /// </summary>
        public bool IsMagicEnabled = false;

        /// <summary>
        ///     The low value to start resting for health.
        /// </summary>
        public int LowHealth = 50;

        /// <summary>
        ///     The lower value to sit down and rest mp.
        /// </summary>
        public int LowMagic = 50;

        /// <summary>
        ///     How close the player should be when attacking a creature.
        /// </summary>
        public double MeleeDistance = Constants.MeleeDistance;

        /// <summary>
        ///     Used to filter out party claimed mobs.
        /// </summary>
        public bool PartyFilter = true;

        /// <summary>
        ///     A list of mobs that we should only kill.
        /// </summary>
        public ObservableCollection<string> TargetedMobs = new ObservableCollection<string>();

        /// <summary>
        ///     Name of the mob to be attacked
        /// </summary>
        public string TargetName = string.Empty;

        /// <summary>
        ///     Used to filter out unclaimed mobs.
        /// </summary>
        public bool UnclaimedFilter = true;

        /// <summary>
        ///     How far to go of the path for a unit.
        /// </summary>
        public double WanderDistance = Constants.DetectionDistance;

        static Config()
        {
            // Add battle moves at the start only once since deserialization
            // can cause duplicate entries when the default constructor is
            // called.
            Instance.BattleLists.Add(new BattleList("Start"));
            Instance.BattleLists.Add(new BattleList("Trusts"));
            Instance.BattleLists.Add(new BattleList("Pull"));
            Instance.BattleLists.Add(new BattleList("Battle"));
            Instance.BattleLists.Add(new BattleList("End"));
            Instance.BattleLists.Add(new BattleList("Healing"));
            Instance.BattleLists.Add(new BattleList("Weaponskill"));
        }

        [XmlIgnore]
        public static Config Instance
        {
            get { return _lazy.Value; }
            set { _lazy = new Lazy<Config>(() => value); }
        }

        public Route Route = new Route();

        /// <summary>
        /// Bot runs a straight route.
        /// </summary>
        public bool StraightRoute => Route.StraightRoute;

        /// <summary>
        /// Number of trusts allowable in the party.
        /// </summary>
        public int TrustPartySize = Constants.TrustPartySize;

        /// <summary>
        /// Whether the player should warp home on death or not.
        /// </summary>
        public bool HomePointOnDeath = false;

        /// <summary>
        /// Toggles whether program should target things using memory or by
        /// tabbing to its target.
        /// </summary>
        public bool EnableTabTargeting = false;

        /// <summary>
        /// Whether the player should avoid objects when becoming stuck.
        /// </summary>
        public bool IsObjectAvoidanceEnabled = false;

        /// <summary>
        /// The distance with which to follow a player.
        /// </summary>
        public double FollowDistance = 5.0;

        /// <summary>
        /// The current player to follow.
        /// </summary>
        public string FollowedPlayer = string.Empty;
    }  
}
