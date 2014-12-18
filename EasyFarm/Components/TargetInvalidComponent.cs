
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

using EasyFarm.FarmingTool;
using EasyFarm.Logging;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool;


<<<<<<< HEAD:EasyFarm/Components/TargetInvalidComponent.cs
namespace EasyFarm.Components
{
    /// <summary>
    /// Changes our target once the target becomes invalid
    /// </summary>
    public class TargetInvalidComponent : BaseComponent
=======
namespace EasyFarm.States
{
    /// <summary>
    /// Changes our target once the target becomes invalid
    /// </summary>    
    [StateAttribute(priority: 3)]
    public class TargetInvalid : BaseState
>>>>>>> VM_and_State_AutoLocate_and_AttackState_Refactor:EasyFarm/States/TargetInvalid.cs
    {
        public TargetInvalidComponent(FFACE fface) : base(fface) { }

        public override bool CheckComponent()
        {
<<<<<<< HEAD:EasyFarm/Components/TargetInvalidComponent.cs
            if(AttackComponent.TargetUnit == null) return true;
            if(!ftools.UnitService.IsValid(AttackComponent.TargetUnit)) return true;
=======
            if (AttackState.TargetUnit == null) return true;
            if (!ftools.UnitService.IsValid(AttackState.TargetUnit)) return true;
>>>>>>> VM_and_State_AutoLocate_and_AttackState_Refactor:EasyFarm/States/TargetInvalid.cs
            return false;
        }

        public override void EnterComponent() { }

        public override void RunComponent()
        {
<<<<<<< HEAD:EasyFarm/Components/TargetInvalidComponent.cs
            AttackComponent.TargetUnit = 
                ftools.UnitService.GetTarget(
                UnitFilters.MobFilter(FFACE), x => x.Distance);
=======
            var target = ftools.UnitService.GetTarget
                (UnitFilters.MobFilter(FFACE), x => x.Distance);

            if (target != null)
            {
                AttackState.TargetUnit = target;

                // Write target to log. 
                App.Current.Dispatcher.Invoke(() => Logger.Write.StateRun(
                    String.Join(" ", "Now targeting", target.ID, target.Name)));
            }
>>>>>>> VM_and_State_AutoLocate_and_AttackState_Refactor:EasyFarm/States/TargetInvalid.cs
        }

        public override void ExitComponent() { }
    }
}
