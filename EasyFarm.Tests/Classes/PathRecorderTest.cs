using System;
using EasyFarm.Classes;
using MemoryAPI;
using MemoryAPI.Navigation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class PathRecorderTest
    {
        [Fact]
        public void WilRecordNewWaypoint()
        {
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var memory = fixture.Create<IMemoryAPI>();

            var recorder = new PathRecorder(memory);
            var position = RecordNewPosition(recorder);
            Assert.NotNull(position);
        }

        private Position RecordNewPosition(PathRecorder recorder)
        {
            Position position = null;

            recorder.OnPositionAdded += pos => position = pos;
            recorder.Interval = 1;
            recorder.Start();

            DateTime start = DateTime.Now;
            while (position == null && DateTime.Now < start.AddSeconds(5)) { }
            recorder.Stop();
            return position;
        }
    }
}