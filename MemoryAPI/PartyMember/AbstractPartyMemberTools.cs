namespace MemoryAPI
{
    public abstract class AbstractPartyMemberTools : IPartyMemberTools
    {
        public virtual bool Active { get; }
        public virtual uint FlagMask { get; }
        public virtual int HPCurrent { get; }
        public virtual int HPPCurrent { get; }
        public virtual int ID { get; }
        public virtual int MPCurrent { get; }
        public virtual int MPPCurrent { get; }
        public virtual string Name { get; }
        public virtual int ServerID { get; }
        public virtual int TPCurrent { get; }
        public virtual Zone Zone { get; }
    }
}