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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.States;
using EasyFarm.Tests.TestTypes;
using EasyFarm.UserSettings;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class BattleStateTests : AbstractTestFixture
    {
        public class Check
        {
            [Fact]
            public void WhenEngagedSetAndEngagedShouldBattle()
            {
                // Fixture setup                
                Config.Instance.IsEngageEnabled = true;                

                var memory = new FakeMemoryAPI();
                var player = FindPlayer();
                player.Status = Status.Fighting;
                memory.Player = player;

                var stateMemory = new StateMemory(memory)
                {
                    Target = FindUnit(),
                    IsFighting = true
                };

                var sut = new BattleState(stateMemory);

                // Exercise system
                var result = sut.Check();

                // Verify outcome
                Assert.True(result);

                // Teardown
            }

            [Fact]
            public void WhenEngagedNotSetShouldBattle()
            {
                // Fixture setup                
                Config.Instance.IsEngageEnabled = false;

                var memory = new FakeMemoryAPI();
                var player = FindPlayer();
                player.Status = Status.Standing;
                memory.Player = player;

                var stateMemory = new StateMemory(memory)
                {
                    Target = FindUnit(),
                    IsFighting = true
                };

                var sut = new BattleState(stateMemory);

                // Exercise system
                var result = sut.Check();

                // Verify outcome
                Assert.True(result);

                // Teardown
            }

            [Fact]
            public void WithNonValidUnitShouldNotBattle()
            {
                // Fixture setup
                Config.Instance.IsEngageEnabled = false;

                var memory = new FakeMemoryAPI { Player = FindPlayer() };

                var stateMemory = new StateMemory(memory)
                {
                    Target = FindNonValidUnit(),
                    IsFighting = true
                };

                var sut = new BattleState(stateMemory);

                // Exercise system
                var result = sut.Check();

                // Verify outcome
                Assert.False(result);

                // Teardown
            }

            [Fact]
            public void WhenNotFightingShouldntBattle()
            {
                // Fixture setup
                Config.Instance.IsEngageEnabled = false;
                var memory = new FakeMemoryAPI { Player = FindPlayer() };

                var stateMemory = new StateMemory(memory)
                {
                    Target = FindUnit(),
                    IsFighting = false
                };

                var sut = new BattleState(stateMemory);

                // Exercise system
                var result = sut.Check();

                // Verify outcome
                Assert.False(result);

                // Teardown
            }

            [Fact]
            public void WhenInjuredShouldntBattle()
            {
                // Fixture setup
                var player = FindPlayer();
                player.HPPCurrent = 25;
                Config.Instance.LowHealth = 50;
                Config.Instance.HighHealth = 100;
                Config.Instance.IsHealthEnabled = true;
                player.Status = Status.Standing;

                Config.Instance.IsEngageEnabled = false;

                var memory = new FakeMemoryAPI { Player = player };

                var stateMemory = new StateMemory(memory)
                {
                    Target = FindUnit(),
                    IsFighting = false
                };

                var sut = new BattleState(stateMemory);

                UnitService.Units = new List<IUnit>();

                // Exercise system
                var result = sut.Check();

                // Verify outcome
                Assert.False(result);

                // Teardown
            }

            [Fact]
            public void WithInvalidTargetShouldntBattle()
            {
                // Fixture setup
                var player = FindPlayer();
                player.Status = Status.Standing;
                Config.Instance.IsEngageEnabled = false;

                var memory = new FakeMemoryAPI { Player = player };

                var stateMemory = new StateMemory(memory)
                {
                    Target = FindNonValidUnit(),
                    IsFighting = true
                };

                var sut = new BattleState(stateMemory);

                UnitService.Units = new List<IUnit>();

                // Exercise system
                var result = sut.Check();                

                // Verify outcome
                Assert.False(result);

                // Teardown
            }
        }

        public class Run
        {
            public Run()
            {
                GlobalFactory.BuildUnitService = fface => new TestUnitService();
            }

            [Theory]
            [InlineData(AbilityType.Jobability, TargetType.Self, "test", "/jobability test <me>")]
            [InlineData(AbilityType.Range, TargetType.Enemy, "Ranged", "/range <t>")]
            [InlineData(AbilityType.Magic, TargetType.Self, "ケアルIII", "/magic ケアルIII <me>")]
            [InlineData(AbilityType.Magic, TargetType.Self, "Cure III", "/magic \"Cure III\" <me>")]
            public void WithValidActionWillSendCommand(
                AbilityType abilityType, 
                TargetType targetType,
                string actionName,
                string expectedCommand)
            {
                var windower = FindWindower();
                var sut = CreateSut(windower);
                var actions = FindBattleActions();

                var ability = FindAction(abilityType, targetType, actionName);
                actions.Add(ability);

                // Exercise system
                sut.Run();

                // Verify outcome
                Assert.Equal(expectedCommand, windower.LastCommand);

                // Teardown
            }

            [Fact]
            public void WithInvalidActionWillNotSendCommand()
            {
                // Fixture setup
                var windower = FindWindower();
                var sut = CreateSut(windower);
                var actions = FindBattleActions();

                var ability = FindJobAbility("test");
                ability.IsEnabled = false;
                actions.Add(ability);

                // Exercise system
                sut.Run();

                // Verify outcome
                Assert.Null(windower.LastCommand);
            }

            private static BattleAbility FindAction(
                AbilityType abilityType, 
                TargetType targetType, 
                string actionName)
            {
                var battleAbility = FindAbility();
                battleAbility.Name = actionName;
                battleAbility.AbilityType = abilityType;
                battleAbility.Ability.TargetType = targetType;
                return battleAbility;
            }

            private static BattleAbility FindJobAbility(string actionName)
            {
                var battleAbility = FindAbility();
                battleAbility.Name = actionName;
                battleAbility.AbilityType = AbilityType.Jobability;
                battleAbility.Ability.TargetType = TargetType.Self;
                return battleAbility;
            }

            private static BattleState CreateSut(FakeWindower windower)
            {
                var player = FindPlayer();
                var navigator = FindNavigator();
                var target = FindTarget();
                var timer = FindTimer();

                var memory = new FakeMemoryAPI()
                {
                    Player = player,
                    Windower = windower,
                    Navigator = navigator,
                    Target = target,
                    Timer = timer
                };

                var sut = new BattleState(new StateMemory(memory)
                {
                    Target = FindUnit()
                });

                return sut;
            }

            private static ObservableCollection<BattleAbility> FindBattleActions()
            {
                var moves = Config.Instance.BattleLists["Battle"].Actions;
                moves.Clear();
                return moves;
            }
        }

        public class Enter
        {
            [Fact]
            public void WithHealingPlayerWillStandUp()
            {
                // Fixture setup
                var windower = FindWindower();
                var player = FindPlayer();
                var sut = CreateSut(windower, player);

                player.Status = Status.Healing;                
                
                // Exercise system
                sut.Enter();

                // Verify outcome
                Assert.Equal(Constants.RestingOff, windower.LastCommand);

                // Teardown
            }

            [Fact]
            public void WillStopPlayerFromMoving()
            {
                // Fixture setup
                var navigator = FindNavigator();
                var sut = CreateSut(navigator);

                // Exercise system
                sut.Enter();

                // Verify outcome
                Assert.True(navigator.ResetWasCalled);

                // Teardown
            }

            private BattleState CreateSut(FakeNavigator navigator)
            {
                var memory = new FakeMemoryAPI()
                {
                    Navigator = navigator,
                    Player = FindPlayer()
                };

                return new BattleState(new StateMemory(memory));
            }

            private static BattleState CreateSut(FakeWindower windower, FakePlayer player)
            {
                var navigator = FindNavigator();

                var memory = new FakeMemoryAPI()
                {
                    Windower = windower,
                    Player = player,
                    Navigator = navigator
                };

                return new BattleState(new StateMemory(memory));
            }
        }
    }
}
