using EasyFarm.Classes;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.Tests.TestTypes.Mocks;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class SummonTrustsStateTests
    {
        private readonly TestContext context = new TestContext();
        private readonly SummonTrustsState sut = new SummonTrustsState();

        [Fact]
        public void SummonsSingleTrust()
        {
            // Setup fixture
            context.SetPlayerHealthy();
            context.MockAPI.PartyMember[0].Name = "Trust";
            context.MockAPI.PartyMember[0].UnitPresent = true;
            context.Config.TrustPartySize = 1;
            context.Config.BattleLists["Trusts"].Actions.Add(new BattleAbility(){ IsEnabled = true, Name = "Trust"});
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }
    }
}
