namespace EasyFarm.ActionRules
{
    public class UsageLimitActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            return action.UsageLimit == 0 || action.Usages < action.UsageLimit;
        }
    }
}
