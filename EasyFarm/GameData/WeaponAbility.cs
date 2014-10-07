
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

using System;
using ZeroLimits.XITools;

namespace EasyFarm.GameData
{
    public class WeaponAbility
    {
        /// <summary>
        /// What is its name?
        /// </summary>
        public String Name = String.Empty;

        /// <summary>
        /// Max distance we cna use a weaponskill at
        /// </summary>
        public double Distance = 0;

        /// <summary>
        /// Can we use the weaponskill?
        /// </summary>
        public bool Enabled = false;

        /// <summary>
        /// Mob hp needed inorder to use weaponskill
        /// </summary>
        public int Health = 0;

        /// <summary>
        /// The weaponskill
        /// </summary>
        public Ability Ability = new Ability();
    }
}
