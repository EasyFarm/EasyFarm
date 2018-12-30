using System.Collections.Generic;
using EasyFarm.Infrastructure;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.Tests.TestTypes;
using EliteMMO.API;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class DeadStateTests
    {
        public class CheckTests
        {
            [Theory]
            [InlineData(Status.Dead1, true)]
            [InlineData(Status.Dead2, true)]
            [InlineData(Status.Standing, false)]
            [InlineData(Status.Fighting, false)]
            public void CheckTrueWhenPlayersDead(Status status, bool expected)
            {
                // Setup fixture
                var context = new TestContext();
                context.Player.Status = status;
                var sut = CreateSut();
                // Exercise system
                var result = sut.Check(context);
                // Verify outcome
                Assert.Equal(expected, result);
                // Teardown
            }

            private DeadState CreateSut()
            {
                return new DeadState();
            }
        }

        public class RunTests : AbstractTestBase
        {
            [Fact]
            public void RunResetIsCalledToStopPlayerFromRunning()
            {
                // Setup fixture
                var context = new TestContext();
                context.MockAPI.Navigator.IsRunning = true;
                var sut = CreateSut();
                // Exercise system
                sut.Run(context);
                // Verify outcome
                Assert.False(context.MockAPI.Navigator.IsRunning);
                // Teardown
            }

            [Fact]
            public void RunDoesNotHomePointIfConfigNotSet()
            {
                // Setup fixture
                var context = new TestContext();
                var sut = CreateSut();
                // Exercise system
                sut.Run(context);
                // Verify outcome
                Assert.Empty(context.MockAPI.Windower.KeyPresses);
                // Teardown
            }

            [Fact]
            public void RunSendsPauseCommand()
            {
                // Setup fixture
                var context = new TestContext();
                var sut = CreateSut();
                // Exercise system
                sut.Run(context);
                // Verify outcome
                Assert.Contains(typeof(Events.PauseEvent), Events);
                // Teardown
            }

            [Fact]
            public void RunAttemptsToHomepointAfterDeath()
            {
                // Setup fixture
                var context = new TestContext();
                context.Config.HomePointOnDeath = true;
                var sut = CreateSut();
                var expected = new List<Keys> {Keys.NUMPADENTER, Keys.LEFT, Keys.NUMPADENTER};
                // Exercise system
                sut.Run(context);
                // Verify outcome
                Assert.Equal(expected, context.MockAPI.Windower.KeyPresses);
                // Teardown
            }

            private DeadState CreateSut()
            {
                return new DeadState();
            }
        }
    }
}
