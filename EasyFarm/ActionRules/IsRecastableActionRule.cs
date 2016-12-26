using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class IsRecastableActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            var memory = context.MemoryAPI;
            return AbilityUtils.IsRecastable(memory, action.Ability);
        }
    }
}