namespace MemoryAPI
{
    public abstract class AbstractFishTools : IFishTools
    {
        public virtual bool FishOnLine { get; }
        public virtual int HPCurrent { get; set; }
        public virtual int HPMax { get; }
        public virtual IFishID ID { get; }
        public virtual int OnLineTime { get; }
        public virtual RodAlign RodPosition { get; }
        public virtual int Timeout { get; set; }

        public abstract bool FightFish();
        public abstract bool SetFishTimeOut(short value);
        public abstract bool SetHP(int value);
    }
}