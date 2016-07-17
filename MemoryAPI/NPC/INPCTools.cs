using MemoryAPI.Navigation;

namespace MemoryAPI
{
    public interface INPCTools
    {
        int ClaimedID(int id);
        double Distance(int id);
        Position GetPosition(int id);
        short HPPCurrent(int id);
        bool IsActive(int id);
        bool IsClaimed(int id);
        bool IsRendered(int id);
        string Name(int id);
        NpcType NPCType(int id);
        float PosX(int id);
        float PosY(int id);
        float PosZ(int id);
        Status Status(int id);
        int PetID(int id);
    }
}