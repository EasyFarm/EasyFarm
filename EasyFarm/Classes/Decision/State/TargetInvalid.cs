using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes.Decision.State
{
    /// <summary>
    /// Changes our target once the target becomes invalid
    /// </summary>
    public class TargetInvalid : BaseState
    {
        public TargetInvalid(ref GameEngine engine) : base(ref engine) { }

        public override bool CheckState()
        {
            return !_engine.Units.IsValid(_engine.TargetData.TargetUnit);
        }

        public override void EnterState() { }

        public override void RunState()
        {
            _engine.TargetData.TargetUnit = _engine.Units.GetTarget();
        }

        public override void ExitState() { }
    }
}
