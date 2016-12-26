using System.Collections.Generic;
using System.Linq;

namespace EasyFarm.Classes
{
    public interface IActionRule
    {
        bool IsValid(BattleAbility action);
    }

    public class ActionRulesComposite : IActionRule
    {
        private readonly List<IActionRule> Rules = new List<IActionRule>();

        public ActionRulesComposite()
        {
            this.Rules.Add(new IsEnabledActionRule());
            this.Rules.Add(new NameValidActionRule());
            this.Rules.Add(new UsageLimitActionRule());
        }

        public bool IsValid(BattleAbility action)
        {
            return Rules.All(x => x.IsValid(action));
        }
    }

    public class IsEnabledActionRule : IActionRule
    {
        public bool IsValid(BattleAbility action) => action.IsEnabled;
    }

    public class NameValidActionRule : IActionRule
    {
        public bool IsValid(BattleAbility action) => !string.IsNullOrWhiteSpace(action.Name);
    }

    public class UsageLimitActionRule : IActionRule
    {
        public bool IsValid(BattleAbility action)
        {
            if (action.UsageLimit == 0) return true;
            return action.Usages < action.UsageLimit;
        }
    }
}
