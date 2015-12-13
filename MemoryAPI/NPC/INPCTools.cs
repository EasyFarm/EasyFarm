namespace MemoryAPI
{
    public interface INPCTools
    {
        int ClaimedID(int id);
        double Distance(int id);
        IPosition GetPosition(int id);
        byte[] GetRawNPCData(int mobindex, int start, int size);
        short HPPCurrent(int id);
        bool IsActive(int id);
        bool IsClaimed(int id);
        bool IsRendered(int id);
        string Name(int id);
        byte NPCBit(int id);
        NPCType NPCType(int id);
        int PetID(int id);
        float PosH(int id);
        float PosX(int id);
        float PosY(int id);
        float PosZ(int id);
        bool SetRawNPCData(int mobindex, int start, byte[] buffer, int size);
        Status Status(int id);
        short TPCurrent(int id);
    }
}