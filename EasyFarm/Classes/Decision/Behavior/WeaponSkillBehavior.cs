using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class WeaponSkillBehavior : Behavior
    {
        private GameEngine _engine;

        public WeaponSkillBehavior(ref GameEngine engine)
        {
            this._engine = engine;
        }

        public override bool CanExecute()
        {
            return 
                _engine.FFInstance.Instance.Player.Status == FFACETools.Status.Fighting
                && _engine.FFInstance.Instance.Player.TPCurrent >= 100                                 
                && _engine.Config.WeaponInfo.WeaponSkill.Ability.IsValidName
                && _engine.TargetData.TargetUnit.HPPCurrent <= _engine.Config.WeaponInfo.WeaponSkill.HPTrigger 
                && _engine.TargetData.TargetUnit.Distance < _engine.Config.WeaponInfo.WeaponSkill.DistanceTrigger;
        }

        public override TerminationStatus Execute()
        {
            var skill = _engine.PlayerActions.WeaponSkill.Ability;
            _engine.Combat.MaintainHeading();
            _engine.AbilityExecutor.UseAbility(skill);

            if (CanExecute())
                return TerminationStatus.Failed;
            else
                return TerminationStatus.Success;
        }
    }
}
