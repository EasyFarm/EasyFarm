using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes.Decision.State
{
    public class PostBattle : BaseState
    {
        public PostBattle(ref GameEngine engine) : base(ref engine) { }

        public override bool CheckState()
        {
            return _engine.TargetData.IsDead;
        }

        public override void EnterState() { }

        public override void RunState()
        {
            _engine.TargetData.TargetUnit = _engine.Units.GetTarget();
            _engine.CombatService.ExecuteActions(_engine.PlayerActions.EndList);
        }

        public override void ExitState() { }
    }
}
