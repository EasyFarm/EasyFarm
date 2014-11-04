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
    }
}
