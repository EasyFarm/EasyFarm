using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.States;
using EasyFarm.Tests.Classes;
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
        }

        public class Run
        {
            [Fact]
            public void WithValidActionAndTargetWillSendCommandToGame()
            {
                // Fixture setup
                var player = FindPlayer();
                var windower = FindWindower();
                var navigator = FindNavigator();
                var target = FindTarget();

                var sut = new BattleState(new FakeMemoryAPI()
                {
                    Player = player,
                    Windower = windower,
                    Navigator = navigator,
                    Target = target
                });
                
                var actions = FindBattleActions();
                actions.Clear();

                var battleAbility = FindAbility();
                actions.Add(battleAbility);

                battleAbility.Name = "test";
                battleAbility.AbilityType = AbilityType.Jobability;
                battleAbility.Ability.TargetType = TargetType.Self;

                CombatState.Target = FindUnit();
                
                // Exercise system
                sut.Run();

                // Verify outcome
                Assert.Equal("/jobability \"test\" <me>", windower.LastCommand);

                // Teardown
            }

            private static ObservableCollection<BattleAbility> FindBattleActions()
            {
                var moves = Config.Instance.BattleLists["Battle"].Actions;
                return moves;
            }
        }
    }
}
