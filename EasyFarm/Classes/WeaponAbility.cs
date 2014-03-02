
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

﻿namespace EasyFarm.PlayerTools
{
    public class WeaponAbility : Ability
    {
        public WeaponAbility() { }
        
        public WeaponAbility(string name, double distance) : base(name) 
        {
            MaxDistance = distance;
        }
        
        /// <summary>
        /// Max distance we cna use a weaponskill at
        /// </summary>
        public double MaxDistance;

        /// <summary>
        /// Can we use the weaponskill?
        /// </summary>
        public bool IsEnabled;
    }
}
