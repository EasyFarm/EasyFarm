
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
        
        private Config Config;
        private FFACETools.FFACE.WindowerTools WTools;
        private PlayerData PlayerData;
        private Classes.GameEngine m_gameEngine;

        public RestingService(ref Classes.GameEngine m_gameEngine)
        {
            this.m_gameEngine = m_gameEngine;
            this.PlayerData = m_gameEngine.PlayerData;
            this.WTools = m_gameEngine.FFInstance.Instance.Windower;
            this.Config = m_gameEngine.Config;
        }

        /// <summary>
        /// Makes the character stop resting
        /// </summary>
        public void Off()
        {
            if (PlayerData.IsResting) { WTools.SendString(RESTING_OFF); }
        }

        /// <summary>
        /// Makes the character rest
        /// </summary>
        public void On()
        {
            if (!PlayerData.IsResting) { WTools.SendString(RESTING_ON); }
        }
    }
}
