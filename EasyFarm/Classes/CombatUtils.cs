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

using FFACETools;

namespace EasyFarm.Classes
{
    public class CombatUtils
    {
        /// <summary>
        ///     Switches the player to attack mode on the current unit
        /// </summary>
        public static void Engage(FFACE fface)
        {
            if (!fface.Player.Status.Equals(Status.Fighting))
            {
                fface.Windower.SendString(Constants.AttackTarget);
            }
        }

        /// <summary>
        ///     Stop the character from fight the target
        /// </summary>
        public static void Disengage(FFACE fface)
        {
            if (fface.Player.Status.Equals(Status.Fighting))
            {
                fface.Windower.SendString(Constants.AttackOff);
            }
        }
    }
}