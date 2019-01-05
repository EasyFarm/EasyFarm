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
using System.Linq;
using EasyFarm.Parsing;
using Xunit;

namespace EasyFarm.Tests.Parsing
{
    public class ResourcesTests
    {
        [Fact]
        public void Ranged_ShouldReturnRangedAttack()
        {
            // Fixture setup
            var sut = new AbilityService(null);
            // Excercise system
            var result = sut.GetAbilitiesWithName("Ranged").FirstOrDefault();
            // Verify outcome
            Assert.NotNull(result);
            Assert.Equal(AbilityType.Range, result.AbilityType);
            Assert.Equal(TargetType.Enemy, result.TargetType);
            Assert.Equal("Ranged", result.English);
            Assert.Equal("/range <t>", result.Command);
            // Teardown	
        }

        [Fact]
        public void Ranged_IsCaseInsensitive()
        {
            // Fixture setup
            var sut = new AbilityService(null);
            // Excercise system
            var result = sut.GetAbilitiesWithName("ranged").FirstOrDefault();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown	
        }
    }
}
