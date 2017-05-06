using EasyFarm.Classes;
using EasyFarm.States;
using MemoryAPI;
using MemoryAPI.Navigation;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class TravelStateTests
    {
        private readonly IFixture fixture = new Fixture()
            .Customize(new AutoConfiguredMoqCustomization());

        [Fact]
        public void RunWithSingleWaypointRouteWillNotKeepRunning()
        {
            // Fixture setup
            var navigator = fixture.Freeze<Mock<INavigatorTools>>();
            var position = fixture.Freeze<Position>();
            SetupPlayerAndRouteWithPosition(position);
            var sut = CreateSut();

            // Exercise system
            sut.Run();

            // Verify outcome
            AssertDoesNotKeepRunningToPosition(navigator, position);
            // Teardown
        }

        private void SetupPlayerAndRouteWithPosition(Position position)
        {
            SetupPlayerWithPosition(position);
            SetupRouteWithPosition(position);
        }

        private void SetupRouteWithPosition(Position position)
        {
            var config = fixture.Freeze<Config>();
            config.Route.Waypoints.Clear();
            config.Route.Waypoints.Add(position);
        }

        private void SetupPlayerWithPosition(Position position)
        {
            fixture.Freeze<Mock<IPlayerTools>>()
                .Setup(x => x.Position)
                .Returns(position);
        }

        private TravelState CreateSut()
        {
            return fixture.Create<TravelState>();
        }

        private static void AssertDoesNotKeepRunningToPosition(Mock<INavigatorTools> navigator, Position position)
        {
            navigator.Verify(x => x.GotoWaypoint(position, false, false), Times.Once);
        }
    }
}