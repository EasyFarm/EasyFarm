using System;
using Xunit;
using Moq;
using MemoryAPI;
using MemoryAPI.Navigation;
using MemoryAPI.Tests;
using EasyFarm.Classes;

namespace EasyFarm.Tests.Classes
{
    public class ActionFilterTest
    {
        private readonly BattleAbility battleAbility = FindAbility();
        private readonly FakePlayer player = FindPlayer();

        [Fact]
        public void AbilityNotUsableWhenDisabled()
        {
            battleAbility.IsEnabled = false;
            VerifyActionNotUsable(null, battleAbility);
        }

        [Fact]
        public void AbilityNotUsableWithBlankName()
        {
            battleAbility.Name = "";
            VerifyActionNotUsable(null, battleAbility);
        }

        [Fact]
        public void AbilityNotUsableWithTooLittleMp()
        {
            battleAbility.Ability.MpCost = 1;
            player.MPCurrent = 0;
            VerifyActionNotUsable(player, battleAbility);
        }

        [Fact]
        public void AbilityNotUsableWithTooLittleTp()
        {
            battleAbility.Ability.TpCost = 1;
            player.TPCurrent = 0;
            VerifyActionNotUsable(player, battleAbility);
        }

        [Fact]
        public void AbilityNotUsableWhenMpNotInReserveRange()
        {
            battleAbility.MPReserveLow = 0;
            battleAbility.MPReserveHigh = 25;
            player.MPPCurrent = 100;
            VerifyActionNotUsable(player, battleAbility);
        }

        [Fact]
        public void AbilityNotUsableWhenTpNotInReserveRange()
        {
            battleAbility.TPReserveLow = 1000;
            battleAbility.TPReserveHigh = 1000;
            player.TPCurrent = 1;
            VerifyActionNotUsable(player, battleAbility);
        }

        /// <summary>
        /// Fixed bug where MPReserve was being used to calculate whether a
        /// weaponskill could be used: should have been using TPReserve
        /// </summary>
        [Fact]
        public void MpReserveWasUseInsteadOfTpReserveWhenCheckingTpReserveValue()
        {
            battleAbility.MPReserveLow = 1;
            battleAbility.MPReserveHigh = 1;
            battleAbility.TPReserveLow = 1000;
            battleAbility.TPReserveHigh = 1000;
            player.MPPCurrent = 1;
            player.TPCurrent = 1;
            VerifyActionNotUsable(player, battleAbility);
        }

        [Fact]
        public void AbilityNotUsableWhenUsageLimitIsReached()
        {
            battleAbility.UsageLimit = 1;
            battleAbility.Usages = 1;
            VerifyActionNotUsable(player, battleAbility);
        }

        public void VerifyActionNotUsable(IPlayerTools player, BattleAbility action)
        {
            var memoryAPI = FindMemoryApi(player);
            var result = ActionFilters.BuffingFilter(memoryAPI, battleAbility);
            Assert.False(result);
        }

        public IMemoryAPI FindMemoryApi(IPlayerTools player)
        {
            var memoryAPI = new Mock<IMemoryAPI>();
            memoryAPI.Setup(x => x.Player).Returns(player);
            return memoryAPI.Object;
        }

        public static BattleAbility FindAbility()
        {
            var battleAbility = new BattleAbility();
            battleAbility.IsEnabled = true;
            battleAbility.Name = "valid";
            return battleAbility;
        }

        public static FakePlayer FindPlayer()
        {
            var player = new FakePlayer();
            player.HPPCurrent=100;
            player.MPCurrent=10000;
            player.MPPCurrent=100;
            player.Name="Mykezero";
            player.Status=Status.Standing;
            player.TPCurrent=100;
            return player;
        }
    }
}
