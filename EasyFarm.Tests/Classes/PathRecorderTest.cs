using System;
using EasyFarm.Classes;
using MemoryAPI;
using MemoryAPI.Navigation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace EasyFarm.Tests.Classes
{
    [TestClass]
    public class PathRecorderTest
    {
        /// <summary>
        /// Test whether the position recorder records positions. 
        /// </summary>
        [TestMethod]
        public void Adds_Waypoint_To_Path()
        {
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var memory = fixture.Create<IMemoryAPI>();

            var recorder = new PathRecorder(memory);
            var position = RecordNewPosition(recorder);
            Assert.IsNotNull(position);
        }

        /// <summary>
        /// Starts recording a new waypoint. 
        /// </summary>
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