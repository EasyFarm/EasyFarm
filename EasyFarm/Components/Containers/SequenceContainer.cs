
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
    public class SequenceContainer : MachineContainer
    {
        private MachineComponent LastRan = null;

        public SequenceContainer() { }

        public override bool CheckComponent()
        {
            var ready = false;

            // Loop through all components and if one reports ready,
            // the attack container may run. 
            foreach (var Component in this.Components)
            {
                if (Component.Enabled)
                {
                    ready |= Component.CheckComponent();
                }
            }

            return ready;
        }

        public override void EnterComponent() { }

        public override void RunComponent()
        {
            lock (this.Components)
            {
                Components.Sort();

                foreach (var Component in this.Components)
                {
                    if (!Component.Enabled) continue;
                    if (Component.CheckComponent())
                    {
                        if (LastRan == null)
                        {
                            LastRan = Component;
                            // Run the enter command or otherwise it won't 
                            // trigger on the first run component. 
                            Component.EnterComponent();
                        }

                        if (LastRan != Component)
                        {
                            LastRan.ExitComponent();
                            LastRan = Component;
                            Component.EnterComponent();
                        }

                        Component.RunComponent();
                    }
                }
            }
        }

        public override void ExitComponent() { }
    }
}
