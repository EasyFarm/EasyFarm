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

using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.Tests.TestTypes;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class BattleStateTests
    {
        public class CheckTests : AbstractTestBase
        {
            [Fact]
            public void WhenEngagedSetAndEngagedShouldBattle()
            {
                var context = new TestContext();
                context.Config.IsEngageEnabled = true;
                context.Player.Status = Status.Fighting;
                context.IsFighting = true;
                context.Target = FindUnit();
                // Fixture setup
                BattleState sut = new BattleState();
                // Exercise system
                bool result = sut.Check(context);
                // Verify outcome
                Assert.True(result);
                // Teardown
            }

            [Fact]
            public void WhenEngagedNotSetShouldBattle()
            {
                // Fixture setup
                var context = new TestContext();
                context.Config.IsEngageEnabled = false;
                context.Target = FindUnit();
                context.IsFighting = true;
                BattleState sut = new BattleState();
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
                var context = new TestContext();
                context.IsFighting = true;
                context.Config.IsEngageEnabled = false;
                context.Target.IsValid = false;
                BattleState sut = new BattleState();
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
                var context = new TestContext();
                context.IsFighting = false;
                BattleState sut = new BattleState();
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
                var context = new TestContext();
                context.Player.HppCurrent = 25;
                context.Player.Status = Status.Standing;
                context.Config.LowHealth = 50;
                context.Config.HighHealth = 100;
                context.Config.IsHealthEnabled = true;
                BattleState sut = new BattleState();
                // Exercise system
                bool result = sut.Check(context);
                // Verify outcome
                Assert.False(result);
                // Teardown
            }
        }

        public class RunTests : AbstractTestBase
        {
            private BattleState CreateSut()
            {
                return new BattleState();
            }

            [Fact]
            public void WithValidActionWillSendCommand()
            {
                // Fixture setup
                var context = new TestContext();
                BattleAbility ability = FindJobAbility("test");
                ability.Command = "/jobability test <me>";
                context.Config.BattleLists["Battle"].Actions.Add(ability);
                BattleState sut = CreateSut();
                // Exercise system
                sut.Run(context);
                // Verify outcome
                Assert.Equal("/jobability test <me>", context.MockAPI.Windower.LastCommand);
                // Teardown
            }

            [Fact]
            public void WithInvalidActionWillNotSendCommand()
            {
                // Fixture setup
                var context = new TestContext();                
                BattleAbility ability = FindJobAbility("test");
                ability.IsEnabled = false;
                context.Config.BattleLists["Battle"].Actions.Add(ability);
                BattleState sut = CreateSut();
                // Exercise system
                sut.Run(context);
                // Verify outcome
                Assert.Null(context.MockAPI.Windower.LastCommand);
            }

            private static BattleAbility FindJobAbility(string actionName)
            {
                BattleAbility battleAbility = FindAbility();
                battleAbility.Name = actionName;
                battleAbility.AbilityType = AbilityType.Jobability;
                battleAbility.TargetType = TargetType.Self;
                return battleAbility;
            }
        }

        public class EnterTests
        {
            [Fact]
            public void WithHealingPlayerWillStandUp()
            {
                // Fixture setup
                var context = new TestContext();
                context.MockAPI.Player.Status = Status.Healing;
                BattleState sut = CreateSut();
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
                var context = new TestContext();
                context.MockAPI.Navigator.IsRunning = true;
                BattleState sut = CreateSut();
                // Exercise system
                sut.Enter(context);
                // Verify outcome
                Assert.False(context.MockAPI.Navigator.IsRunning);
                // Teardown
            }

            private BattleState CreateSut()
            {
                return new BattleState();
            }
        }
    }
}
