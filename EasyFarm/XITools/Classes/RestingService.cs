
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
*////////////////////////////////////////////////////////////////////

using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroLimits.XITool.Classes
{
    /// <summary>
    /// Provides methods that allow the player to start resting or stop resting
    /// through the use of /heal on or /heal off. 
    /// </summary>
    public class RestingService
    {
        private FFACE _fface;

        public RestingService(FFACE session)
        {
            this._fface = session;
        }

        /// <summary>
        /// Makes the character stop resting
        /// </summary>
        public void EndResting()
        {
            if (_fface.Player.Status.Equals(Status.Healing)) 
            { 
                _fface.Windower.SendString(Constants.RESTING_OFF);
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Makes the character rest
        /// </summary>
        public void StartResting()
        {
            /*Fixed bug that caused program to stop attacking
             * Changed while to if
             */
            if (!_fface.Player.Status.Equals(Status.Healing)) 
            { 
                _fface.Windower.SendString(Constants.RESTING_ON);
                System.Threading.Thread.Sleep(50);
            }
        }
    }
}
