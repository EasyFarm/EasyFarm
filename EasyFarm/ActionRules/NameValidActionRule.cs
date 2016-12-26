using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class NameValidActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            return !string.IsNullOrWhiteSpace(action.Name);
        }
    }
}
