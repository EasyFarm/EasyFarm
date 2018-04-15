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
using MemoryAPI;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.Tests.TestTypes;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class AbilityUtilsTests
    {
        private readonly BattleAbility _ability = new BattleAbility();

        [Theory]
        [InlineData(AbilityType.Magic)]
        [InlineData(AbilityType.Jobability)]
        [InlineData(AbilityType.Weaponskill)]
        public void IsRecastableWhenNotOnRecast(AbilityType abilityType)
        {
            _ability.AbilityType = abilityType;
            var memoryApi = new FakeMemoryAPI();
            memoryApi.Timer = new FakeTimer();
            var result = AbilityUtils.IsRecastable(memoryApi, _ability);
            Assert.True(result);
        }

        [Theory]
        [InlineData(AbilityType.Magic)]
        [InlineData(AbilityType.Jobability)]
        public void NotRecastableWhenOnRecast(AbilityType abilityType)
        {
            _ability.AbilityType = abilityType;
            var memoryApi = new FakeMemoryAPI();
            memoryApi.Timer = new FakeTimer() { ActionRecast = 1 };
            var result = AbilityUtils.IsRecastable(memoryApi, _ability);
            Assert.False(result);
        }
    }
}
