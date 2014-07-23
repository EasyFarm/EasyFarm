
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

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
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITools;


namespace EasyFarm.State
{
    /// <summary>
    /// Changes our target once the target becomes invalid
    /// </summary>
    public class TargetInvalid : BaseState
    {
        public TargetInvalid(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return ftools.TargetData.TargetUnit == null || !ftools.UnitService.IsValid(ftools.TargetData.TargetUnit);
        }

        public override void EnterState() { }

        public override void RunState()
        {
            ftools.TargetData.TargetUnit = ftools.UnitService.GetTarget(UnitFilters.MobFilter(fface));
        }

        public override void ExitState() { }
    }
}
