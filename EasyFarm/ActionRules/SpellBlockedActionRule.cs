using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Parsing;

namespace EasyFarm.ActionRules
{
    public class SpellBlockedActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            var memory = context.MemoryAPI;
            return !ResourceHelper.IsSpell(action.Ability.AbilityType) ||
                   !ProhibitEffects.ProhibitEffectsSpell
                       .Intersect(memory.Player.StatusEffects).Any();

        }
    }
}