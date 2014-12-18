
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

﻿using System;
using FFACETools;
using ZeroLimits.FarmingTool;
using EasyFarm.FarmingTool;
using EasyFarm.ViewModels;

namespace EasyFarm.States
{
    public abstract class BaseState : IComparable<BaseState>
    {
        public bool Enabled;
        public int Priority;
        protected static FFACE FFACE;
        protected static FTools ftools;

        public abstract bool CheckState();
        public abstract void EnterState();
        public abstract void RunState();
        public abstract void ExitState();

        public BaseState(FFACE fface)
        {
            if (FFACE == null) FFACE = fface;
            if (ftools == null)
            {
                ftools = new FTools(fface);
                ftools.UnitService.UnitFilter = UnitFilters.MobFilter(fface);
            }
        }

        public int CompareTo(BaseState other)
        {
            return -this.Priority.CompareTo(other.Priority);
        }
    }
}