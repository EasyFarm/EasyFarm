using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.Classes;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Faces, Targets and Engages an enemy
    /// </summary>
    public class EngageSequence : Sequence
    {
        private GameEngine _engine;

        public EngageSequence(ref GameEngine engine)
        {
            this._engine = engine;
            this._behaviors.Add(new FaceMobBehavior(ref engine));
            this._behaviors.Add(new TargetBehavior(ref engine));
            this._behaviors.Add(new EngageBehavior(ref engine));
        }

        public override bool CanExecute()
        {
            return _engine.TargetData.IsValid;
        }

        public override TerminationStatus Execute()
        {
            return base.Execute();
        }
    }
}
