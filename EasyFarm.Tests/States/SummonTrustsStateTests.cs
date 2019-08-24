using EasyFarm.Classes;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class SummonTrustsStateTests
    {
        private readonly TestContext context = new TestContext();
        private readonly SummonTrustsState sut = new SummonTrustsState();

        [Fact]
        public void HasSpaceToSummonTrust()
        {
            // Setup fixture
            context.SetPlayerHealthy();
            context.Config.TrustPartySize = 1;
            context.Config.BattleLists["Trusts"].Actions.Add(new BattleAbility(){ IsEnabled = true, Name = "Trust"});
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void DismissesTrustWithLowMp()
        {
            // Setup fixture
            context.SetPlayerHealthy();
            context.AddTrustToParty("Trust");
            
            context.Config.TrustPartySize = 1;
            context.Config.BattleLists["Trusts"].Actions.Add(new BattleAbility()
            {
                IsEnabled = true, 
                Name = "Trust",
                ResummonOnLowMP = true,
                ResummonMPLow = 0,
                ResummonMPHigh = 50
            });

            // Exercise system
            sut.Run(context);
            // Verify outcome
            Assert.Equal("/refa Trust", context.MockAPI.Windower.LastCommand);
            // Teardown
        }
    }
}
