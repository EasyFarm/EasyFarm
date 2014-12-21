
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

namespace EasyFarm.Components
{
    /// <summary>
    /// Changes our target once the target becomes invalid
    /// </summary>
    public class TargetInvalidComponent : BaseComponent
    {
        public TargetInvalidComponent(FFACE fface) : base(fface) { }

        public override bool CheckComponent()
        {
            if (AttackContainer.TargetUnit == null) return true;
            if (!ftools.UnitService.IsValid(AttackContainer.TargetUnit)) return true;
            return false;
        }

        public override void EnterComponent() { }

        public override void RunComponent()
        {
            var Target = ftools.UnitService.GetTarget(
                UnitFilters.MobFilter(FFACE), x => x.Distance);

            AttackContainer.TargetUnit = Target;

            App.Current.Dispatcher.Invoke(() =>
               Logger.Write.StateRun("Now targeting " + Target.Name + " : " + Target.ID));
        }

        public override void ExitComponent() { }
    }
}
