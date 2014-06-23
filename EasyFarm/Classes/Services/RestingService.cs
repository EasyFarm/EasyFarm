
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

        public FFACE Session { get; set; }

        private static RestingService _restingService;

        private RestingService(ref FFACE session)
        {
            this.Session = session;
        }

        /// <summary>
        /// Returns the resting service set to this version of fface
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static RestingService GetInstance(FFACE session)
        {
            return _restingService ?? (_restingService = new RestingService(ref session));
        }

        /// <summary>
        /// Returns a resting service object that may not have it's fface session set.
        /// </summary>
        /// <returns></returns>
        public static RestingService GetInstance()
        {
            return _restingService;
        }

        /// <summary>
        /// Makes the character stop resting
        /// </summary>
        public void Off()
        {
            if (IsResting) { Session.Windower.SendString(RESTING_OFF); }
        }

        /// <summary>
        /// Makes the character rest
        /// </summary>
        public void On()
        {
            if (!IsResting) { Session.Windower.SendString(RESTING_ON); }
        }

        /// <summary>
        /// Is our player resting?
        /// </summary>
        public bool IsResting 
        {
            get
            {
                return Session.Player.Status == Status.Healing;
            }
        }
    }
}
