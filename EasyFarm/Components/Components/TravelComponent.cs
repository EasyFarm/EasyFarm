
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

﻿using EasyFarm.Classes;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using FFACETools;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyFarm.Components
{
    public class TravelComponent : MachineComponent
    {
        private int _position = 0;
        private FFACE _fface;
        private RestingService _resting;
        private CombatService _combat;
        private UnitService _units;

        public TravelComponent(FFACE fface)
        {
            _fface = fface;
            _resting = new RestingService(fface);
            _combat = new CombatService(fface);

            // Create unit object for parsing of npc array. 
            _units = new UnitService(fface);
            _units.UnitFilter = UnitFilters.MobFilter(fface);
        }

        public override bool CheckComponent()
        {
            // Waypoint list is empty.
            if (Config.Instance.Waypoints.Count <= 0) return false;

            // We are not able to attack any creatures. 
            if (new AttackContainer(_fface).CheckComponent()) return false;

            // We don't have to rest. 
            if (new RestComponent(_fface).CheckComponent()) return false;

            // We don't have to heal. 
            if (new HealingComponent(_fface).CheckComponent()) return false;

            // We are not bound or struck by an other movement
            // disabling condition. 
            if (ProhibitEffects.PROHIBIT_EFFECTS_MOVEMENT
                .Intersect(_fface.Player.StatusEffects).Any()) return false;

            return true;
        }

        public override void EnterComponent()
        {
            _combat.Disengage();
            _fface.Navigator.DistanceTolerance = 1;
            _fface.Navigator.HeadingTolerance = 1;
        }

        public override void RunComponent()
        {
            // If we've reached the end of the path....
            if (_position > Config.Instance.Waypoints.Count - 1)
            {
                // Turn around and run the path in reverse with the old end being the new starting point
                Config.Instance.Waypoints = new ObservableCollection<Waypoint>
                    (Config.Instance.Waypoints.Reverse());

                _position = 0;
            }

            // If we are more than 10 yalms away from the nearest point...

            // Fix: May throw array out of bounds error when collection 
            // is modified during execution. 

            if (_fface.Navigator.DistanceTo(Config.Instance.Waypoints[_position].Position) > 10)
            {
                SetPositionToNearestPoint();
            }

            _fface.Navigator.Goto(Config.Instance.Waypoints[_position].Position, false);
            _position++;
        }

        public DateTime LastAggroCheck = DateTime.Now;

        /// <summary>
        /// Returns true when the player should stop traveling. 
        /// </summary>
        /// <returns></returns>
        public bool IsCancellationRequired()
        {
            /// Defines some common situations to stop travel. 

            // Check if the player has aggro, but don't hammer the 
            // npc array less than every second. 
            if (LastAggroCheck.AddSeconds(Constants.UNIT_ARRAY_CHECK_RATE) < DateTime.Now)
            {
                if (_units.HasAggro) return true;
            }

            // Return when the user has pause the program. 
            if (!ViewModelBase.GameEngine.IsWorking)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Set the position to nearest point. 
        /// </summary>
        private void SetPositionToNearestPoint()
        {
            // Get the nearest waypoint to the player. 
            var nearest = Config.Instance.Waypoints
                .OrderBy(x => _fface.Navigator.DistanceTo(x.Position))
                .FirstOrDefault();

            // Return if the list is empty; 
            if (nearest == null) return;

            // Get its index in the array of points, then ...
            _position = Config.Instance.Waypoints.IndexOf(nearest);
        }
    }
}
