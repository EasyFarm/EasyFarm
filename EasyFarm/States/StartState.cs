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
    ///     Buffs the player.
    /// </summary>
    public class StartState : CombatState
    {
        public StartState(IMemoryAPI fface) : base(fface)
        {
            Executor = new Executor(fface);
        }

        public Executor Executor { get; set; }

        public override bool Check()
        {
            if (new RestState(fface).Check()) return false;

            // target dead or null. 
            if (!UnitFilters.MobFilter(fface, Target)) return false;

            // Return true if fight has not started. 
            return !Target.Status.Equals(Status.Fighting);
        }

        public override void Enter()
        {
            fface.Navigator.Reset();
        }

        public override void Run()
        {
            var usable = Config.Instance.BattleLists["Start"]
                .Actions.Where(x => ActionFilters.BuffingFilter(fface, x));

            // Execute moves at target. 
            Executor.UseBuffingActions(usable);
        }
    }
}