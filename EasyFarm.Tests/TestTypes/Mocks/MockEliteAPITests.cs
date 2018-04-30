using EasyFarm.UserSettings;
using EliteMMO.API;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockEliteAPITests
    {
        public class MockWindowerToolsTests : AbstractTestBase
        {
            [Fact]
            public void SendKeyPress_RecordsKeyPresses()
            {
                // Setup fixture
                var sut = CreateSut();
                // Exercise system
                Keys expected = Keys.RETURN;
                sut.Windower.SendKeyPress(expected);
                // Verify outcome
                Assert.Contains(expected, MockEliteAPI.Windower.KeyPresses);
                // Teardown
            }

            [Fact]
            public void SendKeyPress_RecordsLastKeyPress()
            {
                // Setup fixture
                var sut = CreateSut();
                // Exercise system
                Keys expected = Keys.RETURN;
                sut.Windower.SendKeyPress(expected);
                // Verify outcome
                Assert.Equal(expected, sut.Windower.LastKeyPress);
                // Teardown
            }

            [Theory]
            [InlineData(Constants.AttackTarget, Status.Fighting)]
            [InlineData(Constants.RestingOn, Status.Healing)]
            [InlineData(Constants.RestingOff, Status.Standing)]
            public void SendString_CorrectlyChangesPlayerStatus(string command, Status expected)
            {
                // Setup fixture
                var sut = CreateSut();
                // Exercise system
                sut.Windower.SendString(command);
                // Verify outcome
                Assert.Equal(expected, sut.Player.Status);
                // Teardown
            }

            [Fact]
            public void SendString_RecordsLastCommand()
            {
                // Setup fixture
                var sut = CreateSut();
                // Exercise system
                string expected = "TEST COMMAND";
                sut.Windower.SendString(expected);
                // Verify outcome
                Assert.Equal(expected, sut.Windower.LastCommand);
                // Teardown
            }

            private MockEliteAPI CreateSut()
            {
                return MockEliteAPI;
            }
        }
    }
}
