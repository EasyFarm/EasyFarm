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
                    if (LastRan != null) LastRan.ExitComponent();
                    if (!Component.Enabled) continue;
                    if (Component.CheckComponent())
                    {
                        if (LastRan == null) LastRan = Component;
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
