using EasyFarm.Classes;
using MemoryAPI;
using System.Linq;

namespace EasyFarm.Components
{
    public class SetFightingState : CombatBaseState
    {
        public SetFightingState(FFACE fface) : base(fface) { }

        public override bool CheckComponent()
        {
            if (UnitFilters.MobFilter(FFACE, Target))
            {
                // No moves in pull list, set FightStarted to true to let
                // other components who depend on it trigger. 
                if (!Config.Instance.BattleLists["Pull"].Actions.Any(x => x.IsEnabled))
                {
                    return IsFighting = true;
                }
                else
                {
                    return IsFighting = Target.Status.Equals(Status.Fighting);
                }
            }

            return IsFighting = false;
        }
    }
}
