using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Behavior
{
    class Selector : Node
    {
        public Selector(params Task[] Tasks)
        {
            this.Tasks = Tasks;
        }

        public bool Run() 
        {
            foreach (var Task in Tasks)
            {
                if (Task.Run())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
