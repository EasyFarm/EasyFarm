using EasyFarm.Parsing;
using Xunit;

namespace EasyFarm.Tests.Parsing
{
    public class AbilityTests
    {
        [Fact]
        public void JapanseCommandsDoNotHaveSpaces()
        {
            var sut = new Ability()
            {
                Prefix = "/magic",
                English = "ケアルIII",
                TargetType = TargetType.Self
            };

            Assert.Equal("/magic ケアルIII <me>", sut.Command);
        }

        [Fact]
        public void RangeCommandReturnsCorrectResults()
        {
            var sut = new Ability()
            {
                Prefix = "/range",
                AbilityType = AbilityType.Range,
                TargetType = TargetType.Enemy
            };

            Assert.Equal("/range <t>", sut.Command);
        }

        [Fact]
        public void JobAbilityCommandReturnsCorrectResults()
        {
            var sut = new Ability()
            {
                Prefix = "/jobability",
                English = "Boost",
                TargetType = TargetType.Self
            };

            Assert.Equal("/jobability Boost <me>", sut.Command);
        }

        [Fact]
        public void CommandWithSpaceIsSurroundByQuotes()
        {
            var sut = new Ability()
            {
                Prefix = "/magic",
                English = "Cure III",
                TargetType = TargetType.Self
            };

            Assert.Equal("/magic \"Cure III\" <me>", sut.Command);
        }
    }
}
