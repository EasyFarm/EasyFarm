using EasyFarm.UserSettings;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.Components
{
    /// <summary>
    /// Engages target enemies. 
    /// </summary>
    public class EngageComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public CombatService Combat { get; set; }

        public EngageComponent(FFACE fface)
        {
            this.FFACE = fface;
            this.Combat = new CombatService(fface);
        }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public override bool CheckComponent()
        {
            return Target != null && !FFACE.Player.Status.Equals(Status.Fighting) &&
                !Target.IsDead && Target.Distance < 25;
        }

        public override void EnterComponent() { }

        public override void RunComponent()
        {
            this.Combat.Engage();
        }

        public override void ExitComponent() { }
    }
}
