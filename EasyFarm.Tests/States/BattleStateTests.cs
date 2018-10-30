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

using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.States;
using EasyFarm.Tests.TestTypes;
using EasyFarm.Tests.TestTypes.Mocks;
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
                // Fixture setup
                MockConfig.IsEngageEnabled = true;
                MockGameAPI.Mock.Player.Status = Status.Fighting;
                StateMemory memory = CreateStateMemory();
                memory.Target = FindUnit();
                memory.IsFighting = true;
                BattleState sut = new BattleState(memory);
                // Exercise system
                bool result = sut.Check();
                // Verify outcome
                Assert.True(result);
                // Teardown
            }

            [Fact]
            public void WhenEngagedNotSetShouldBattle()
            {
                // Fixture setup
                MockConfig.IsEngageEnabled = false;
                StateMemory memory = CreateStateMemory();
                memory.Target = FindUnit();
                memory.IsFighting = true;
                BattleState sut = new BattleState(memory);
                // Exercise system
                bool result = sut.Check();
                // Verify outcome
                Assert.True(result);
                // Teardown
            }

            [Fact]
            public void WithNonValidUnitShouldNotBattle()
            {
                // Fixture setup
                MockConfig.IsEngageEnabled = false;
                StateMemory memory = CreateStateMemory(targetValid: false);
                memory.Target = FindNonValidUnit();
                memory.IsFighting = true;
                BattleState sut = new BattleState(memory);
                // Exercise system
                bool result = sut.Check();
                // Verify outcome
                Assert.False(result);
                // Teardown
            }

            [Fact]
            public void WhenNotFightingShouldntBattle()
            {
                // Fixture setup
                MockConfig.IsEngageEnabled = false;
                StateMemory memory = CreateStateMemory();
                memory.Target = FindUnit();
                memory.IsFighting = false;
                BattleState sut = new BattleState(memory);
                // Exercise system
                bool result = sut.Check();
                // Verify outcome
                Assert.False(result);
                // Teardown
            }

            [Fact]
            public void WhenInjuredShouldntBattle()
            {
                // Fixture setup
                StateMemory memory = CreateStateMemory();
                memory.Target = FindUnit();
                memory.IsFighting = false;
                MockGameAPI.Mock.Player.HPPCurrent = 25;
                MockGameAPI.Mock.Player.Status = Status.Standing;
                MockConfig.LowHealth = 50;
                MockConfig.HighHealth = 100;
                MockConfig.IsHealthEnabled = true;
                MockConfig.IsEngageEnabled = false;
                BattleState sut = new BattleState(memory);
                // Exercise system
                bool result = sut.Check();
                // Verify outcome
                Assert.False(result);
                // Teardown
            }

            [Fact]
            public void WithInvalidTargetShouldntBattle()
            {
                // Fixture setup
                StateMemory memory = CreateStateMemory(targetValid: false);
                memory.Target = FindNonValidUnit();
                memory.IsFighting = true;
                MockGameAPI.Mock.Player.Status = Status.Standing;
                MockConfig.IsEngageEnabled = false;
                BattleState sut = new BattleState(memory);
                // Exercise system
                bool result = sut.Check();
                // Verify outcome
                Assert.False(result);
                // Teardown
            }
        }

        public class RunTests : AbstractTestBase
        {
            private BattleState CreateSut()
            {
                var memory = CreateStateMemory();
                memory.Target = FindUnit();
                BattleState sut = new BattleState(memory);
                return sut;
            }

            [Fact]
            public void WithValidActionWillSendCommand()
            {
                // Fixture setup
                BattleState sut = CreateSut();
                ObservableCollection<BattleAbility> actions = FindBattleActions();
                BattleAbility ability = FindJobAbility("test");
                ability.Command = "/jobability test <me>";
                actions.Add(ability);
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.Equal("/jobability test <me>", MockGameAPI.Mock.Windower.LastCommand);
                // Teardown
            }

            [Fact]
            public void WithInvalidActionWillNotSendCommand()
            {
                // Fixture setup
                BattleState sut = CreateSut();
                ObservableCollection<BattleAbility> actions = FindBattleActions();
                BattleAbility ability = FindJobAbility("test");
                ability.IsEnabled = false;
                actions.Add(ability);
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.Null(MockGameAPI.Mock.Windower.LastCommand);
            }

            private static BattleAbility FindJobAbility(string actionName)
            {
                BattleAbility battleAbility = FindAbility();
                battleAbility.Name = actionName;
                battleAbility.AbilityType = AbilityType.Jobability;
                battleAbility.TargetType = TargetType.Self;
                return battleAbility;
            }

            private ObservableCollection<BattleAbility> FindBattleActions()
            {
                ObservableCollection<BattleAbility> moves = MockConfig.BattleLists["Battle"].Actions;
                moves.Clear();
                return moves;
            }
        }

        public class EnterTests : AbstractTestBase
        {
            [Fact]
            public void WithHealingPlayerWillStandUp()
            {
                // Fixture setup
                BattleState sut = CreateSut();
                MockGameAPI.Mock.Player.Status = Status.Healing;
                // Exercise system
                sut.Enter();
                // Verify outcome
                Assert.Equal(Status.Standing, MockGameAPI.Mock.Player.Status);
                // Teardown
            }

            [Fact]
            public void WillStopPlayerFromMoving()
            {
                // Fixture setup
                MockGameAPI.Mock.Navigator.IsRunning = true;
                BattleState sut = CreateSut();
                // Exercise system
                sut.Enter();
                // Verify outcome
                Assert.False(MockGameAPI.Mock.Navigator.IsRunning);
                // Teardown
            }

            private BattleState CreateSut()
            {
                return new BattleState(CreateStateMemory());
            }
        }
    }
}
