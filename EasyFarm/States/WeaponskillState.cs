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
    ///     Performs weaponskills on targets.
    /// </summary>
    public class WeaponskillState : CombatState
    {
        private readonly Executor _executor;

        public WeaponskillState(IMemoryAPI fface) : base(fface)
        {
            _executor = new Executor(fface);
        }

        public override bool Check()
        {
            if (new RestState(fface).Check()) return false;

            if (!UnitFilters.MobFilter(fface, Target)) return false;

            // Use skill if we are engaged. 
            return fface.Player.Status.Equals(Status.Fighting);
        }

        public override void Run()
        {
            // Check engaged
            // FIXED: no longer return on not engage but don't execute 
            // these moves instead. Fixes the bot not attacking things 
            // from move than 30 yalms problem. 
            if (!fface.Player.Status.Equals(Status.Fighting)) return;
            var weaponskill = Config.Instance.BattleLists["Weaponskill"].Actions
                .FirstOrDefault(x => ActionFilters.TargetedFilter(fface, x, Target));
            if (weaponskill == null) return;
            _executor.UseTargetedActions(new [] { weaponskill }, Target);
        }
    }
}