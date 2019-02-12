using EasyFarm.Classes;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class EventsTests
    {
        private readonly EventMessenger _sut = new EventMessenger();
        private readonly TestEvent _event = new TestEvent();

        [Fact]
        public void FireASingleEvent()
        {
            _sut.Bind<TestEvent>(x => x.EventsFired++);
            _sut.Fire(_event);
            Assert.Equal(1, _event.EventsFired);
        }

        [Fact]
        public void FireMultipleEventsBoundToSameType()
        {
            _sut.Bind<TestEvent>(x => x.EventsFired++);
            _sut.Bind<TestEvent>(x => x.EventsFired++);
            _sut.Fire(_event);
            Assert.Equal(2, _event.EventsFired);
        }

        [Fact]
        public void FireMultipleEventsBoundToDifferentTypes()
        {
            // Setup fixture
            _sut.Bind<TestEvent>((e) => e.EventsFired++);
            _sut.Bind<OtherTestEvent>((e) => { });
            // Exercise system
            _sut.Fire(new TestEvent());
            // Verify outcome
            // Teardown
        }
    }
}
