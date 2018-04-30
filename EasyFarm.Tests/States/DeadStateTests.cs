using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using EasyFarm.Infrastructure;
using EasyFarm.States;
using EasyFarm.Tests.TestTypes;
using EasyFarm.Tests.TestTypes.Mocks;
using EasyFarm.UserSettings;
using EliteMMO.API;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class DeadStateTests
    {
        public class CheckTests : AbstractTestBase
        {
            [Theory]
            [InlineData(Status.Dead1, true)]
            [InlineData(Status.Dead2, true)]
            [InlineData(Status.Standing, false)]
            [InlineData(Status.Fighting, false)]
            public void CheckTrueWhenPlayersDead(Status status, bool expected)
            {
                // Setup fixture
                MockEliteAPI.Player.Status = status;
                var sut = CreateSut(MockEliteAPI);
                // Exercise system
                var result = sut.Check();
                // Verify outcome
                Assert.Equal(expected, result);
                // Teardown
            }
        }

        public class RunTests : AbstractTestBase
        {
            [Fact]
            public void RunResetIsCalledToStopPlayerFromRunning()
            {
                // Setup fixture
                MockEliteAPI.Navigator.IsRunning = true;
                var sut = CreateSut(MockEliteAPI);
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.False(MockEliteAPI.Navigator.IsRunning);
                // Teardown
            }

            [Fact]
            public void RunDoesNotHomePointIfConfigNotSet()
            {
                // Setup fixture
                var sut = CreateSut(MockEliteAPI);
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.Empty(MockEliteAPI.Windower.KeyPresses);
                // Teardown
            }

            [Fact]
            public void RunSendsPauseCommand()
            {
                // Setup fixture
                var sut = CreateSut(MockEliteAPI);
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.Contains(typeof(Events.PauseEvent), Events);
                // Teardown
            }

            [Fact]
            public void RunAttemptsToHomepointAfterDeath()
            {
                // Setup fixture
                var sut = CreateSut(MockEliteAPI);
                Config.HomePointOnDeath = true;
                var expected = new List<Keys> {Keys.NUMPADENTER, Keys.LEFT, Keys.NUMPADENTER};
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.Equal(expected, MockEliteAPI.Windower.KeyPresses);
                // Teardown
            }
        }

        private static DeadState CreateSut(MockEliteAPI mockEliteAPI)
        {
            return new DeadState(new StateMemory(new MockEliteAPIAdapter(mockEliteAPI)));
        }
    }
}
