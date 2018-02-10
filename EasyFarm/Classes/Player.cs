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
using System.Diagnostics;
using MemoryAPI;
using System.Threading;
using EasyFarm.UserSettings;
using EliteMMO.API;

namespace EasyFarm.Classes
{
    public class Player
    {
        private static Player _instance = new Player();

        public bool IsMoving { get; set; }

        public static Player Instance
        {
            get { return _instance = _instance ?? new Player(); ; }
            private set { _instance = value; }
        }

        /// <summary>
        ///     Makes the character rest
        /// </summary>
        public static void Rest(IMemoryAPI fface)
        {
            if (!fface.Player.Status.Equals(Status.Healing))
            {
                fface.Windower.SendString(Constants.RestingOn);
                TimeWaiter.Pause(50);
            }
        }

        /// <summary>
        ///     Makes the character stop resting
        /// </summary>
        public static void Stand(IMemoryAPI fface)
        {
            if (fface.Player.Status.Equals(Status.Healing))
            {
                fface.Windower.SendString(Constants.RestingOff);
                TimeWaiter.Pause(50);
            }
        }

        /// <summary>
        ///     Switches the player to attack mode on the current unit
        /// </summary>
        public static void Engage(IMemoryAPI fface)
        {
            if (!fface.Player.Status.Equals(Status.Fighting))
            {
                fface.Windower.SendString(Constants.AttackTarget);
            }
        }

        /// <summary>
        ///     Stop the character from fight the target
        /// </summary>
        public static void Disengage(IMemoryAPI fface)
        {
            if (fface.Player.Status.Equals(Status.Fighting))
            {
                fface.Windower.SendString(Constants.AttackOff);
            }
        }

        public static void StopRunning(IMemoryAPI fface)
        {
            fface.Navigator.Reset();
            TimeWaiter.Pause(100);
        }

        public static void SetTarget(IMemoryAPI fface, IUnit target)
        {
            if (!Config.Instance.EnableTabTargeting)
            {
                SetTargetUsingMemory(fface, target);
            }
            else
            {
                SetTargetByTabbing(fface, target);
            }
        }

        private static void SetTargetByTabbing(IMemoryAPI fface, IUnit target)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (target.Id != fface.Target.ID)
            {
                if (stopwatch.Elapsed >= TimeSpan.FromSeconds(1))
                {
                    break;
                }

                fface.Windower.SendKeyPress(Keys.TAB);
                TimeWaiter.Pause(200);
            }
        }

        private static void SetTargetUsingMemory(IMemoryAPI fface, IUnit target)
        {
            if (target.Id != fface.Target.ID)
            {
                fface.Target.SetNPCTarget(target.Id);
                fface.Windower.SendString("/ta <t>");
            }
        }
    }
}
