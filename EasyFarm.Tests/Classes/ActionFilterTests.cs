using System;
using Xunit;
using MemoryAPI;
using MemoryAPI.Tests;
using EasyFarm.Classes;
using EasyFarm.Parsing;

namespace EasyFarm.Tests.Classes
{
    public class ActionFilterTest : AbstractTestFixture
    {
        private readonly BattleAbility _battleAbility = FindAbility();
        private readonly FakePlayer _player = FindPlayer();
        private readonly FakeTimer _timer = FindTimer();

        [Fact]
        public void ActionNotUsableWhenDisabled()
        {
            _battleAbility.IsEnabled = false;
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWithBlankName()
        {
            _battleAbility.Name = "";
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWithTooLittleMp()
        {
            _battleAbility.Ability.MpCost = 1;
            _player.MPCurrent = 0;
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWithTooLittleTp()
        {
            _battleAbility.Ability.TpCost = 1;
            _player.TPCurrent = 0;
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenMpNotInReserveRange()
        {
            _battleAbility.MPReserveLow = 0;
            _battleAbility.MPReserveHigh = 25;
            _player.MPPCurrent = 100;
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenTpNotInReserveRange()
        {
            _battleAbility.TPReserveLow = 1000;
            _battleAbility.TPReserveHigh = 1000;
            _player.TPCurrent = 1;
            VerifyActionNotUsable();
        }

        /// <summary>
        /// Fixed bug where MPReserve was being used to calculate whether a
        /// weaponskill could be used: should have been using TPReserve
        /// </summary>
        [Fact]
        public void MpReserveWasUseInsteadOfTpReserveWhenCheckingTpReserveValue()
        {
            _battleAbility.MPReserveLow = 1;
            _battleAbility.MPReserveHigh = 1;
            _battleAbility.TPReserveLow = 1000;
            _battleAbility.TPReserveHigh = 1000;
            _player.MPPCurrent = 1;
            _player.TPCurrent = 1;
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenUsageLimitIsReached()
        {
            _battleAbility.UsageLimit = 1;
            _battleAbility.Usages = 1;
            VerifyActionNotUsable();
        }

        [Theory]
        [InlineData(AbilityType.Magic, StatusEffect.Silence)]
        [InlineData(AbilityType.Jobability, StatusEffect.Amnesia)]
        public void ActionNotUsableWhenBlockedByStatusEffect(
            AbilityType abilityType,
            StatusEffect statusEffect)
        {
            _battleAbility.Ability.AbilityType = abilityType;
            _player.StatusEffects = new StatusEffect[] { statusEffect };
            VerifyActionNotUsable();
        }

        [Theory]
        [InlineData(AbilityType.Magic)]
        [InlineData(AbilityType.Jobability)]
        public void ActionNotUsableWhenOnRecast(AbilityType abilityType)
        {
            _battleAbility.Ability.AbilityType = abilityType;
            _timer.ActionRecast = 1;
            VerifyActionNotUsable();
        }

        [Theory]
        [InlineData(75)]
        [InlineData(0)]
        public void ActionNotUsableWhenPlayersHealthNotInRange(int hppCurrent)
        {
            _battleAbility.PlayerLowerHealth = 25;
            _battleAbility.PlayerUpperHealth = 50;
            _player.HPPCurrent = hppCurrent;
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenStatusEffectPresentButActionSetToTriggerOnEffectMissing()
        {
            _battleAbility.StatusEffect = "Magic Acc Down";
            _battleAbility.TriggerOnEffectPresent = false;
            _player.StatusEffects = new StatusEffect[] { StatusEffect.Magic_Acc_Down };
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenStatusEffectMissingButActionSetToTriggerOnEffectPresent()
        {
            _battleAbility.StatusEffect = "Dia";
            _battleAbility.TriggerOnEffectPresent = true;
            _player.StatusEffects = new StatusEffect[] { };
            VerifyActionNotUsable();
        }

        [Fact]
        public void ActionNotUsableWhenRecastPeriodHasNotPassed()
        {
            _battleAbility.Recast = 1;
            _battleAbility.LastCast = DateTime.Now.AddMinutes(1);
            VerifyActionNotUsable();
        }

        public void VerifyActionNotUsable()
        {
            var memoryAPI = FindMemoryApi();
            var result = ActionFilters.BuffingFilter(memoryAPI, _battleAbility);
            Assert.False(result);
        }

        public IMemoryAPI FindMemoryApi()
        {
            var memoryAPI = new FakeMemoryAPI();
            memoryAPI.Player = _player;
            memoryAPI.Timer = _timer;
            return memoryAPI;
        }
    }
}
