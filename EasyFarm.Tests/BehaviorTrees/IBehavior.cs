namespace EasyFarm.Tests.BehaviorTrees
{
    public interface IBehavior
    {
        bool Check();
        void Run();
    }
}