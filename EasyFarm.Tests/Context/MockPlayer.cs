using EasyFarm.Context;
using MemoryAPI;

namespace EasyFarm.Tests.Context
{
    public class MockPlayer : IPlayer
    {
        public Status Status { get; set; }
        public int HppCurrent { get; set; }
        public bool HasAggro { get; set; }
        public Zone Zone { get; set; }
        public int Str { get; set; }
        public int MppCurrent { get; set; }
    }
}