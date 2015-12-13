namespace MemoryAPI
{
    public abstract class AbstractTargetTools : ITargetTools
    {
        public virtual int ID { get; }
        public abstract bool SetNPCTarget(int index);
    }
}