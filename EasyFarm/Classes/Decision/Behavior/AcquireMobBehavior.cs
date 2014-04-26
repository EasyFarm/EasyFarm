using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Pulls a creature.
    /// </summary>
    class AcquireMobBehavior : Behavior
    {
        private GameEngine _engine;

        public AcquireMobBehavior(ref GameEngine engine) : base()
        {
            this._engine = engine;
        }

        public override TerminationStatus Execute()
        {
            _engine.AbilityExecutor.ExecuteActions(_engine.PlayerActions.StartList, 
                () => _engine.CombatService.MaintainHeading());

            return TerminationStatus.Success;
        }

        public override bool CanExecute()
        {
            return _engine.TargetData.IsPullable;
        }
    }
}
