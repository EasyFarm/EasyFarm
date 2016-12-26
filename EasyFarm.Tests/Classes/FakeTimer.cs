using MemoryAPI;

namespace MemoryAPI.Tests
{
    public class FakeTimer : ITimerTools
    {
        public int SpellRecast { get; set; }
        public int AbilityRecast { get; set; }

        public int GetAbilityRecast(int index)
        {
            return AbilityRecast;
        }

        public int GetSpellRecast(int index)
        {
            return SpellRecast;
        }
    }
}
