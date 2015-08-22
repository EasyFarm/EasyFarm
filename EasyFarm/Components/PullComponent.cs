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
    public class PullComponent : BaseState
    {
        public PullComponent(FFACE fface)
        {
            FFACE = fface;
            Executor = new Executor(fface);
        }

        public FFACE FFACE { get; set; }
        public Executor Executor { get; set; }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        /// <summary>
        ///     Allow component to run when moves need to be triggered or
        ///     FightStarted state needs updating.
        /// </summary>
        /// <returns></returns>
        public override bool CheckComponent()
        {
            // Target not null, dead or empty.
            return (Target != null && !Target.IsDead && Target.Id != 0);
        }

        public override void EnterComponent()
        {
            FFACE.Navigator.Reset();
        }

        /// <summary>
        ///     Use pulling moves if applicable to make the target
        ///     mob aggressive to us.
        /// </summary>
        public override void RunComponent()
        {
            // Do not pull if we've done so already. 
            if (AttackContainer.FightStarted) return;

            // Only pull if we have moves. 
            if (Config.Instance.BattleLists["Pull"]
                .Actions.Any(x => x.IsEnabled))
            {
                var usable = Config.Instance.BattleLists["Pull"]
                    .Actions.Where(x => ActionFilters.TargetedFilter(FFACE, x, Target));

                Executor.UseTargetedActions(usable, Target);
            }
        }

        /// <summary>
        ///     Handle all cases of setting fight started to proper values
        ///     so other components can fire.
        /// </summary>
        public override void ExitComponent()
        {
            if (Target.Status.Equals(Status.Fighting))
                AttackContainer.FightStarted = true;
            // No moves in pull list, set FightStarted to true to let
            // other components who depend on it trigger. 
            else if (!Config.Instance.BattleLists["Pull"].Actions.Any(x => x.IsEnabled))
                AttackContainer.FightStarted = true;
            else
                AttackContainer.FightStarted = false;
        }
    }
}