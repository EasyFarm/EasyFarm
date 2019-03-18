using System;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class SimulationNavigator : INavigatorTools
    {
        private readonly ISimulation _simulation;

        public SimulationNavigator(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public double DistanceTolerance { get; set; }
        public void FaceHeading(Position position)
        {
            throw new NotImplementedException();
        }

        public void GotoWaypoint(Position position, bool useObjectAvoidance, bool keepRunning)
        {
            throw new NotImplementedException();
        }

        public void GotoNPC(int ID, bool useObjectAvoidance)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
        }
    }
}