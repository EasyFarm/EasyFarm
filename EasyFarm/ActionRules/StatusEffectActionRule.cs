using System.Linq;
using System.Text.RegularExpressions;

namespace EasyFarm.ActionRules
{
    public class StatusEffectActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            var memory = context.MemoryAPI;

            var hasEffect = memory.Player.StatusEffects.Any(effect =>
                Regex.IsMatch(effect.ToString(),
                    action.StatusEffect.Replace(" ", "_"),
                    RegexOptions.IgnoreCase));

            return string.IsNullOrWhiteSpace(action.StatusEffect) ||
                   (hasEffect && action.TriggerOnEffectPresent) ||
                   (!hasEffect && !action.TriggerOnEffectPresent);                
        }
    }
}