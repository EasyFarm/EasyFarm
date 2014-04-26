using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Engages a mob
    /// </summary>
    public class EngageBehavior : Behavior
    {
        const string ATTACK_TARGET = "/attack <t>";

        private GameEngine _engine;

        public EngageBehavior(ref GameEngine engine) : base()
        {
            this._engine = engine;
        }

        public override bool CanExecute()
        {
            return _engine.TargetData.IsTarget && !_engine.PlayerData.IsFighting;
        }

        public override TerminationStatus Execute()
        {
            _engine.Session.Instance.Windower.SendString(ATTACK_TARGET);
            if (CanExecute())
                return TerminationStatus.Success;
            else
                return TerminationStatus.Failed;
        }
    }
}
