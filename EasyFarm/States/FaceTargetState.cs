using EasyFarm.ViewModels;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.States
{
    [StateAttribute(priority:3)]
    public class FaceTargetState : BaseState
    {
        public Unit Target
        {
            get { return AttackState.TargetUnit; }
            set { AttackState.TargetUnit = value; }
        }

        public FaceTargetState(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            ViewModelBase.InformUser(FFACE.Navigator.HeadingError(FFACE.Player.PosH, Target.PosH).ToString());

            if (Target != null && !Target.IsDead)
            {
                var HeadingError = Math.Abs(FFACE.Navigator.HeadingError(FFACE.Player.PosH, Target.PosH));

                // 3: 3.2 or 2.8
                var IdealValue = 3.0;
                var Threshold = 0.5;
                var UpperValue = IdealValue + Threshold;
                var LowerValue = IdealValue - Threshold;

                return HeadingError < LowerValue || HeadingError > UpperValue;                
            }            

            return false;
        }

        public override void EnterState() { }

        public override void RunState() 
        {
            FFACE.Navigator.FaceHeading(Target.Position);
        }

        public override void ExitState() { }
    }
}
