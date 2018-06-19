using System.Linq;
using EasyFarm.Parsing;
using Xunit;

namespace EasyFarm.Tests.Parsing
{
    public class ResourcesTests
    {
        [Fact]
        public void Ranged_ShouldReturnRangedAttack()
        {
            // Fixture setup
            var sut = new AbilityService(null);
            // Excercise system
            var result = sut.GetAbilitiesWithName("Ranged").FirstOrDefault();
            // Verify outcome
            Assert.NotNull(result);
            Assert.Equal(AbilityType.Range, result.AbilityType);
            Assert.Equal(TargetType.Enemy, result.TargetType);
            Assert.Equal("Ranged", result.English);
            Assert.Equal("/range <t>", result.Command);
            // Teardown	
        }

        [Fact]
        public void Ranged_IsCaseInsensitive()
        {
            // Fixture setup
            var sut = new AbilityService(null);
            // Excercise system
            var result = sut.GetAbilitiesWithName("ranged").FirstOrDefault();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown	
        }
    }
}
