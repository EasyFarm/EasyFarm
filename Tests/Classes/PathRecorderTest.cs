using System;
using EasyFarm.Classes;
using MemoryAPI;
using MemoryAPI.Navigation;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class PathRecorderTest
    {
        /// <summary>
        /// Test whether the position recorder records positions. 
        /// </summary>
        [Theory, AutoMoqData]
        public void Adds_Waypoint_To_Path([Frozen]Mock<IMemoryAPI> memoryApi)
        {
            var recorder = new PathRecorder(memoryApi.Object);
            var position = RecordNewPosition(recorder);
            Assert.NotNull(position);
        }

        /// <summary>
        /// Starts recording a new waypoint. 
        /// </summary>
        private Position RecordNewPosition(PathRecorder recorder)
        {
            Position position = null;

            recorder.OnPositionAdded += (pos) =>
            {
                position = pos;
            };

            recorder.Interval = 1;

            recorder.Start();

            DateTime start = DateTime.Now;
            while (position == null && DateTime.Now < start.AddSeconds(5)) { }
            recorder.Stop();
            return position;
        }
    }
}