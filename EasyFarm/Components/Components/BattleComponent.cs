
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

using EasyFarm.Classes;
using FFACETools;
using System.Linq;

namespace EasyFarm.Components
{
    /// <summary>
    /// A class for defeating monsters.
    /// </summary>
    public class BattleComponent : MachineComponent
    {
        private FFACE _fface;
        private Executor Executor;

        /// <summary>
        /// Who we are trying to kill currently
        /// </summary>
        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public BattleComponent(FFACE fface)
        {
            this._fface = fface;
            this.Executor = new Executor(fface);
        }

        public override bool CheckComponent()
        {
            // target null or dead. 
            if (Target == null || Target.IsDead || Target.ID == 0) return false;

            // Mobs has not been pulled if pulling moves are available. 
            if (!AttackContainer.FightStarted) return false;

            // Engage is enabled and we are not engaged. We cannot proceed. 
            if (Config.Instance.IsEngageEnabled)
                return _fface.Player.Status.Equals(Status.Fighting);
            // Engage is not checked, so just proceed to battle. 
            else return true;
        }

        public override void EnterComponent()
        {
            RestingUtils.Stand(_fface);
            _fface.Navigator.Reset();
        }

        public override void RunComponent()
        {               
            // Cast only one action to prevent blocking curing. 
            var action = Config.Instance.BattleLists["Battle"].Actions
                .Where(x => ActionFilters.TargetedFilter(_fface, x, Target))
                .FirstOrDefault();

            if (action != null)
            {
                Executor.UseTargetedAction(action, Target);
            }
        }
    }
}
