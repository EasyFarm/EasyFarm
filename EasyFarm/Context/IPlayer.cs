using MemoryAPI;

namespace EasyFarm.Context
{
    public interface IPlayer
    {
        Status Status { get; set; }
        int HppCurrent { get; set; }
        bool HasAggro { get; set; }
        Zone Zone { get; set; }
        int Str { get; set; }
        int MppCurrent { get; set; }
    }
}