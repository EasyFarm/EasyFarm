using MemoryAPI;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class SimulationPartyMemberTools : IPartyMemberTools
    {
        private readonly byte _index;
        private readonly ISimulation _simulation;

        public SimulationPartyMemberTools(byte index, ISimulation simulation)
        {
            _index = index;
            _simulation = simulation;
        }

        public bool UnitPresent { get; }
        public int ServerID { get; }
        public string Name { get; }
        public int HPCurrent { get; }
        public int HPPCurrent { get; }
        public int MPCurrent { get; }
        public int MPPCurrent { get; }
        public int TPCurrent { get; }
        public Job Job { get; }
        public Job SubJob { get; }
        public NpcType NpcType { get; }
    }
}