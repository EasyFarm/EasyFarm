using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class UsageLimitActionRule : IActionRule
    {
        public bool IsValid(BattleAbility action)
        {
            if (action.UsageLimit == 0) return true;
            return action.Usages < action.UsageLimit;
        }
    }
}
