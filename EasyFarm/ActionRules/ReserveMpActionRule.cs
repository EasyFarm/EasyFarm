using EasyFarm.Classes;

namespace EasyFarm.ActionRules
{
    public class ReserveMpActionRule : IActionRule
    {
        public bool IsValid(ActionContext context)
        {
            var action = context.BattleAbility;
            var memory = context.MemoryAPI;
            var mpReserve = new Range(action.MPReserveLow, action.MPReserveHigh);
            return mpReserve.InRange(memory.Player.MPPCurrent) || mpReserve.NotSet();
        }
    }
}