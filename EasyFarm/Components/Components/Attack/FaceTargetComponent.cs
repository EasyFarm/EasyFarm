using EasyFarm.ViewModels;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.Components
{
    /// <summary>
    /// Faces the enemy every second. 
    /// </summary>
    public class FaceTargetComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public FaceTargetComponent(FFACE fface)
        { 
            this.FFACE = fface;
        }

        public override bool CheckComponent()
        {
            return Target != null && !Target.IsDead;
        }

        public override void EnterComponent() { }

        public override void RunComponent() 
        {
            FFACE.Navigator.FaceHeading(Target.Position);
        }

        public override void ExitComponent() { }
    }
}
