using EasyFarm.States;
using EasyFarm.Tests.TestTypes;
using EasyFarm.Tests.TestTypes.Mocks;
using EasyFarm.UserSettings;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class RestStateTests
    {
        public class CheckTests : AbstractTestBase
        {
            [Theory]
            [InlineData(50, 50, true)]
            [InlineData(50, 51, true)]
            [InlineData(50, 49, false)]
            public void ShouldHealWithLowMP(int currentMPP, int lowMPP, bool expected)
            {
                // Setup fixture
                var sut = CreateSut();
                MockEliteAPI.Player.MPPCurrent = currentMPP;
                MockConfig.IsMagicEnabled = true;
                MockConfig.LowMagic = lowMPP;
                // Exercise system
                var result = sut.Check();
                // Verify outcome
                Assert.Equal(expected, result);
                // Teardown
            }

            [Theory]
            [InlineData(50, 50, true)]
            [InlineData(50, 51, true)]
            [InlineData(50, 49, false)]
            public void ShouldHealWithLowHP(int currentHPP, int lowHPP, bool expected)
            {
                // Setup fixture
                RestState sut = CreateSut();
                MockEliteAPI.Player.HPPCurrent = currentHPP;
                MockConfig.IsHealthEnabled = true;
                MockConfig.LowHealth = lowHPP;
                // Exercise system
                var result = sut.Check();
                // Verify outcome
                Assert.Equal(expected, result);
                // Teardown
            }

            private RestState CreateSut()
            {
                return new RestState(CreateStateMemory());
            }
        }

        public class RunTests : AbstractTestBase
        {
            [Fact]
            public void PlayerSwitchesFromStandingToHealing()
            {
                // Setup fixture
                var sut = CreateSut();
                MockEliteAPI.Player.Status = Status.Standing;
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.Equal(Status.Healing, MockEliteAPI.Player.Status);
                // Teardown
            }

            private RestState CreateSut()
            {
                return new RestState(CreateStateMemory());
            }
        }

        public class ExitTests : AbstractTestBase
        {
            [Fact]
            public void PlayerSwitchesFromHealingToStanding()
            {
                // Setup fixture
                var sut = CreateSut();
                MockEliteAPI.Player.Status = Status.Healing;
                // Exercise system
                sut.Exit();
                // Verify outcome
                Assert.Equal(Status.Standing, MockEliteAPI.Player.Status);
                // Teardown
            }

            private RestState CreateSut()
            {
                return new RestState(CreateStateMemory());
            }
        }
    }
}
