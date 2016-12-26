using System;
using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class RecastPeriodActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            return action.Recast == 0 || action.LastCast <= DateTime.Now;
        }
    }
}
