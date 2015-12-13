namespace MemoryAPI
{
    public interface ITargetTools
    {
        int ID { get; }
        bool SetNPCTarget(int index);
    }
}