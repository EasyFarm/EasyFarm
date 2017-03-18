using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes
{
    public class FakeNavigator : INavigatorTools
    {
        public double DistanceTolerance { get; set; }
        public bool ResetWasCalled { get; set; }

        public void FaceHeading(Position position)
        {            
        }

        public void GotoWaypoint(Position position, bool useObjectAvoidance, bool keepRunning)
        {            
        }

        public void GotoNPC(int ID, bool useObjectAvoidance)
        {            
        }

        public void Reset()
        {
            ResetWasCalled = true;
        }
    }
}