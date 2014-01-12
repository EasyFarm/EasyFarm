using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Behavior
{
    class Node
    {
        public Task[] Tasks { get; set; }

        public Node(params Task[] Tasks)
        {
            this.Tasks = Tasks;
        }
    }
}
