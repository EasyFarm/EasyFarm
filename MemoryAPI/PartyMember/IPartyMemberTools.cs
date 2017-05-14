using MemoryAPI.Navigation;

namespace MemoryAPI
{
    public interface IPartyMemberTools
    {        
        bool UnitPresent { get; }

        int ServerID { get; }

        string Name { get; }

        int HPCurrent { get; }

        int HPPCurrent { get; }

        int MPCurrent { get; }

        int MPPCurrent { get; }

        int TPCurrent { get; }

        Job Job { get; }

        Job SubJob { get; }

        NpcType NpcType { get; }
    }
}