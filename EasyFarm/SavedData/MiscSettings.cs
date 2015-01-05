
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.UserSettings
{
    public class MiscSettings
    {
        /// <summary>
        /// How far a player should detect a creature. 
        /// </summary>
        public double DetectionDistance = Constants.DETECTION_DISTANCE;
        
        /// <summary>
        /// How high or low a player should detect a creature. 
        /// </summary>
        public double HeightThreshold = Constants.HEIGHT_THRESHOLD;
        
        /// <summary>
        /// How close the player should be when attacking a creature. 
        /// </summary>
        public double MeleeDistance = Constants.MELEE_DISTANCE;

        /// <summary>
        /// The amount of time in seconds to wait before refiring a 
        /// ranged weapon. 
        /// </summary>
        public double RangedAttackDelay = 3;

        /// <summary>
        /// How far to go of the path for a unit. 
        /// </summary>
        public double WanderDistance = Constants.DETECTION_DISTANCE;

        /// <summary>
        /// Cast delay for laggy servers. 
        /// </summary>
        public int CastLatency = Constants.SPELL_CAST_LATENCY;

        /// <summary>
        /// Cast delay before casting next spell 
        /// (stops cannot use ability spam)
        /// </summary>
        public int GlobalCooldown = Constants.GLOBAL_SPELL_COOLDOWN;
    }
}
