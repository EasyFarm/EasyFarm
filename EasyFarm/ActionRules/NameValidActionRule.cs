using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class NameValidActionRule : IActionRule
    {
        public bool IsValid(BattleAbility action) => !string.IsNullOrWhiteSpace(action.Name);
    }
}
