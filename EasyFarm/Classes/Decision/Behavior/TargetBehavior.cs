using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Targets a creature
    /// </summary>
    public class TargetBehavior : Behavior
    {
        private GameEngine _engine;

        public TargetBehavior(ref GameEngine engine)
        {
            this._engine = engine;
        }

        public override TerminationStatus Execute()
        {
            _engine.FFInstance.Instance.Target.SetNPCTarget
                (_engine.TargetData.TargetUnit.ID);

            System.Threading.Thread.Sleep(50);

            if (_engine.TargetData.IsTarget) { return TerminationStatus.Success; }
            else { return TerminationStatus.Failed; }
        }

        public override bool CanExecute()
        {
            return !_engine.TargetData.IsTarget;
        }
    }
}
