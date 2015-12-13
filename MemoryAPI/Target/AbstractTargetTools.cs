namespace MemoryAPI
{
    public abstract class AbstractTargetTools : ITargetTools
    {
        public virtual short HPPCurrent { get; }
        public virtual int ID { get; }
        public virtual bool IsLocked { get; }
        public virtual bool IsSub { get; }
        public virtual ushort Mask { get; }
        public virtual string Name { get; }
        public virtual float PosH { get; }
        public virtual IPosition Position { get; }
        public virtual float PosX { get; }
        public virtual float PosY { get; }
        public virtual float PosZ { get; }
        public virtual int ServerID { get; }
        public virtual Status Status { get; }
        public virtual int SubID { get; }
        public virtual ushort SubMask { get; }
        public virtual int SubServerID { get; }
        public virtual NPCType Type { get; }

        public abstract bool SetNPCTarget(int index);
    }
}