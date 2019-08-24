// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
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
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.Tests.TestTypes;
using MemoryAPI;
using Xunit;
using Xunit.Sdk;

namespace EasyFarm.Tests.States
{
    public class BattleStateTests : AbstractTestBase
    {
        private static readonly TestContext context = new TestContext();
        private static readonly BattleState sut = new BattleState();

        [Fact]
        public void WhenEngagedSetAndEngagedShouldBattle()
        {
            // Fixture setup
            context.Config.IsEngageEnabled = true;
            context.Player.Status = Status.Fighting;
            context.IsFighting = true;
            context.Target = FindUnit();
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact(Skip = "Race")]
        public void WhenEngagedNotSetShouldBattle()
        {
            // Fixture setup
            context.Config.IsEngageEnabled = false;
            context.Target = FindUnit();
            context.IsFighting = true;
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void WithNonValidUnitShouldNotBattle()
        {
            // Fixture setup
            context.IsFighting = true;
            context.Config.IsEngageEnabled = false;
            context.Target.IsValid = false;
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void WhenNotFightingShouldntBattle()
        {
            // Fixture setup
            context.IsFighting = false;
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void WhenInjuredShouldntBattle()
        {
            // Fixture setup
            context.Player.HppCurrent = 25;
            context.Player.Status = Status.Standing;
            context.Config.LowHealth = 50;
            context.Config.HighHealth = 100;
            context.Config.IsHealthEnabled = true;
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void WithValidActionWillSendCommand()
        {
            // Fixture setup
            BattleAbility ability = FindAbility();
            ability.Command = "/jobability test <me>";
            context.Config.BattleLists["Battle"].Actions.Add(ability);
            // Exercise system
            sut.Run(context);
            // Verify outcome
            Assert.Equal("/jobability test <me>", context.MockAPI.Windower.LastCommand);
            // Teardown
        }

        [Fact(Skip = "Race")]
        public void WithInvalidActionWillNotSendCommand()
        {
            // Fixture setup
            BattleAbility ability = FindAbility();
            ability.IsEnabled = false;
            context.Config.BattleLists["Battle"].Actions.Add(ability);
            // Exercise system
            sut.Run(context);
            // Verify outcome
            Assert.Null(context.MockAPI.Windower.LastCommand);
        }

        [Fact]
        public void WithHealingPlayerWillStandUp()
        {
            // Fixture setup
            context.MockAPI.Player.Status = Status.Healing;
            // Exercise system
            sut.Enter(context);
            // Verify outcome
            Assert.Equal(Status.Standing, context.MockAPI.Player.Status);
            // Teardown
        }

        [Fact]
        public void WillStopPlayerFromMoving()
        {
            // Fixture setup
            context.MockAPI.Navigator.IsRunning = true;
            // Exercise system
            sut.Enter(context);
            // Verify outcome
            Assert.False(context.MockAPI.Navigator.IsRunning);
            // Teardown
        }
    }
}
