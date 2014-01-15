using EasyFarm.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Behavior
{
    class BehaviorTree
    {
        GameState State = null;

        public BehaviorTree(ref GameState State)
        {
            this.State = State;
        }

        public void Run()
        {

        }
    }
}
