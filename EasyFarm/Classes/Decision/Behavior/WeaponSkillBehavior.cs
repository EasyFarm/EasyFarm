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
                _engine.Session.Instance.Player.Status == FFACETools.Status.Fighting
                && _engine.Session.Instance.Player.TPCurrent >= 100                                 
                && _engine.UserSettings.WeaponInfo.Ability.IsValidName
                && _engine.TargetData.TargetUnit.HPPCurrent <= _engine.UserSettings.WeaponInfo.Health 
                && _engine.TargetData.TargetUnit.Distance < _engine.UserSettings.WeaponInfo.Distance;
        }

        public override TerminationStatus Execute()
        {
            var skill = _engine.PlayerActions.WeaponSkill.Ability;
            _engine.CombatService.MaintainHeading();
            _engine.AbilityExecutor.UseAbility(skill);

            if (CanExecute())
                return TerminationStatus.Failed;
            else
                return TerminationStatus.Success;
        }
    }
}
