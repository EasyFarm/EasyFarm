namespace EasyFarm.Tests.TestTypes.Simulation
{
    public interface ISimulation
    {
        T GetState<T>(string key);
        void Run();
    }
}