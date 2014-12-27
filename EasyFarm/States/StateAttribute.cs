
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.States
{
    /// <summary>
    /// Marks state classes for autolocation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class StateAttribute : Attribute
    {
        readonly bool enabled = false;

        readonly int priority = 0;

        public StateAttribute(bool enabled = true, int priority = 0)
        {
            this.enabled = enabled;
            this.priority = priority;
        }

        public bool Enabled
        {
            get { return enabled; }
        }

        public int Priority
        { 
            get { return priority; } 
        }
    }
}
