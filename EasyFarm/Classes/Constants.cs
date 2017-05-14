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
        /// Command for disengaging
        /// </summary>
        public const string AttackOff = "/attack off";

        /// <summary>
        /// Command for engaging
        /// </summary>
        public const string AttackTarget = "/attack <t>";

        /// <summary>
        /// Min distance to stand when attacking an opponent.
        /// </summary>
        public const int MeleeDistance = 3;

        /// <summary>
        /// Max difference an attackable mob can be before being considered unreachable.
        /// </summary>
        public const int HeightThreshold = 5;

        /// <summary>
        /// The max distance a mob will be recoginized by the bot.
        /// </summary>
        public const double DetectionDistance = 17;

        /// <summary>
        /// Command for resting
        /// </summary>
        public const string RestingOn = "/heal on";

        /// <summary>
        /// Command for stopping resting
        /// </summary>
        public const string RestingOff = "/heal off";

        /// <summary>
        /// Each spell takes 5 seconds to cast after a previous spell has been casted.
        /// </summary>
        public const int GlobalSpellCooldown = 1000;

        /// <summary>
        /// The upper limit of the spawn array. (Monsters, NPCs, Players)
        /// </summary>
        public const int UnitArrayMax = 4096;

        /// <summary>
        /// Controls the wait time between checks on the unit array to control performance.
        /// </summary>
        public const double UnitArrayCheckRate = 1;

        /// <summary>
        /// The maximum priority for states in the FSM. 
        /// </summary>
        public const int MaxPriority = int.MaxValue;

        /// <summary>
        /// The maximum number of trusts allowed in the party.
        /// </summary>
        public static int TrustPartySize = 5;
    }
}