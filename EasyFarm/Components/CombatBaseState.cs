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

using EasyFarm.Classes;
using FFACETools;

namespace EasyFarm.Components
{
    /// <summary>
    ///     A class for defeating monsters.
    /// </summary>
    public class CombatBaseState : BaseState
    {
        static CombatBaseState()
        {
           IsFighting = false;
        }

        public CombatBaseState(FFACE fface)
        {
            FFACE = fface;
        }

        /// <summary>
        ///     Whether the fight has started or not.
        /// </summary>
        public static bool IsFighting { get; set; }

        /// <summary>
        ///     Who we are trying to kill currently
        /// </summary>
        public static Unit Target { get; set; }

        /// <summary>
        ///     The game session.
        /// </summary>
        public FFACE FFACE { get; set; }
    }
}