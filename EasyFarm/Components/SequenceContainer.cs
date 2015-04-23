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

using System.Linq;

namespace EasyFarm.Components
{
    public class SequenceContainer : MachineContainer
    {
        private MachineComponent _lastRan;

        public override bool CheckComponent()
        {
            // Loop through all components and if one reports ready,
            // the attack container may run. 

            return Components.Any(x => x.Enabled && x.CheckComponent());
        }

        public override void EnterComponent()
        {
        }

        public override void RunComponent()
        {
            lock (Components)
            {
                Components.Sort();

                foreach (var component in Components)
                {
                    if (!component.Enabled) continue;
                    if (component.CheckComponent())
                    {
                        if (_lastRan == null)
                        {
                            _lastRan = component;
                            // Run the enter command or otherwise it won't 
                            // trigger on the first run component. 
                            component.EnterComponent();
                        }

                        if (_lastRan != component)
                        {
                            _lastRan.ExitComponent();
                            _lastRan = component;
                            component.EnterComponent();
                        }

                        component.RunComponent();
                    }
                }
            }
        }

        public override void ExitComponent()
        {
        }
    }
}