using EasyFarm.Classes.Services;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes.Decision.State
{
    public class PostBattle : BaseState
    {
        public PostBattle(FFACE fface) : base(fface) { }

        public override bool CheckState()
        {
            return FarmingTools.GetInstance(fface)
                .TargetData.IsDead;
        }

        public override void EnterState() { }

        public override void RunState()
        {
            FarmingTools.GetInstance(fface).TargetData.TargetUnit = 
                FarmingTools.GetInstance(fface).UnitService.GetTarget();

            FarmingTools.GetInstance(fface).CombatService
                .ExecuteActions(FarmingTools.GetInstance(fface).PlayerActions.EndList);
        }

        public override void ExitState() { }
    }
}
