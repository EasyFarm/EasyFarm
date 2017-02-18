using MemoryAPI.Navigation;

namespace MemoryAPI
{
    public interface INavigatorTools
    {
        double DistanceTolerance { get; set; }
                
        double DistanceTo(Position position);
        bool FaceHeading(Position position);        
        void Goto(Position position, bool useObjectAvoidance, bool keepRunning);
        void GotoNPC(int ID, bool useObjectAvoidance, bool keepRunning);
        void Reset();        
    }
}