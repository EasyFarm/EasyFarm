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
    public class WeaponskillState : CombatBaseState
    {
        private readonly Executor _executor;

        public WeaponskillState(IMemoryAPI fface) : base(fface)
        {
            _executor = new Executor(fface);
        }

        public override bool CheckComponent()
        {
            if (new RestState(fface).CheckComponent()) return false;

            if (!UnitFilters.MobFilter(fface, Target)) return false;

            // Use skill if we are engaged. 
            return fface.Player.Status.Equals(Status.Fighting);
        }

        public override void RunComponent()
        {
            // Check engaged
            // FIXED: no longer return on not engage but don't execute 
            // these moves instead. Fixes the bot not attacking things 
            // from move than 30 yalms problem. 
            if (fface.Player.Status.Equals(Status.Fighting))
            {
                // Grab the first weaponskill or null. 
                var weaponskill = Config.Instance.BattleLists["Weaponskill"]
                    .Actions.FirstOrDefault();

                // See if they the user set a weaponskill. 
                if (weaponskill == null) return;

                // Perform the weaponskill if it is valid. 
                if (ActionFilters.TargetedFilter(fface, weaponskill, Target))
                {
                    _executor.UseTargetedAction(weaponskill, Target);
                }
            }
        }
    }
}