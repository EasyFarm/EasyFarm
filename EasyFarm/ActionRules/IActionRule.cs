using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public interface IActionRule
    {
        bool IsValid(BattleAbility action);
    }
}
