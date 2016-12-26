using System;
using Xunit;
using Moq;
using MemoryAPI;
using MemoryAPI.Navigation;
using MemoryAPI.Tests;
using EasyFarm.Classes;
using EasyFarm.Parsing;

namespace EasyFarm.Tests.Classes
{
    public class ActionFilterTest
    {
        private readonly BattleAbility battleAbility = FindAbility();
        private readonly FakePlayer player = FindPlayer();
        private readonly FakeTimer timer = FindTimer();

        [Fact]
        public void ActionNotUsableWhenDisabled()
        {
            battleAbility.IsEnabled = false;
            VerifyActionNotUsable(null, battleAbility);
        }

        [Fact]
        public void ActionNotUsableWithBlankName()
        {
            battleAbility.Name = "";
            VerifyActionNotUsable(null, battleAbility);
        }

        [Fact]
        public void ActionNotUsableWithTooLittleMp()
        {
            battleAbility.Ability.MpCost = 1;
            player.MPCurrent = 0;
            VerifyActionNotUsable(player, battleAbility);
        }

        [Fact]
        public void ActionNotUsableWithTooLittleTp()
        {
            battleAbility.Ability.TpCost = 1;
            player.TPCurrent = 0;
            VerifyActionNotUsable(player, battleAbility);
        }

        [Fact]
        public void ActionNotUsableWhenMpNotInReserveRange()
        {
            battleAbility.MPReserveLow = 0;
            battleAbility.MPReserveHigh = 25;
            player.MPPCurrent = 100;
            VerifyActionNotUsable(player, battleAbility);
        }

        [Fact]
        public void ActionNotUsableWhenTpNotInReserveRange()
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
        public void ActionNotUsableWhenUsageLimitIsReached()
        {
            battleAbility.UsageLimit = 1;
            battleAbility.Usages = 1;
            VerifyActionNotUsable(player, battleAbility);
        }

        [Fact]
        public void SpellNotUsableWhenBlockedBySpellCastingBlockingEffect()
        {
            battleAbility.Ability.AbilityType = AbilityType.Magic;
            player.StatusEffects = new StatusEffect[] { StatusEffect.Silence };
            VerifyActionNotUsable(player, battleAbility);
        }

        [Fact]
        public void SpellNotUsableWithSpellOnRecast()
        {
            battleAbility.Ability.AbilityType = AbilityType.Magic;
            timer.SpellRecast = 1;
            VerifyActionNotUsable(player, battleAbility);
        }

        public void VerifyActionNotUsable(IPlayerTools player, BattleAbility action)
        {
            var memoryAPI = FindMemoryApi(player);
            memoryAPI.Player = player;
            memoryAPI.Timer = timer;
            var result = ActionFilters.BuffingFilter(memoryAPI, battleAbility);
            Assert.False(result);
        }

        public IMemoryAPI FindMemoryApi(IPlayerTools player)
        {
            var memoryAPI = new FakeMemoryAPI();
            memoryAPI.Player = player;
            memoryAPI.Timer = timer;
            return memoryAPI;
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
            player.HPPCurrent = 100;
            player.MPCurrent = 10000;
            player.MPPCurrent = 100;
            player.Name = "Mykezero";
            player.Status = Status.Standing;
            player.TPCurrent = 100;
            player.StatusEffects = new StatusEffect[]{ };
            return player;
        }

        public static FakeTimer FindTimer()
        {
            var timer = new FakeTimer();
            return timer;
        }
    }
}
