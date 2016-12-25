using EasyFarm.Classes;
using MemoryAPI.Navigation;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class PathRecorderTest
    {
        [Fact]
        public void AddNewPositionWithPositionWillRecordIt()
        {
            Position expected = new Position {H = 1, X = 1, Y = 1, Z = 1};
            Position result = null;

            var recorder = new PathRecorder(null);

            recorder.OnPositionAdded += actual => result = actual;
            recorder.AddNewPosition(expected);

            Assert.Equal(expected, result);
        }
    }
}
