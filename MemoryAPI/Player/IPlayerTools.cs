namespace MemoryAPI
{
    public interface IPlayerTools
    {
        float CastCountDown { get; }
        float CastMax { get; }
        float CastPercent { get; }
        short CastPercentEx { get; }
        int HPCurrent { get; }
        int HPPCurrent { get; }
        int ID { get; }       
        int MPCurrent { get; }
        int MPPCurrent { get; }
        string Name { get; }
        byte PlayerServerID { get; }
        float PosH { get; }
        IPosition Position { get; }
        float PosX { get; }
        float PosY { get; }
        float PosZ { get; }
        Structures.PlayerStats Stats { get; }
        Status Status { get; }
        StatusEffect[] StatusEffects { get; }        
        int TPCurrent { get; }       
        Zone Zone { get; }       
    }
}