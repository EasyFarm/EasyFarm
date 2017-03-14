using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.States;
using EasyFarm.Tests.TestTypes;
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
                CombatState.Target = FindUnit();
                CombatState.IsFighting = true;
                Config.Instance.IsEngageEnabled = true;

                var memory = new FakeMemoryAPI();
                var player = FindPlayer();
                player.Status = Status.Fighting;
                memory.Player = player;

                var sut = new BattleState(memory);

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
                CombatState.Target = FindUnit();
                CombatState.IsFighting = true;
                Config.Instance.IsEngageEnabled = false;

                var memory = new FakeMemoryAPI();
                var player = FindPlayer();
                player.Status = Status.Standing;
                memory.Player = player;

                var sut = new BattleState(memory);

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
                CombatState.Target = FindNonValidUnit();
                CombatState.IsFighting = true;
                Config.Instance.IsEngageEnabled = false;

                var memory = new FakeMemoryAPI { Player = FindPlayer() };
                var sut = new BattleState(memory);

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
                CombatState.IsFighting = false;
                CombatState.Target = FindUnit();
                Config.Instance.IsEngageEnabled = false;
                var memory = new FakeMemoryAPI { Player = FindPlayer() };
                var sut = new BattleState(memory);

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

                CombatState.IsFighting = true;
                Config.Instance.IsEngageEnabled = false;
                CombatState.Target = FindUnit();

                var memory = new FakeMemoryAPI { Player = player };
                var sut = new BattleState(memory);

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

                CombatState.IsFighting = true;
                Config.Instance.IsEngageEnabled = false;
                CombatState.Target = FindNonValidUnit();

                var memory = new FakeMemoryAPI { Player = player };
                var sut = new BattleState(memory);

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

                var sut = new BattleState(new FakeMemoryAPI()
                {
                    Player = player,
                    Windower = windower,
                    Navigator = navigator,
                    Target = target,
                    Timer = timer
                });

                CombatState.Target = FindUnit();

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
                return new BattleState(new FakeMemoryAPI()
                {
                    Navigator = navigator,
                    Player = FindPlayer()
                });
            }

            private static BattleState CreateSut(FakeWindower windower, FakePlayer player)
            {
                var navigator = FindNavigator();

                var sut = new BattleState(new FakeMemoryAPI()
                {
                    Windower = windower,
                    Player = player,
                    Navigator = navigator
                });
                return sut;
            }
        }
    }
}
