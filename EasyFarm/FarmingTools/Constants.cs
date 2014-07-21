using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroLimits.FarmingTool
{
    public class Constants
    {
        /// <summary>
        /// Command for disengaging
        /// </summary>
        public const string ATTACK_OFF = "/attack off";

        /// <summary>
        /// Command for engaging
        /// </summary>
        public const string ATTACK_TARGET = "/attack <t>";

        /// <summary>
        /// Command for engaging a target
        /// </summary>
        public const string ATTACK_ON = "/attack on";

        /// <summary>
        /// Max time used to spend running to a mob before timing out.
        /// </summary>
        public const int RUN_DURATION = 3;

        /// <summary>
        /// Min distance to stand when attacking an opponent.
        /// </summary>
        public const double MELEE_DISTANCE = 3;

        /// <summary>
        /// Max difference an attackable mob can be before being considered unreachable. 
        /// </summary>
        public const double HEIGHT_THRESHOLD = 5;

        /// <summary>
        /// The max distance a mob will be recoginized by the bot. 
        /// </summary>
        public const double DETECTION_DISTANCE = 17;

        /// <summary>
        /// The minimum amount of tp needed to perform a weapon skill.
        /// </summary>
        public const int WEAPONSKILL_TP = 1000;

        /// <summary>
        /// Command for resting
        /// </summary>
        public const string RESTING_ON = "/heal on";

        /// <summary>
        /// Command for stopping resting
        /// </summary>
        public const string RESTING_OFF = "/heal off";

        /// <summary>
        /// The amount of time to wait to recast any spell that has failed to 
        /// help prevent spamming pulling moves. 
        /// </summary>
        public const int PULL_SPELL_RECAST_DURATION = 0;
    }
}
