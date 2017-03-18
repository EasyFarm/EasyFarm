using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class ReserveTpActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            var memory = context.MemoryAPI;
            var tpReserve = new Range(action.TPReserveLow, action.TPReserveHigh);
            return tpReserve.NotSet() || tpReserve.InRange(memory.Player.TPCurrent);
        }
    }
}