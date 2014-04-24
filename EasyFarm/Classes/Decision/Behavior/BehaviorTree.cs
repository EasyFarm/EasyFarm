using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    public class BehaviorTree : Selector
    {
        private GameEngine _engine;        

        public BehaviorTree(ref GameEngine engine) : base() 
        {
            this._engine = engine;
            this._behaviors.Add(new AttackSequence(ref engine));
        }

        public override TerminationStatus Execute()
        {
            return base.Execute();
        }

        public override bool CanExecute()
        {
            return _engine.IsWorking && !_engine.PlayerData.IsDead;
        }
    }
}
