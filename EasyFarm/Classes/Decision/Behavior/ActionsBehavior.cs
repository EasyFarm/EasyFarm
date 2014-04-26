using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class ActionBehavior : Behavior
    {
        private GameEngine _engine;

        public ActionBehavior(ref GameEngine engine)
        {
            this._engine = engine;
        }

        public override bool CanExecute()
        {
            return _engine.PlayerData.IsFighting && _engine.TargetData.IsTarget && _engine.PlayerActions.HasBattleMoves;
        }

        public override TerminationStatus Execute()
        {
            var skill = _engine.PlayerActions.BattleList.First();

            _engine.CombatService.MaintainHeading();
            _engine.AbilityExecutor.UseAbility(skill);
            
            if (_engine.PlayerActions.AbilityRecastable(skill))
                return TerminationStatus.Failed;
            else
                return TerminationStatus.Success;
        }
    }
}
