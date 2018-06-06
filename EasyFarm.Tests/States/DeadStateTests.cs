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
                var sut = CreateSut();
                // Exercise system
                var result = sut.Check();
                // Verify outcome
                Assert.Equal(expected, result);
                // Teardown
            }

            private DeadState CreateSut()
            {
                return new DeadState(new StateMemory(MockEliteAPI.AsMemoryApi())
                {
                    Config = MockConfig,
                    UnitFilters = new MockUnitFilters()
                });
            }
        }

        public class RunTests : AbstractTestBase
        {
            [Fact]
            public void RunResetIsCalledToStopPlayerFromRunning()
            {
                // Setup fixture
                MockEliteAPI.Navigator.IsRunning = true;
                var sut = CreateSut();
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
                var sut = CreateSut();
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
                var sut = CreateSut();
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
                var sut = CreateSut();
                MockConfig.HomePointOnDeath = true;
                var expected = new List<Keys> {Keys.NUMPADENTER, Keys.LEFT, Keys.NUMPADENTER};
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.Equal(expected, MockEliteAPI.Windower.KeyPresses);
                // Teardown
            }

            private DeadState CreateSut()
            {
                return new DeadState(new StateMemory(MockEliteAPI.AsMemoryApi())
                {
                    Config = MockConfig,
                    UnitFilters = new MockUnitFilters()
                });
            }
        }
    }
}
