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
