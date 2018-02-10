// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using EasyFarm.Classes;
using EasyFarm.States;
using EasyFarm.UserSettings;
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
            navigator.Verify(x => x.GotoWaypoint(position, true, false), Times.Once);
        }
    }
}