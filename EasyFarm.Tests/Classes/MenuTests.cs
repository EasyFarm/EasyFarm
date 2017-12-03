using System;
using System.Windows.Input;
using EasyFarm.Classes;
using EliteMMO.API;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class MenuTests
    {
        [Fact]
        public void TargetCommandWithTargetSetsIt()
        {
            // Setup fixture
            var sut = new MenuCommand(@"Target ""Warblade Beak""");
            // Exercise system
            // Verify outcome
            Assert.Equal("Target", sut.CommandType);
            Assert.Equal("Warblade Beak", sut.Target);
            // Teardown
        }

        [Fact]
        public void WaitCommandWithWaitTimeSetsIt()
        {
            // Setup fixture
            var sut = new MenuCommand("Wait 2.0");
            // Exercise system
            // Verify outcome
            Assert.Equal("Wait", sut.CommandType);
            Assert.Equal(TimeSpan.FromSeconds(2.0), sut.Delay);
            // Teardown
        }

        [Theory]
        [InlineData("Up", Keys.UP)]
        [InlineData("Down", Keys.DOWN)]
        [InlineData("Left", Keys.LEFT)]
        [InlineData("Right", Keys.RIGHT)]
        [InlineData("Enter", Keys.NUMPADENTER)]
        [InlineData("Escape", Keys.ESCAPE)]
        [InlineData("XXX", 0)]
        public void KeyCommandWithKeySetsIt(string key, Keys expected)
        {
            // Setup fixture
            var sut = new MenuCommand($"Key {key}");
            // Exercise system
            // Verify outcome
            Assert.Equal("Key", sut.CommandType);
            Assert.Equal(expected, sut.Key);
            // Teardown
        }

        [Fact]
        public void ExpectCommandWithExpectedMobSetsIt()
        {
            // Setup fixture
            var sut = new MenuCommand(@"Expect ""Warblade Beak""");
            // Exercise system
            // Verify outcome
            Assert.Equal("Expect", sut.CommandType);
            Assert.Equal("Warblade Beak", sut.Expect);
            // Teardown
        }
    }
}
