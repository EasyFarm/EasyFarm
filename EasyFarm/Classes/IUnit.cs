using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Classes
{
    public interface IUnit
    {
        int Id { get; set; }
        int ClaimedId { get; }
        double Distance { get; }
        Position Position { get; }
        short HppCurrent { get; }
        bool IsActive { get; }
        bool IsClaimed { get; }
        bool IsRendered { get; }
        string Name { get; }
        NpcType NpcType { get; }
        float PosX { get; }
        float PosY { get; }
        float PosZ { get; }
        Status Status { get; }    
        bool MyClaim { get; }
        bool HasAggroed { get; }
        bool IsDead { get; }
        bool PartyClaim { get; }
        double YDifference { get; }
        bool IsPet { get; }
    }
}