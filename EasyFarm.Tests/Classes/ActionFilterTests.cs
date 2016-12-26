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
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWithBlankName()
        {
            battleAbility.Name = "";
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWithTooLittleMp()
        {
            battleAbility.Ability.MpCost = 1;
            player.MPCurrent = 0;
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWithTooLittleTp()
        {
            battleAbility.Ability.TpCost = 1;
            player.TPCurrent = 0;
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenMpNotInReserveRange()
        {
            battleAbility.MPReserveLow = 0;
            battleAbility.MPReserveHigh = 25;
            player.MPPCurrent = 100;
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenTpNotInReserveRange()
        {
            battleAbility.TPReserveLow = 1000;
            battleAbility.TPReserveHigh = 1000;
            player.TPCurrent = 1;
            VerifyActionNotUsable();
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
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenUsageLimitIsReached()
        {
            battleAbility.UsageLimit = 1;
            battleAbility.Usages = 1;
            VerifyActionNotUsable();
        }

        [Theory]
        [InlineData(AbilityType.Magic, StatusEffect.Silence)]
        [InlineData(AbilityType.Jobability, StatusEffect.Amnesia)]
        public void ActionNotUsableWhenBlockedByStatusEffect(
            AbilityType abilityType,
            StatusEffect statusEffect)
        {
            battleAbility.Ability.AbilityType = abilityType;
            player.StatusEffects = new StatusEffect[] { statusEffect };
            VerifyActionNotUsable();
        }

        [Theory]
        [InlineData(AbilityType.Magic)]
        [InlineData(AbilityType.Jobability)]
        public void ActionNotUsableWhenOnRecast(AbilityType abilityType)
        {
            battleAbility.Ability.AbilityType = abilityType;
            timer.ActionRecast = 1;
            VerifyActionNotUsable();
        }

        [Theory]
        [InlineData(75)]
        [InlineData(0)]
        public void ActionNotUsableWhenPlayersHealthNotInRange(int hppCurrent)
        {
            battleAbility.PlayerLowerHealth = 25;
            battleAbility.PlayerUpperHealth = 50;
            player.HPPCurrent = hppCurrent;
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenStatusEffectPresentButActionSetToTriggerOnEffectMissing()
        {
            battleAbility.StatusEffect = "Magic Acc Down";
            battleAbility.TriggerOnEffectPresent = false;
            player.StatusEffects = new StatusEffect[] { StatusEffect.Magic_Acc_Down };
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenStatusEffectMissingButActionSetToTriggerOnEffectPresent()
        {
            battleAbility.StatusEffect = "Dia";
            battleAbility.TriggerOnEffectPresent = true;
            player.StatusEffects = new StatusEffect[] { };
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenRecastPeriodHasNotPassed()
        {
            battleAbility.Recast = 1;
            battleAbility.LastCast = DateTime.Now.AddMinutes(1);
            VerifyActionNotUsable();
        }

        public void VerifyActionNotUsable()
        {
            var memoryAPI = FindMemoryApi();
            var result = ActionFilters.BuffingFilter(memoryAPI, battleAbility);
            Assert.False(result);
        }

        public IMemoryAPI FindMemoryApi()
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
            player.TPCurrent = 1000;
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
