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
    public class EngageState : BaseState
    {
        public EngageState(FFACE fface) : base(fface) { }

        public Unit Target
        {
            get { return AttackState.TargetUnit; }
            set { AttackState.TargetUnit = value; }
        }

        public override bool CheckState()
        {
            return Target != null && !FFACE.Player.Status.Equals(Status.Fighting) &&
                !Target.IsDead && Target.Distance < 25;
        }

        public override void EnterState() { }

        public override void RunState()
        {
            int engageCount = 0;
            while (!FFACE.Player.Status.Equals(Status.Fighting) && engageCount++ < 3)
            {
                ftools.CombatService.Engage();
            }
        }

        public override void ExitState() { }
    }
}
