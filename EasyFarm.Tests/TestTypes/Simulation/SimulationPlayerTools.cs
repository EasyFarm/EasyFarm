using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class SimulationPlayerTools : IPlayerTools
    {
        private readonly ISimulation _simulation;

        public SimulationPlayerTools(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public float CastPercentEx { get; }
        public int HPPCurrent { get; }
        public int ID { get; }
        public int MPCurrent { get; }
        public int MPPCurrent { get; }
        public string Name { get; }
        public Position Position { get; }
        public float PosX { get; }
        public float PosY { get; }
        public float PosZ { get; }
        public Structures.PlayerStats Stats { get; }
        public Status Status { get; }
        public StatusEffect[] StatusEffects { get; }
        public int TPCurrent { get; }
        public Zone Zone { get; }
        public Job Job { get; }
        public Job SubJob { get; }
    }
}