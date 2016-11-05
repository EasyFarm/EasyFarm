using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class SetFightingState : CombatState
    {
        public SetFightingState(IMemoryAPI fface) : base(fface) { }

        public override bool Check()
        {
            if (UnitFilters.MobFilter(fface, Target))
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
