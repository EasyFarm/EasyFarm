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

using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    /// <summary>
    ///     Moves to target enemies.
    /// </summary>
    public class ApproachState : CombatState
    {
        public ApproachState(IMemoryAPI fface) : base(fface)
        {
        }

        public override bool Check()
        {
            if (new RestState(fface).Check()) return false;

            // target dead or null.
            if (!UnitFilters.MobFilter(fface, Target)) return false;

            // We should approach mobs that have aggroed or have been pulled. 
            if (Target.Status.Equals(Status.Fighting)) return true;

            // Get usable abilities. 
            var usable = Config.Instance.BattleLists["Pull"].Actions
                .Where(x => ActionFilters.BuffingFilter(fface, x));

            // Approach when there are no pulling moves available. 
            if (!usable.Any()) return true;

            // Approach mobs if their distance is close. 
            return Target.Distance < 8;
        }

        public override void Run()
        {
            Player.SwitchTarget(Target, fface);
            Player.ApproachMob(Target, fface);            
        }        
    }
}