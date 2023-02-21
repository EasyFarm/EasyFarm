// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
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
using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.States;

namespace EasyFarm.UserSettings
{
    public interface IConfig
    {
        bool AggroFilter { get; set; }
        BattleLists BattleLists { get; set; }
        bool ClaimedFilter { get; set; }
        double DetectionDistance { get; set; }
        int GlobalCooldown { get; set; }
        double HeightThreshold { get; set; }
        int HighHealth { get; set; }
        int HighMagic { get; set; }
        ObservableCollection<string> IgnoredMobs { get; set; }
        string IgnoredName { get; set; }
        bool IsApproachEnabled { get; set; }
        bool IsEngageEnabled { get; set; }
        bool IsHealthEnabled { get; set; }
        bool IsMagicEnabled { get; set; }
        int LowHealth { get; set; }
        int LowMagic { get; set; }
        double MeleeDistance { get; set; }
        bool PartyFilter { get; set; }
        ObservableCollection<string> TargetedMobs { get; set; }
        string TargetName { get; set; }
        bool UnclaimedFilter { get; set; }
        double WanderDistance { get; set; }
        bool StraightRoute { get; set; }
        bool MinimizeToTray { get; set; }
        int TrustPartySize { get; set; }
        bool HomePointOnDeath { get; set; }
        bool EnableTabTargeting { get; set; }
        bool IsObjectAvoidanceEnabled { get; set; }
        double FollowDistance { get; set; }
        string FollowedPlayer { get; set; }
        int MinutesToRun { get; set; }
        int StopAtLevel { get; set; }

        Route Route { get; set; }
    }
}