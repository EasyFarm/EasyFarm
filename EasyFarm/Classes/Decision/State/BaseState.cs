
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

﻿using System;
using EasyFarm.Classes;

public abstract class BaseState : IComparable<BaseState>
{
    public bool Enabled;
    public int Priority;
    protected GameEngine _engine;
    
    public abstract bool CheckState();
    public abstract void EnterState();
    public abstract void RunState();
    public abstract void ExitState();

    public BaseState(ref GameEngine engine)
    {
        this._engine = engine;
    }

    public int CompareTo(BaseState other)
    {
        return -this.Priority.CompareTo(other.Priority);
    }
}