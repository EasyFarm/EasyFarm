using EasyFarm.Classes.Services;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes.Decision.State
{
    /// <summary>
    /// Changes our target once the target becomes invalid
    /// </summary>
    public class TargetInvalid : BaseState
    {
        public TargetInvalid(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return !FarmingTools.GetInstance(fface).UnitService
                .IsValid(FarmingTools.GetInstance(fface).TargetData.TargetUnit);
        }

        public override void EnterState() { }

        public override void RunState()
        {
            FarmingTools.GetInstance(fface).TargetData.TargetUnit = 
                FarmingTools.GetInstance(fface).UnitService.GetTarget();
        }

        public override void ExitState() { }
    }
}
