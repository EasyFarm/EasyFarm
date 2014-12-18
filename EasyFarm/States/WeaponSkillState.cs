using EasyFarm.UserSettings;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.States
{
    [StateAttribute(priority: 1)]
    public class WeaponSkillState : BaseState
    {
        public WeaponSkillState(FFACE fface) : base(fface) { }

        public Unit Target
        {
            get { return AttackState.TargetUnit; }
            set { AttackState.TargetUnit = value; }
        }

        public override bool CheckState()
        {
            return Target != null && FFACE.Player.Status.Equals(Status.Fighting) && !Target.IsDead;
        }

        public override void EnterState() { }

        public override void RunState()
        {
            // Check engaged
            // FIXED: no longer return on not engage but don't execute 
            // these moves instead. Fixes the bot not attacking things 
            // from move than 30 yalms problem. 
            if (FFACE.Player.Status.Equals(Status.Fighting))
            {
                // Weaponskill
                if (ShouldWeaponSkill)
                {
                    // Not sure if weapon skills or job abilities endure the same penalties that 
                    // spell do in regards to wait times. So I'm using zero's here. 
                    ftools.AbilityExecutor.CastLatency = 0;
                    ftools.AbilityExecutor.GlobalCooldown = 0;

                    // Cast the weaponskill. 
                    ftools.AbilityExecutor.UseAbility(Config.Instance.WeaponSkill.Ability);

                    // Rest casting parameters. 
                    ftools.AbilityExecutor.SetDefaults();
                }
            }
        }

        public override void ExitState() { }

        /// <summary>
        /// Can we perform our weaponskill on the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool ShouldWeaponSkill
        {
            get
            {
                return ActionFilters.WeaponSkillFilter(FFACE)
                    (Config.Instance.WeaponSkill, Target);
            }
        }
    }
}
