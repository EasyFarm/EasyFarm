using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.Tests.TestTypes.Mocks;
using MemoryAPI;
using MemoryAPI.Navigation;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class ValidMobTests
    {
        [Fact]
        public void WillTargetValidMob()
        {
            // Fixture setup
            var context = new TestContext();
            var sut = new SetTargetState();
            context.Units[0].IsValid = true;
            // Exercise system
            sut.Check(context);
            // Verify outcome
            Assert.NotNull(context.Target);
            // Teardown	
        }
    }
}