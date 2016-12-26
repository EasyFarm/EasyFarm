using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class HasEnoughTpActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            var memory = context.MemoryAPI;
            return action.Ability.TpCost <= memory.Player.TPCurrent;
        }
    }
}
