using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.ActionRules
{
    public class ActionContext
    {
        public BattleAbility BattleAbility { get; set; }
        public IMemoryAPI MemoryAPI { get; set; }
    }
}
