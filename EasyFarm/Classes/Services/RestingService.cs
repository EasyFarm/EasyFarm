
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

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

﻿using EasyFarm.Classes;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class RestingService
    {
        /// <summary>
        /// Command for resting
        /// </summary>
        const string RESTING_ON = "/heal on";

        /// <summary>
        /// Command for stopping resting
        /// </summary>
        const string RESTING_OFF = "/heal off";
        
        private GameEngine _gameEngine;

        public RestingService(ref GameEngine gameEngine)
        {
            this._gameEngine = gameEngine;
        }

        /// <summary>
        /// Makes the character stop resting
        /// </summary>
        public void Off()
        {
            if (_gameEngine.PlayerData.IsResting) { _gameEngine.Session.Instance.Windower.SendString(RESTING_OFF); }
        }

        /// <summary>
        /// Makes the character rest
        /// </summary>
        public void On()
        {
            if (!_gameEngine.PlayerData.IsResting) { _gameEngine.Session.Instance.Windower.SendString(RESTING_ON); }
        }
    }
}
