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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EasyFarm.Classes;
using FFACETools;
using AutoMapper;

namespace EasyFarm.Components
{
    public class TravelComponent : BaseState
    {
        private readonly FFACE _fface;
        private readonly UnitService _units;
        private int _position;

        public TravelComponent(FFACE fface)
        {
            _fface = fface;

            // Create unit object for parsing of npc array. 
            _units = new UnitService(fface);
        }

        /// <summary>
        ///     Returns a copy of the current values in our path.
        /// </summary>
        public List<FFACE.Position> Path
        {
            get { return Config.Instance.Waypoints.Select(x => Mapper.Map<Position, FFACE.Position>(x)).ToList(); }
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
            if (ProhibitEffects.ProhibitEffectsMovement
                .Intersect(_fface.Player.StatusEffects).Any()) return false;

            return true;
        }

        public override void RunComponent()
        {
            // Make a copy of the waypoint path from config. 
            var route = Path;

            // Reverse the waypoint path. 
            if (_position == Path.Count)
            {
                Config.Instance.Waypoints = new ObservableCollection<Position>(
                    Config.Instance.Waypoints.Reverse());

                // Copy new waypoint path from config. 
                route = Path;

                _position = 0;
            }

            // If far away from the path, set us to run to the closest waypoint
            if (_fface.Navigator.DistanceTo(Path[_position]) > 15)
            {
                var closest = route.OrderBy(x => _fface.Navigator.DistanceTo(x))
                    .FirstOrDefault();

                _position = route.IndexOf(closest);
            }

            // Run to the waypoint allowing cancellation on aggro or paused. 
            _fface.Navigator.Goto(route[_position], false);
            _position++;
        }

        /// <summary>
        ///     Returns true when the player should stop traveling.
        /// </summary>
        /// <returns></returns>
        public bool IsCancellationRequired()
        {
            // Defines some common situations to stop travel. 
            if (_units.HasAggro) return true;

            // Return when the user has pause the program. 
            return !App.GameEngine.IsWorking;
        }
    }
}