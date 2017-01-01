using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Parsing;

namespace EasyFarm.ActionRules
{
    public class AbilityBlockedActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            var memory = context.MemoryAPI;
            return !ResourceHelper.IsAbility(action.Ability.AbilityType) ||
                   !ProhibitEffects.ProhibitEffectsAbility
                       .Intersect(memory.Player.StatusEffects).Any();
        }
    }
}