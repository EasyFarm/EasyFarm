namespace MemoryAPI
{
    public abstract class AbstractTreasureItem : ITreasureItem
    {
        public virtual byte Count { get; set; }
        public virtual byte Flag { get; set; }
        public virtual bool IsWon { get; }
        public virtual short ItemID { get; set; }
        public virtual short MyLot { get; set; }
        public virtual TreasureStatus Status { get; set; }
        public virtual int TimeStamp { get; set; }
        public virtual short WinLot { get; set; }
        public virtual int WinPlayerID { get; set; }
        public virtual int WinPlayerSrvID { get; set; }
    }
}