using MemoryAPI;
using MemoryAPI.Navigation;

namespace MemoryAPI.Tests
{
    public class FakePlayer : IPlayerTools
    {
        public float CastPercentEx { get; set; }
        public int HPPCurrent { get; set; }
        public int ID { get; set; }
        public int MPCurrent { get; set; }
        public int MPPCurrent { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public Structures.PlayerStats Stats { get; set; }
        public Status Status { get; set; }
        public StatusEffect[] StatusEffects { get; set; }
        public int TPCurrent { get; set; }
        public Zone Zone { get; set; }
    }
}
