
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

﻿using EasyFarm.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class Resting
    {
        /// <summary>
        /// Command for resting
        /// </summary>
        const string RESTING_ON = "/heal on";

        /// <summary>
        /// Command for stopping resting
        /// </summary>
        const string RESTING_OFF = "/heal off";

        public Resting(ref GameEngine Engine)
        {
            this.Engine = Engine;
            this.PlayerData = Engine.PlayerData;
        }

        /// <summary>
        /// Makes the character stop resting
        /// </summary>
        public void Off()
        {
            if (PlayerData.IsResting) { Engine.FFInstance.Instance.Windower.SendString(RESTING_OFF); }
        }

        /// <summary>
        /// Makes the character rest
        /// </summary>
        public void On()
        {
            if (!PlayerData.IsResting) { Engine.FFInstance.Instance.Windower.SendString(RESTING_ON); }
        }

        /// <summary>
        /// All information about the bot.
        /// </summary>
        private GameEngine Engine { get; set; }

        /// <summary>
        /// Details about the player
        /// </summary>
        private PlayerData PlayerData { get; set; }
    }
}
