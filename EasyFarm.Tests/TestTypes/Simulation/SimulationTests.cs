using EasyFarm.States;
using Xunit;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class SimulationTests
    {
        [Fact]
        public void FactMethodName()
        {
            Simulation simulation = new Simulation();
            simulation.Run();
            SimulationAPI api = new SimulationAPI(simulation);
            FiniteStateMachine finiteStateMachine = new FiniteStateMachine(api);
            finiteStateMachine.Run();
        }
    }
}
