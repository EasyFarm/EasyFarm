using System.Collections.Generic;
using System.Linq;

namespace EasyFarm.ActionRules
{
    public class BuffingActionRules : IActionRule
    {
        private readonly List<IActionRule> rules = new List<IActionRule>();

        public BuffingActionRules()
        {
            rules.Add(new AbilityBlockedActionRule());
            rules.Add(new HasEnoughMpActionRule());
            rules.Add(new HasEnoughTpActionRule());
            rules.Add(new IsEnabledActionRule());
            rules.Add(new IsRecastableActionRule());
            rules.Add(new NameValidActionRule());
            rules.Add(new PlayerHealthActionRule());
            rules.Add(new RecastPeriodActionRule());
            rules.Add(new ReserveMpActionRule());
            rules.Add(new ReserveTpActionRule());
            rules.Add(new SpellBlockedActionRule());
            rules.Add(new StatusEffectActionRule());
            rules.Add(new UsageLimitActionRule());
        }

        public bool IsValid(ActionContext context)
        {
            return rules.All(x => x.IsValid(context));
        }
    }
}
