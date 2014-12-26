
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

﻿using FFACETools;
using ZeroLimits.FarmingTool;
using System.Linq;
using ZeroLimits.XITool;
using ZeroLimits.XITool.Classes;
using EasyFarm.ViewModels;
using EasyFarm.UserSettings;

namespace EasyFarm.Components
{
    public class HealingComponent : BaseComponent
    {
        public HealingComponent(FFACE fface) : base(fface) { }

        public override bool CheckComponent()
        {
            if (new RestComponent(FFACE).CheckComponent()) return false;

            if (!Config.Instance.ActionInfo.HealingList
                .Any(x => ActionFilters.HealingFilter(FFACE, x))) 
                return false;

            return true;
        }

        public override void EnterComponent()
        {
            ftools.RestingService.EndResting();
            FFACE.Navigator.Reset();
        }

        public override void RunComponent()
        {
            // Get the list of healing abilities that can be used.
            var UsableHealingMoves = Config.Instance.ActionInfo.HealingList
                .Where(x => ActionFilters.HealingFilter(FFACE, x))
                .ToList();

            // Check if we have any moves to use. 
            if (UsableHealingMoves.Count > 0)
            {
                // Check for actions available
                var Action = UsableHealingMoves.FirstOrDefault();
                if (Action == null) { return; }

                // Create an ability from the name and launch the move. 
                var HealingMove = new AbilityService().CreateAbility(Action.Name);
                ftools.AbilityExecutor.UseAbility(HealingMove);
            }
        }

        public override void ExitComponent() { }
    }
}
