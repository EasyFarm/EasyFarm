using MemoryAPI;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.Tests.TestTypes;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class AbilityUtilsTests
    {
        private readonly Ability ability = new Ability();

        [Theory]
        [InlineData(AbilityType.Magic)]
        [InlineData(AbilityType.Jobability)]
        [InlineData(AbilityType.Weaponskill)]
        public void IsRecastableWhenNotOnRecast(AbilityType abilityType)
        {
            ability.AbilityType = abilityType;
            var memoryApi = new FakeMemoryAPI();
            memoryApi.Timer = new FakeTimer();
            var result = AbilityUtils.IsRecastable(memoryApi, ability);
            Assert.True(result);
        }

        [Theory]
        [InlineData(AbilityType.Magic)]
        [InlineData(AbilityType.Jobability)]
        public void NotRecastableWhenOnRecast(AbilityType abilityType)
        {
            ability.AbilityType = abilityType;
            var memoryApi = new FakeMemoryAPI();
            memoryApi.Timer = new FakeTimer() { ActionRecast = 1 };
            var result = AbilityUtils.IsRecastable(memoryApi, ability);
            Assert.False(result);
        }
    }
}
