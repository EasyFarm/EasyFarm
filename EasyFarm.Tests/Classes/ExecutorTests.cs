using System;
using System.Collections.Generic;
using Xunit;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.Tests.TestTypes;

namespace EasyFarm.Tests.Classes
{
    public class ExecutorTests : AbstractTestFixture
    {
        [Fact]
        public void UseBuffingActionsWillUseAbilityNameToCastCommand()
        {
            // Fixture setup
            var battleAbility = FindAbility();
            battleAbility.Name = "test";
            battleAbility.AbilityType = AbilityType.Magic;

            var windower = FindWindower();
            var player = FindPlayer();

            var sut = CreateSut(windower, player);

            // Exercise system
            sut.UseBuffingActions(new List<BattleAbility> { battleAbility });

            // Verify outcome
            Assert.Equal("/magic \"test\" <t>", windower.LastCommand);

            // Teardown
        }

        [Fact]
        public void UsedTargetedActionsWillUseAbilityNameToCastCommand()
        {
            // Fixture setup
            var battleAbility = FindAbility();
            battleAbility.Name = "test";
            battleAbility.AbilityType = AbilityType.Magic;

            var windower = FindWindower();
            var player = FindPlayer();
            var navigator = FindNavigator();
            var target = FindTarget();

            var unit = FindUnit();
            var sut = CreateSut(windower, player, navigator, target);

            // Exercise system
            sut.UseTargetedActions(new List<BattleAbility> { battleAbility }, unit);

            // Verify outcome
            Assert.Equal("/magic \"test\" <t>", windower.LastCommand);

            // Teardown
        }        

        [Fact]
        public void UseBuffingActionsWithNullActionListThrows()
        {
            var sut = CreateSut();
            var result = Record.Exception(() => sut.UseBuffingActions(null));
            Assert.IsType<ArgumentNullException>(result);
        }

        private Executor CreateSut()
        {
            var memory = new FakeMemoryAPI();
            var sut = new Executor(memory);
            return sut;
        }

        private Executor CreateSut(FakeWindower windower, FakePlayer player)
        {
            var memory = new FakeMemoryAPI();
            memory.Player = player;
            memory.Windower = windower;

            var sut = new Executor(memory);
            return sut;
        }

        private Executor CreateSut(
            FakeWindower windower, 
            FakePlayer player, 
            FakeNavigator navigator, 
            FakeTarget target)
        {
            var memory = new FakeMemoryAPI();
            memory.Player = player;
            memory.Windower = windower;
            memory.Navigator = navigator;
            memory.Target = target;

            var sut = new Executor(memory);
            return sut;
        }
    }
}
