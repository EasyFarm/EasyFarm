namespace MemoryAPI
{
    public interface ITargetTools
    {
        short HPPCurrent { get; }
        int ID { get; }
        bool IsLocked { get; }
        bool IsSub { get; }
        ushort Mask { get; }
        string Name { get; }
        float PosH { get; }
        IPosition Position { get; }
        float PosX { get; }
        float PosY { get; }
        float PosZ { get; }
        int ServerID { get; }
        Status Status { get; }
        int SubID { get; }
        ushort SubMask { get; }
        int SubServerID { get; }
        NPCType Type { get; }

        bool SetNPCTarget(int index);
    }
}