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
    public class HealingState : BaseState
    {
        private readonly Executor _executor;

        public HealingState(IMemoryAPI fface) : base(fface)
        {
            _executor = new Executor(fface);
        }

        public override bool Check()
        {
            if (new RestState(fface).Check()) return false;

            if (!Config.Instance.BattleLists["Healing"].Actions
                .Any(x => ActionFilters.BuffingFilter(fface, x)))
                return false;

            return true;
        }

        public override void Enter()
        {
            // Stop resting. 
            if (fface.Player.Status.Equals(Status.Healing))
            {
                Player.Stand(fface);
            }

            // Stop moving. 
            fface.Navigator.Reset();
        }

        public override void Run()
        {
            // Get the list of healing abilities that can be used.
            var usableHealingMoves = Config.Instance.BattleLists["Healing"].Actions
                .Where(x => ActionFilters.BuffingFilter(fface, x))
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