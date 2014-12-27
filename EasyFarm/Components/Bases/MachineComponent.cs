
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

namespace EasyFarm.Components
{
    public abstract class MachineComponent : IMachineComponent, IComparable<MachineComponent>
    {
        /// <summary>
        /// Is this component enabled?
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The priority of the component. 
        /// </summary>
        public int Priority { get; set; }

        public abstract bool CheckComponent();

        public abstract void EnterComponent();

        public abstract void RunComponent();

        public abstract void ExitComponent();

        public int CompareTo(MachineComponent other)
        {
            return -this.Priority.CompareTo(other.Priority);
        }
    }
}
