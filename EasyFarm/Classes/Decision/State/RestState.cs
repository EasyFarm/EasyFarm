
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
*////////////////////////////////////////////////////////////////////

﻿using EasyFarm.Classes;
using EasyFarm.Classes.Services;
using FFACETools;

class RestState : BaseState
{
    public RestState(FFACE fface) : base(fface) { }

    public override bool CheckState()
    {
        return FarmingTools.GetInstance(fface).PlayerData.shouldRest && FarmingTools.GetInstance(fface).GameEngine.IsWorking;
    }

    public override void EnterState() { }

    public override void RunState()
    {
        if (!FarmingTools.GetInstance(fface).PlayerData.IsResting)
        {
            FarmingTools.GetInstance(fface).RestingService.On();
        }
    }

    public override void ExitState() { }
}
