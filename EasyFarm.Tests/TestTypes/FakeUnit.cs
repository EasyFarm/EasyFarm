using EasyFarm.Classes;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes
{
    public class FakeUnit : IUnit
    {
        public int Id { get; set; }
        public int ClaimedId { get; set; }
        public double Distance { get; set; }
        public Position Position { get; set; }
        public short HppCurrent { get; set; }
        public bool IsActive { get; set; }
        public bool IsClaimed { get; set; }
        public bool IsRendered { get; set; }
        public string Name { get; set; }
        public NpcType NpcType { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public Status Status { get; set; }
        public bool MyClaim { get; set; }
        public bool HasAggroed { get; set; }
        public bool IsDead { get; set; }
        public bool PartyClaim { get; set; }
        public double YDifference { get; set; }
        public bool IsPet { get; set; }
    }
}