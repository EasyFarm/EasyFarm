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

using System.Collections.Generic;

namespace EasyFarm.Components
{
    public abstract class MachineContainer : MachineComponent, IMachineContainer
    {
        protected MachineContainer()
        {
            Components = new List<MachineComponent>();
        }

        /// <summary>
        ///     List of components to run.
        /// </summary>
        public List<MachineComponent> Components { get; set; }

        public void AddComponent(MachineComponent component)
        {
            lock (Components)
            {
                Components.Add(component);
            }
        }

        public void RemoveComponent(int index)
        {
            lock (Components)
            {
                Components.RemoveAt(index);
            }
        }

        public void ClearComponents()
        {
            lock (Components)
            {
                Components.Clear();
            }
        }
    }
}