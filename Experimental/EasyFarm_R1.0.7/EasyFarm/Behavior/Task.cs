using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Behavior
{
    public class Task
    {
        public Func<object, bool> Action { get; set; }

        public Task(Func<object, bool> Action)
        {
            this.Action = Action;
        }

        public bool Run()
        {
            if (Action != null)
                return (bool)Action.Invoke(null);
            else
                return false;
        }
    }
}
