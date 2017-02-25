using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.Classes
{
    public class FakeNavigator : INavigatorTools
    {
        public double DistanceTolerance { get; set; }

        public void FaceHeading(Position position)
        {            
        }

        public void GotoWaypoint(Position position, bool useObjectAvoidance)
        {            
        }

        public void GotoNPC(int ID, bool useObjectAvoidance)
        {            
        }

        public void Reset()
        {            
        }
    }
}