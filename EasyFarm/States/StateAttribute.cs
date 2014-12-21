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
