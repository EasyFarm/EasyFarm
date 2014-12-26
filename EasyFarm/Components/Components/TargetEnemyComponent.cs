using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.Components
{
    public class TargetEnemyComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public CombatService Combat { get; set; }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public TargetEnemyComponent(FFACE fface)
        {
            this.FFACE = fface;
            this.Combat = new CombatService(fface);
        }

        public override bool CheckComponent()
        {
            if (Target == null) return false;
            return Target.ID != FFACE.Target.ID;
        }

        public override void EnterComponent() { }

        public override void RunComponent()
        {
            if (Target == null || !Target.IsActive) return;

            // Face the target
            this.FFACE.Navigator.FaceHeading(Target.ID);

            // Check correct target
            this.Combat.Disengage();

            // Target the target
            this.Combat.TargetUnit(Target);
        }

        public override void ExitComponent() { }        
    }
}
