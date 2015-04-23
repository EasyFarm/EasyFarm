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
using FFACETools;

namespace EasyFarm.Components
{
    public class HealingComponent : MachineComponent
    {
        private readonly Executor _executor;
        private readonly FFACE _fface;

        public HealingComponent(FFACE fface)
        {
            _fface = fface;
            _executor = new Executor(fface);
        }

        public override bool CheckComponent()
        {
            if (new RestComponent(_fface).CheckComponent()) return false;

            if (!Config.Instance.BattleLists["Healing"].Actions
                .Any(x => ActionFilters.BuffingFilter(_fface, x)))
                return false;

            return true;
        }

        public override void EnterComponent()
        {
            // Stop resting. 
            if (_fface.Player.Status.Equals(Status.Healing))
            {
                _fface.Windower.SendString(Constants.RestingOff);
            }

            // Stop moving. 
            _fface.Navigator.Reset();
        }

        public override void RunComponent()
        {
            // Get the list of healing abilities that can be used.
            var usableHealingMoves = Config.Instance.BattleLists["Healing"].Actions
                .Where(x => ActionFilters.BuffingFilter(_fface, x))
                .ToList();

            // Check if we have any moves to use. 
            if (usableHealingMoves.Count > 0)
            {
                // Check for actions available
                var action = usableHealingMoves.FirstOrDefault();
                if (action == null)
                {
                    return;
                }

                // Create an ability from the name and launch the move. 
                var healingMove = App.AbilityService.CreateAbility(action.Name);
                _executor.UseBuffingAction(healingMove);
            }
        }
    }
}