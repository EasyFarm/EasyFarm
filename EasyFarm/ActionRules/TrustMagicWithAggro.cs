using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Parsing;

namespace EasyFarm.ActionRules
{
    public class TrustMagicWithAggro : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var isTrustMagic = context.BattleAbility.Ability.Type == "Trust";
            var hasAggro = new UnitService(context.MemoryAPI).MobArray.Any(x => x.HasAggroed);
            return !(isTrustMagic && hasAggro);
        }
    }
}