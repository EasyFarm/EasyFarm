
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

﻿using System.Collections.Generic;
using System.Linq;
using FFACETools;
using System;
using System.Threading;

namespace ZeroLimits.XITool.Classes
{
    public class CombatService
    {
        private FFACE _fface;

        public CombatService(FFACE fface)
        {
            this._fface = fface;
        }

        /// <summary>
        /// Closes the distance between the character and 
        /// the target unit. 
        /// </summary>
        /// <param name="unit"></param>
        public void MoveToUnit(Unit unit, double distance)
        {
            // If the target is out of range move into range.
            if (_fface.Navigator.DistanceTo(unit.Position) > distance)
            {
                // Save old tolerance
                var old = _fface.Navigator.DistanceTolerance;

                // Set to max engagement distance.
                _fface.Navigator.DistanceTolerance = distance;

                // Goto target at max engagement distance.
                _fface.Navigator.GotoNPC(unit.ID);

                // Restore old tolerance. 
                _fface.Navigator.DistanceTolerance = old;
            }
        }

        /// <summary>
        /// Sets our target to unit and places our cursor on it. 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool TargetUnit(Unit unit)
        {
            if (TabToTarget(unit)) return true;

            if (unit != null)
            {
                _fface.Target.SetNPCTarget(unit.ID);
                _fface.Windower.SendString("/ta <t>");
                _fface.Navigator.FaceHeading(unit.ID);
            }

            return _fface.Target.ID == unit.ID;
        }

        private bool TabToTarget(Unit unit)
        {
            _fface.Navigator.SetViewMode(ViewMode.FirstPerson);
            _fface.Windower.SendString("/ta <t>");

            int count = 0;
            while (_fface.Target.ID != unit.ID && count++ < 10)
            {
                _fface.Windower.SendKeyPress(KeyCode.TabKey);
                Thread.Sleep(30);
            }

            return _fface.Target.ID == unit.ID;
        }

        /// <summary>
        /// Switches the player to attack mode on the current unit
        /// </summary>
        /// <param name="unit"></param>
        public void Engage()
        {
            if (!_fface.Player.Status.Equals(Status.Fighting))
            {
                _fface.Windower.SendString(Constants.ATTACK_TARGET);
            }
        }

        /// <summary>
        /// Stop the character from fight the target
        /// </summary>
        public void Disengage()
        {
            if (_fface.Player.Status.Equals(Status.Fighting))
            {
                _fface.Windower.SendString(Constants.ATTACK_OFF);
            }
        }
    }
}