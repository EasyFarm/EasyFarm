namespace EasyFarm.States
{
    public interface IState
    {
        bool Enabled { get; set; }
        int Priority { get; set; }

        bool CheckComponent();        
        void EnterComponent();
        void ExitComponent();        
        void RunComponent();

        int CompareTo(object other);        
    }
}
