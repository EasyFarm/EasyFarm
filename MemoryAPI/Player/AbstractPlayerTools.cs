namespace MemoryAPI
{
    public abstract class AbstractPlayerTools : IPlayerTools
    {
        public virtual float CastCountDown { get; }
        public virtual float CastMax { get; }
        public virtual float CastPercent { get; }
        public virtual short CastPercentEx { get; }
        public virtual int HPCurrent { get; }
        public virtual int HPPCurrent { get; }
        public virtual int ID { get; }              
        public virtual int MPCurrent { get; }
        public virtual int MPPCurrent { get; }
        public virtual string Name { get; }
        public virtual byte PlayerServerID { get; }
        public virtual float PosH { get; }
        public virtual IPosition Position { get; }
        public virtual float PosX { get; }
        public virtual float PosY { get; }
        public virtual float PosZ { get; }
        public virtual Structures.PlayerStats Stats { get; }
        public virtual Status Status { get; }
        public virtual StatusEffect[] StatusEffects { get; }        
        public virtual int TPCurrent { get; }        
        public virtual Zone Zone { get; }
    }
}