
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

<<<<<<< HEAD:EasyFarm/Components/Interfaces/BaseComponent.cs
namespace EasyFarm.Components
=======
namespace EasyFarm.States
>>>>>>> VM_and_State_AutoLocate_and_AttackState_Refactor:EasyFarm/States/BaseState.cs
{
    public abstract class BaseComponent : MachineComponent
    {
        protected static FFACE FFACE;
        protected static FTools ftools;

        public abstract override bool CheckComponent();
        public abstract override void EnterComponent();
        public abstract override void RunComponent();
        public abstract override void ExitComponent();

        public BaseComponent(FFACE fface)
        {
            if (FFACE == null) FFACE = fface;
            if (ftools == null)
            {
                ftools = new FTools(fface);
                ftools.UnitService.UnitFilter = UnitFilters.MobFilter(fface);
            }
        }      

        public int CompareTo(BaseComponent other)
        {
            return -this.Priority.CompareTo(other.Priority);
        }
    }
}