using EasyFarm.Classes;
using EasyFarm.Parsing;
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
        public void SummonsTrust()
        {
            // Setup fixture
            context.SetPlayerHealthy();
            context.Config.TrustPartySize = 1;
            context.Config.BattleLists["Trusts"].Actions.Add(new BattleAbility()
            {
                IsEnabled = true, 
                AbilityType = AbilityType.Trust,
                Name = "Trust",
                Command = "Command"
            });
            // Exercise system
            sut.Run(context);
            // Verify outcome
            Assert.Equal("Command", context.MockAPI.Windower.LastCommand);
            // Teardown
        }

        [Theory()]
        [InlineData(50)]
        [InlineData(25)]
        [InlineData(0)]
        public void DismissesTrustWithLowMpp(int trustMpp)
        {
            // Setup fixture
            context.SetPlayerHealthy();
            context.AddTrustToParty();
            context.MockAPI.PartyMember[1].MPPCurrent = trustMpp;
            
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

        [Theory()]
        [InlineData(50)]
        [InlineData(25)]
        [InlineData(0)]
        public void DismissesTrustWithLowHpp(int trustHpp)
        {
            // Setup fixture
            context.SetPlayerHealthy();
            context.AddTrustToParty();
            context.MockAPI.PartyMember[1].HPPCurrent = trustHpp;
            
            context.Config.BattleLists["Trusts"].Actions.Add(new BattleAbility()
            {
                IsEnabled = true, 
                Name = "Trust",
                ResummonOnLowHP = true,
                ResummonHPLow = 0,
                ResummonHPHigh = 50
            });

            // Exercise system
            sut.Run(context);
            // Verify outcome
            Assert.Equal("/refa Trust", context.MockAPI.Windower.LastCommand);
            // Teardown
        }
    }
}
