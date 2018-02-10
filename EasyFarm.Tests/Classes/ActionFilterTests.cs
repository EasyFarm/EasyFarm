// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using MemoryAPI;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.Tests.TestTypes;

namespace EasyFarm.Tests.Classes
{

    public class ActionFilterTests : AbstractTestFixture
    {
        private readonly BattleAbility _battleAbility = FindAbility();
        private readonly FakePlayer _player = FindPlayer();
        private readonly FakeTimer _timer = FindTimer();

        public class ActionNotUsable : ActionFilterTests
        {
            [Fact]
            public void WhenDisabled()
            {
                _battleAbility.IsEnabled = false;
                VerifyActionNotUsable();
            }

            [Fact]
            public void WthEmptyName()
            {
                _battleAbility.Name = "";
                VerifyActionNotUsable();
            }

            [Fact]
            public void WithTooLittleMp()
            {
                _battleAbility.Ability.MpCost = 1;
                _player.MPCurrent = 0;
                VerifyActionNotUsable();
            }

            [Fact]
            public void WithTooLittleTp()
            {
                _battleAbility.Ability.TpCost = 1;
                _player.TPCurrent = 0;
                VerifyActionNotUsable();
            }

            [Fact]
            public void WithMpNotInRange()
            {
                _battleAbility.MPReserveLow = 0;
                _battleAbility.MPReserveHigh = 25;
                _player.MPPCurrent = 100;
                VerifyActionNotUsable();
            }

            [Fact]
            public void WithTpNotInRange()
            {
                _battleAbility.TPReserveLow = 1000;
                _battleAbility.TPReserveHigh = 1000;
                _player.TPCurrent = 1;
                VerifyActionNotUsable();
            }

            [Fact]
            public void WhenTpRangeNotSet()
            {
                // Fixture setup
                _battleAbility.TPReserveLow = 0;
                _battleAbility.TPReserveHigh = 0;
                _player.TPCurrent = -1;
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
                _battleAbility.Usages = 2;
                VerifyActionNotUsable();
            }

            [Theory]
            [InlineData(AbilityType.Magic, StatusEffect.Silence)]
            [InlineData(AbilityType.Jobability, StatusEffect.Amnesia)]
            public void WithBlockingStatusEffect(
                AbilityType abilityType,
                StatusEffect statusEffect)
            {
                _battleAbility.Ability.AbilityType = abilityType;
                _player.StatusEffects = new StatusEffect[] {statusEffect};
                VerifyActionNotUsable();
            }

            [Theory]
            [InlineData(AbilityType.Magic)]
            [InlineData(AbilityType.Jobability)]
            public void WhenOnRecast(AbilityType abilityType)
            {
                _battleAbility.Ability.AbilityType = abilityType;
                _timer.ActionRecast = 1;
                VerifyActionNotUsable();
            }

            [Theory]
            [InlineData(75)]
            [InlineData(0)]
            public void WithPlayerHealthNotInRange(int hppCurrent)
            {
                _battleAbility.PlayerLowerHealth = 25;
                _battleAbility.PlayerUpperHealth = 50;
                _player.HPPCurrent = hppCurrent;
                VerifyActionNotUsable();
            }

            [Fact]
            public void WithStatusEffectAndWithTriggerOnMissingEffect()
            {
                _battleAbility.StatusEffect = "Magic Acc Down";
                _battleAbility.TriggerOnEffectPresent = false;
                _player.StatusEffects = new[] {StatusEffect.Magic_Acc_Down};
                VerifyActionNotUsable();
            }

            [Fact]
            public void WithoutStatusEffectAndWithTriggerOnEffectPresent()
            {
                _battleAbility.StatusEffect = "Dia";
                _battleAbility.TriggerOnEffectPresent = true;
                _player.StatusEffects = new StatusEffect[] {};
                VerifyActionNotUsable();
            }

            [Fact]
            public void WhenRecastPeriodNotPassed()
            {
                _battleAbility.Recast = 1;
                _battleAbility.LastCast = DateTime.Now.AddMinutes(1);
                VerifyActionNotUsable();
            }

            public void VerifyActionNotUsable()
            {
                var memoryAPI = FindMemoryApi();
                var result = ActionFilters.ValidateBuffingAction(memoryAPI, _battleAbility, new List<IUnit>
                {
                    new FakeUnit()
                }).ToList();
                Assert.NotEmpty(result);
            }
        }

        public class Usable : ActionFilterTests
        {
            [Fact]
            public void TpInReserveRange()
            {
                _battleAbility.TPReserveHigh = 1;
                _battleAbility.TPReserveLow = 1;
                _player.TPCurrent = 1;
                VerifyActionUsable();
            }

            public void VerifyActionUsable()
            {
                GlobalFactory.BuildUnitService = x => new TestUnitService();
                var memory = FindMemoryApi();
                var result = ActionFilters.BuffingFilter(memory, _battleAbility);
                Assert.True(result);
            }
        }        

        public IMemoryAPI FindMemoryApi()
        {
            var memoryApi = new FakeMemoryAPI();
            memoryApi.Player = _player;
            memoryApi.Timer = _timer;
            return memoryApi;
        }
    }
}
