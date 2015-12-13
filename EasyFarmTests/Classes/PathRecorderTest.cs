using EasyFarm.Classes;
using EasyFarm.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace EasyFarm.Tests.UnitTests.Classes
{
    public class PathRecorderTest
    {
        protected MockMemorySource memory;
        protected PathRecorder recorder;

        [TestInitialize]
        public void Initialize()
        {
            memory = new MockMemorySource();
            recorder = new PathRecorder(memory);
            recorder.Interval = 5;
        }

        [TestClass]
        public class Start : PathRecorderTest
        {
            /// <summary>
            /// Test whether the position recorder records positions. 
            /// </summary>
            [TestMethod]
            public void Adds_Waypoint_To_Path()
            {                
                var position = RecordNewPosition();
                Assert.IsNotNull(position);
            }

            /// <summary>
            /// Starts recording a new waypoint. 
            /// </summary>
            private Position RecordNewPosition()
            {
                var task = new TaskCompletionSource<Position>();
                recorder.OnPositionAdded += (pos) => task.SetResult(pos);
                recorder.Interval = 1;
                recorder.Start();
                var position = task.Task.GetAwaiter().GetResult();
                recorder.Stop();
                return position;
            }
        }        
    }
}