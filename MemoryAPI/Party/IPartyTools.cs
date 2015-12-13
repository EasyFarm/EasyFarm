namespace MemoryAPI
{
    public interface IPartyTools
    {
        int AllianceLeaderID { get; }
        bool Invited { get; }
        int Party0Count { get; }
        int Party0LeaderID { get; }
        int Party0Visible { get; }
        int Party1Count { get; }
        int Party1LeaderID { get; }
        int Party1Visible { get; }
        int Party2Count { get; }
        int Party2LeaderID { get; }
        int Party2Visible { get; }
    }
}