namespace EasyFarm.ActionRules
{
    public class PlayerHealthActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            var memory = context.MemoryAPI;
            return (action.PlayerLowerHealth == 0 && action.PlayerUpperHealth == 0) ||
                   memory.Player.HPPCurrent <= action.PlayerUpperHealth &&
                   memory.Player.HPPCurrent >= action.PlayerLowerHealth;
        }
    }
}