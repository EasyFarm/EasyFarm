
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

﻿using EasyFarm.Classes;
using FFACETools;
using System.Linq;

namespace EasyFarm.Components
{
    public class HealingComponent : MachineComponent
    {
        private FFACE FFACE;
        private Executor Executor;

        public HealingComponent(FFACE fface)
        {
            this.FFACE = fface;
            this.Executor = new Executor(fface);
        }

        public override bool CheckComponent()
        {
            if (new RestComponent(FFACE).CheckComponent()) return false;

            if (!Config.Instance.BattleLists["Healing"].Actions
                .Any(x => ActionFilters.BuffingFilter(FFACE, x))) 
                return false;

            return true;
        }

        public override void EnterComponent()
        {
            // Stop resting. 
            if (FFACE.Player.Status.Equals(Status.Healing))
            {
                FFACE.Windower.SendString(Constants.RESTING_OFF);
            }

            // Stop moving. 
            FFACE.Navigator.Reset();
        }

        public override void RunComponent()
        {
            // Get the list of healing abilities that can be used.
            var UsableHealingMoves = Config.Instance.BattleLists["Healing"].Actions
                .Where(x => ActionFilters.BuffingFilter(FFACE, x))
                .ToList();

            // Check if we have any moves to use. 
            if (UsableHealingMoves.Count > 0)
            {
                // Check for actions available
                var Action = UsableHealingMoves.FirstOrDefault();
                if (Action == null) { return; }

                // Create an ability from the name and launch the move. 
                var HealingMove = App.AbilityService.CreateAbility(Action.Name);
                Executor.UseBuffingAction(HealingMove);
            }
        }        
    }
}
