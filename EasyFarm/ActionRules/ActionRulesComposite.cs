using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class ActionRulesComposite : IActionRule
    {
        private readonly List<IActionRule> Rules = new List<IActionRule>();

        public ActionRulesComposite()
        {
            this.Rules.Add(new IsEnabledActionRule());
            this.Rules.Add(new NameValidActionRule());
            this.Rules.Add(new UsageLimitActionRule());
            this.Rules.Add(new RecastPeriodActionRule());
        }

        public bool IsValid(BattleAbility action)
        {
            return Rules.All(x => x.IsValid(action));
        }
    }
}
