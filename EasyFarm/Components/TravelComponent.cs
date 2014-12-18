
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

﻿using System.Linq;
using System.Collections.ObjectModel;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using EasyFarm.GameData;
using System.Diagnostics;
using EasyFarm.ViewModels;
using EasyFarm.UserSettings;

<<<<<<< HEAD:EasyFarm/Components/TravelComponent.cs

namespace EasyFarm.Components
{
    public class TravelComponent : BaseComponent
=======
namespace EasyFarm.States
{
    [StateAttribute(priority: 1)]
    public class TravelState : BaseState
>>>>>>> VM_and_State_AutoLocate_and_AttackState_Refactor:EasyFarm/States/TravelState.cs
    {
        private int position = 0;

        public TravelComponent(FFACE fface) : base(fface) { }

        public override bool CheckComponent()
        {
            // Waypoint list is empty.
            if (Config.Instance.Waypoints.Count <= 0) return false;

            // We are not able to attack any creatures. 
            if (new AttackComponent(FFACE).CheckComponent()) return false;

            // We don't have to rest. 
            if (new RestComponent(FFACE).CheckComponent()) return false;

            // We don't have to heal. 
            if (new HealingComponent(FFACE).CheckComponent()) return false;

            // We are not bound or struck by an other movement
            // disabling condition. 
            if (ftools.ActionBlocked.IsUnable) return false;

            return true;
        }

        public override void EnterComponent()
        {
            ftools.RestingService.EndResting();
            ftools.CombatService.Disengage();

            FFACE.Navigator.DistanceTolerance = 1;
            FFACE.Navigator.HeadingTolerance = 1;
        }

        public override void RunComponent()
        {
            // If we've reached the end of the path....
            if (position > Config.Instance.Waypoints.Count - 1)
            {
                // Turn around and run the path in reverse with the old end being the new starting point
                Config.Instance.Waypoints = new ObservableCollection<Waypoint>
                    (Config.Instance.Waypoints.Reverse());

                position = 0;
            }

            // If we are more than 10 yalms away from the nearest point...
            if (FFACE.Navigator.DistanceTo(Config.Instance.Waypoints[position].Position) > 10)
            {
                SetPositionToNearestPoint();
            }

            bool isCanceled = false;
            var KeepFromRestingTask = new Task(() =>
            {
                while (!isCanceled)
                {
                    if (FFACE.Player.Status.Equals(Status.Healing))
                    {
                        ftools.RestingService.EndResting();
                    }
                }
            });

            KeepFromRestingTask.Start();
            FFACE.Navigator.Goto(Config.Instance.Waypoints[position].Position, false);
            isCanceled = true;
            position++;
        }

        public override void ExitComponent() { FFACE.Navigator.Reset(); }

        /// <summary>
        /// Set the position to nearest point. 
        /// </summary>
        private void SetPositionToNearestPoint()
        {
            // Get the nearest waypoint to the player. 
            var nearest = Config.Instance.Waypoints
                .OrderBy(x => FFACE.Navigator.DistanceTo(x.Position))
                .FirstOrDefault();

            // Return if the list is empty; 
            if (nearest == null) return;

            // Get its index in the array of points, then ...
            position = Config.Instance.Waypoints.IndexOf(nearest);
        }
    }
}
