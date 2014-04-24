using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFarm.Classes;

namespace EasyFarm.Classes
{
    // Fights a creature
    public class AttackSequence : Sequence
    {
        private GameEngine _engine;

        public AttackSequence(ref GameEngine engine)
        {
            this._engine = engine;
            this._behaviors.Add(new EngageSequence(ref engine));
            this._behaviors.Add(new AcquireMobBehavior(ref engine));
            this._behaviors.Add(new MoveToUnitBehavior(ref engine));
            this._behaviors.Add(new ActionBehavior(ref engine));
            this._behaviors.Add(new WeaponSkillBehavior(ref engine));
        }

        public override TerminationStatus Execute()
        {
            return base.Execute();
        }

        public override bool CanExecute()
        {
            // Should we attack?
            bool IsAttacking = !_engine.PlayerData.IsDead && (_engine.TargetData.IsValid &&
                    (_engine.PlayerData.IsFighting || _engine.PlayerData.IsAggroed ||
                    (_engine.PlayerData.IsFighting || !_engine.PlayerData.shouldRest)));

            return IsAttacking && _engine.IsWorking;
        }
    }
}
