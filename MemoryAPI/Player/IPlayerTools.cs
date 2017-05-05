using MemoryAPI.Navigation;

namespace MemoryAPI
{
    public interface IPlayerTools
    {
        float CastPercentEx { get; }
        int HPPCurrent { get; }
        int ID { get; }       
        int MPCurrent { get; }
        int MPPCurrent { get; }
        string Name { get; }
        Position Position { get; }
        float PosX { get; }
        float PosY { get; }
        float PosZ { get; }
        Structures.PlayerStats Stats { get; }
        Status Status { get; }
        StatusEffect[] StatusEffects { get; }        
        int TPCurrent { get; }       
        Zone Zone { get; }
        Job Job { get; }
        Job SubJob { get; }
    }
}