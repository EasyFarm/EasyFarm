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
using Xunit;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.Tests.TestTypes;

namespace EasyFarm.Tests.Classes
{
    public class ExecutorTests : AbstractTestFixture
    {
        /// <summary>
        /// Ignoring test since it will always hang right now. 
        /// 
        /// Ensure cast does not seem to proceed, and will loop indefintiely. 
        /// </summary>
        public void UseBuffingActionsWillUseAbilityNameToCastCommand()
        {
            // Fixture setup
            var battleAbility = FindAbility();
            battleAbility.Name = "test";
            battleAbility.AbilityType = AbilityType.Magic;

            var windower = FindWindower();
            var player = FindPlayer();
            var timer = FindTimer();

            var sut = CreateSut(windower, player, timer);

            // Exercise system
            sut.UseBuffingActions(new List<BattleAbility> { battleAbility });

            // Verify outcome
            Assert.Equal("/magic test <t>", windower.LastCommand);

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
            Assert.Equal("/magic test <t>", windower.LastCommand);

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

        private Executor CreateSut(
            FakeWindower windower, 
            FakePlayer player, 
            FakeTimer timer)
        {
            var memory = new FakeMemoryAPI();
            memory.Player = player;
            memory.Windower = windower;
            memory.Timer = timer;

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
