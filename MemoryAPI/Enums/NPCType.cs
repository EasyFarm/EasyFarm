namespace MemoryAPI
{
    /// <summary>
    /// Type of NPC returned from GetNPCType
    /// </summary>
    public enum NpcType : byte
    {
        PC = 0x01,
        NPC = 0x02,
        Self = 0x0D,
        Mob = 0x10,
        InanimateObject,
    }
}