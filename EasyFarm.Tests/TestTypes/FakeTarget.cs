using MemoryAPI;

namespace EasyFarm.Tests.Classes
{
    public class FakeTarget : ITargetTools
    {
        public int ID { get; }

        public bool SetNPCTarget(int index)
        {
            return true;
        }
    }
}