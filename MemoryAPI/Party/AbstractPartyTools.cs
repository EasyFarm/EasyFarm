namespace MemoryAPI
{
    public abstract class AbstractPartyTools : IPartyTools
    {
        public virtual int AllianceLeaderID { get; }
        public virtual bool Invited { get; }
        public virtual int Party0Count { get; }
        public virtual int Party0LeaderID { get; }
        public virtual int Party0Visible { get; }
        public virtual int Party1Count { get; }
        public virtual int Party1LeaderID { get; }
        public virtual int Party1Visible { get; }
        public virtual int Party2Count { get; }
        public virtual int Party2LeaderID { get; }
        public virtual int Party2Visible { get; }
    }
}