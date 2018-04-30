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
using EasyFarm.Tests.TestTypes.Mocks;

namespace EasyFarm.Tests.Classes
{
    public class ExecutorTests : AbstractTestBase
    {
        private readonly MockEliteAPIAdapter _memoryApi;

        public ExecutorTests()
        {
            _memoryApi = new MockEliteAPIAdapter(MockEliteAPI);
        }

        /// <summary>
        /// Ignoring test since it will always hang right now. 
        /// 
        /// Ensure cast does not seem to proceed, and will loop indefintiely. 
        /// </summary>
        public void UseBuffingActionsWillUseAbilityNameToCastCommand()
        {
            // Fixture setup
            BattleAbility battleAbility = FindAbility();
            battleAbility.Name = "test";
            battleAbility.AbilityType = AbilityType.Magic;
            Executor sut = new Executor(_memoryApi);
            // Exercise system
            sut.UseBuffingActions(new List<BattleAbility> { battleAbility });
            // Verify outcome
            Assert.Equal("/magic test <t>", MockEliteAPI.Windower.LastCommand);
            // Teardown
        }

        [Fact]
        public void UsedTargetedActionsWillUseAbilityNameToCastCommand()
        {
            // Fixture setup
            BattleAbility battleAbility = FindAbility();
            battleAbility.Name = "test";
            battleAbility.AbilityType = AbilityType.Magic;
            battleAbility.Command = "/magic test <t>";
            IUnit unit = FindUnit();
            Executor sut = new Executor(_memoryApi);
            // Exercise system
            sut.UseTargetedActions(new List<BattleAbility> { battleAbility }, unit);
            // Verify outcome
            Assert.Equal("/magic test <t>", MockEliteAPI.Windower.LastCommand);
            // Teardown
        }

        [Fact]
        public void UseBuffingActionsWithNullActionListThrows()
        {
            Executor sut = new Executor(_memoryApi);
            Exception result = Record.Exception(() => sut.UseBuffingActions(null));
            Assert.IsType<ArgumentNullException>(result);
        }
    }
}
