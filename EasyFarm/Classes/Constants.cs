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

namespace EasyFarm.Classes
{
    public class Constants
    {
        /// <summary>
        ///     Command for disengaging
        /// </summary>
        public const string AttackOff = "/attack off";

        /// <summary>
        ///     Command for engaging
        /// </summary>
        public const string AttackTarget = "/attack <t>";

        /// <summary>
        ///     Command for engaging a target
        /// </summary>
        public const string AttackOn = "/attack on";

        /// <summary>
        ///     Max time used to spend running to a mob before timing out.
        /// </summary>
        public const int RunDuration = 3;

        /// <summary>
        ///     Min distance to stand when attacking an opponent.
        /// </summary>
        public const int MeleeDistance = 3;

        /// <summary>
        ///     Max difference an attackable mob can be before being considered unreachable.
        /// </summary>
        public const int HeightThreshold = 5;

        /// <summary>
        ///     The max distance a mob will be recoginized by the bot.
        /// </summary>
        public const double DetectionDistance = 17;

        /// <summary>
        ///     The minimum amount of tp needed to perform a weapon skill.
        /// </summary>
        public const int WeaponskillTp = 1000;

        /// <summary>
        ///     Command for resting
        /// </summary>
        public const string RestingOn = "/heal on";

        /// <summary>
        ///     Command for stopping resting
        /// </summary>
        public const string RestingOff = "/heal off";

        /// <summary>
        ///     The amount of time to wait to recast any spell that has failed to
        ///     help prevent spamming pulling moves.
        /// </summary>
        public const int PullSpellRecastDuration = 0;

        /// <summary>
        ///     One second for spells to fire when casted through
        ///     WindowerTools.SendString(command).
        /// </summary>
        public const int SpellCastLatency = 1000;

        /// <summary>
        ///     Each spell takes 5 seconds to cast after a previous spell
        ///     has been casted.
        /// </summary>
        public const int GlobalSpellCooldown = 1000;

        /// <summary>
        ///     Maximum range a spell may be casted.
        /// </summary>
        public const int SpellCastDistance = 21;

        /// <summary>
        ///     Maximum range for ranged attack.
        /// </summary>
        public const int RangedAttackMaxDistance = 25;

        /// <summary>
        ///     The upper limit of the spawn array. (Monsters, NPCs, Players)
        /// </summary>
        public const int UnitArrayMax = 2048;

        /// <summary>
        ///     The upper limit of the mob array. (Monsters, NPCS)
        /// </summary>
        public const int MobArrayMax = 768;

        /// <summary>
        ///     Controls the wait time between checks on the
        ///     unit array to control performance.
        /// </summary>
        public const double UnitArrayCheckRate = 1;
    }

    public class EnsureCastConstants
    {
        public const int SpellRecastAttempts = 3;
        public const double SpellRecastDelay = 0;
    }
}