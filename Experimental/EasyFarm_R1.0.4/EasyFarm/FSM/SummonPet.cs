using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.Engine;

namespace EasyFarm.FSM
{
    class SummonPet : BaseState
    {
        public SummonPet(ref GameState GameState) : base(ref GameState) { }

        public override bool CheckState()
        {
            return !GameState.IsTraveling && !GameState.IsResting;
        }

        public override void EnterState()
        {
            GameState.FFInstance.Instance.Navigator.Reset();
        }

        public override void RunState()
        {
            
        }

        public override void ExitState()
        {
            throw new NotImplementedException();
        }
    }
}
