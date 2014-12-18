using EasyFarm.States;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.States
{
    [StateAttribute(priority: 1)]
    public class TargetEnemyState : BaseState
    {
        public Unit Target
        {
            get { return AttackState.TargetUnit; }
            set { AttackState.TargetUnit = value; }
        }

        public TargetEnemyState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            if (Target == null) return false;
            return Target.ID != FFACE.Target.ID;
        }

        public override void EnterState() { }

        public override void RunState()
        {
            if (Target == null || !Target.IsActive) return;

            // Face the target
            FFACE.Navigator.FaceHeading(Target.ID);

            // Check correct target
            ftools.CombatService.Disengage();

            // Target the target
            ftools.CombatService.TargetUnit(Target);
        }

        public override void ExitState() { }
    }
}
