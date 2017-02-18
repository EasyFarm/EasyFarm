using MemoryAPI.Navigation;

namespace MemoryAPI
{
    public interface INavigatorTools
    {
        double DistanceTolerance { get; set; }                
        void FaceHeading(Position position);        
        void GotoWaypoint(Position position, bool useObjectAvoidance);
        void GotoNPC(int ID, bool useObjectAvoidance);
        void Reset();        
    }
}