using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class IsEnabledActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            return action.IsEnabled;
        }
    }
}
