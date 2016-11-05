namespace EasyFarm.States
{
    public interface IState
    {
        bool Enabled { get; set; }
        int Priority { get; set; }

        bool Check();        
        void Enter();
        void Exit();        
        void Run();

        int CompareTo(object other);        
    }
}
