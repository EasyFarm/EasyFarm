using MemoryAPI;

namespace EasyFarm.Tests.TestTypes
{
    public class FakeTimer : ITimerTools
    {
        public int ActionRecast { get; set; }

        public int GetAbilityRecast(int index)
        {
            return ActionRecast;
        }

        public int GetSpellRecast(int index)
        {
            return ActionRecast;
        }
    }
}
