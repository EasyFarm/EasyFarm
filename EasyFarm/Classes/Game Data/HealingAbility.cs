
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Class for Healing Abilities
    /// </summary>
    public class HealingAbility
    {
        public HealingAbility()
        {
            SetDefaults();
        }

        public void SetDefaults()
        { 
            IsEnabled = false;
            Name = "Empty";
            TriggerLevel = 0;
        }

        /// <summary>
        /// Can we use this abilitiy?
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// What is its name?
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The level to which we should use the ability
        /// </summary>
        public int TriggerLevel { get; set; }
    }
}
