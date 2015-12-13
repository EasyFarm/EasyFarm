namespace MemoryAPI
{
    public interface ITreasureItem
    {
        byte Count { get; set; }
        byte Flag { get; set; }
        bool IsWon { get; }
        short ItemID { get; set; }
        short MyLot { get; set; }
        TreasureStatus Status { get; set; }
        int TimeStamp { get; set; }
        short WinLot { get; set; }
        int WinPlayerID { get; set; }
        int WinPlayerSrvID { get; set; }
    }
}