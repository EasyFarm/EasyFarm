namespace MemoryAPI
{
    public interface IPartyMemberTools
    {
        bool Active { get; }
        uint FlagMask { get; }
        int HPCurrent { get; }
        int HPPCurrent { get; }
        int ID { get; }
        int MPCurrent { get; }
        int MPPCurrent { get; }
        string Name { get; }
        int ServerID { get; }
        int TPCurrent { get; }
        Zone Zone { get; }

        string ToString();
    }
}