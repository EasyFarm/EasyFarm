using System;
using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class RecastPeriodActionRule : IActionRule
    {
        public bool IsValid(BattleAbility action)
        {
            return action.Recast == 0 || action.LastCast <= DateTime.Now;
        }
    }
}
