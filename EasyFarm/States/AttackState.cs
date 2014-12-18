
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
using System.Threading;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool;
using System.Linq;
using System.Collections.Generic;
using System;
using ZeroLimits.XITool.Classes;
using EasyFarm.ViewModels;
using EasyFarm.UserSettings;
using EasyFarm.Logging;

namespace EasyFarm.States
{
    /// <summary>
    /// A class for defeating monsters.
    /// </summary>

    [StateAttribute(priority: 1)]
    public class AttackState : BaseState
    {
        public static bool fightStarted = false;

        private static Unit _targetUnit = Unit.CreateUnit(0);

        /// <summary>
        /// Who we are trying to kill currently
        /// </summary>
        public static Unit TargetUnit
        {
            get { return _targetUnit; }
            set { _targetUnit = value; }
        }

        public AttackState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            bool success = false;

            // If we have a valid target
            if (ftools.UnitService.IsValid(TargetUnit))
            {
                // If we're alive
                if (!FFACE.Player.Status.Equals(Status.Dead1 | Status.Dead2))
                {
                    // If we're not injured
                    if (!new RestState(FFACE).CheckState())
                        success = true;
                }
            }            

            return success;
        }

        public override void EnterState()
        {
            ftools.RestingService.EndResting();
        }

        public override void RunState()
        {
            ///////////////////////////////////////////////////////////////////
            // Battle Enemy. 
            ///////////////////////////////////////////////////////////////////

            // Check engaged
            // FIXED: no longer return on not engage but don't execute 
            // these moves instead. Fixes the bot not attacking things 
            // from move than 30 yalms problem. 
            if (FFACE.Player.Status.Equals(Status.Fighting))
            {
                var UsableBattleMoves = Config.Instance.ActionInfo.BattleList
                    .Where(x => ActionFilters.AbilityFilter(FFACE)(x))
                    .ToList();

                // Cast all battle moves
                ftools.AbilityExecutor.ExecuteActions(UsableBattleMoves);
            }
        }

        public override void ExitState() { }
    }
}
