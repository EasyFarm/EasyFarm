using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class HasEnoughMpActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            var memory = context.MemoryAPI;
            return action.Ability.MpCost <= memory.Player.MPCurrent;
        }
    }
}
