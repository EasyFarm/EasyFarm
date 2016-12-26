using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class IsEnabledActionRule : IActionRule
    {
        public bool IsValid(BattleAbility action) => action.IsEnabled;
    }
}
