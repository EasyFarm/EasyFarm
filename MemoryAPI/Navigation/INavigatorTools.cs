using MemoryAPI.Navigation;

namespace MemoryAPI
{
    public interface INavigatorTools
    {
        double DistanceTolerance { get; set; }
                
        double DistanceTo(Position position);
        bool FaceHeading(Position position);        
        void Goto(Position position);
        void GotoNPC(int ID);
        void Reset();        
    }
}