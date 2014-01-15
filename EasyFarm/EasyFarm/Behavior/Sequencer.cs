using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Behavior
{
    class Sequencer : Node
    {
        public Sequencer(params Task[] Tasks)
        {
            this.Tasks = Tasks;
        }

        public bool Run()
        {
            foreach (var Task in Tasks)
            {
                if (!Task.Run())
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
