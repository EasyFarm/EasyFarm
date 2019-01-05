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
using EasyFarm.Parsing;
using Xunit;

namespace EasyFarm.Tests.Parsing
{
    public class AbilityTests
    {
        [Fact]
        public void JapanseCommandsDoNotHaveSpaces()
        {
            var sut = new Ability()
            {
                Prefix = "/magic",
                English = "ケアルIII",
                TargetType = TargetType.Self
            };

            Assert.Equal("/magic ケアルIII <me>", sut.Command);
        }

        [Fact]
        public void RangeCommandReturnsCorrectResults()
        {
            var sut = new Ability()
            {
                Prefix = "/range",
                AbilityType = AbilityType.Range,
                TargetType = TargetType.Enemy
            };

            Assert.Equal("/range <t>", sut.Command);
        }

        [Fact]
        public void JobAbilityCommandReturnsCorrectResults()
        {
            var sut = new Ability()
            {
                Prefix = "/jobability",
                English = "Boost",
                TargetType = TargetType.Self
            };

            Assert.Equal("/jobability Boost <me>", sut.Command);
        }

        [Fact]
        public void CommandWithSpaceIsSurroundByQuotes()
        {
            var sut = new Ability()
            {
                Prefix = "/magic",
                English = "Cure III",
                TargetType = TargetType.Self
            };

            Assert.Equal("/magic \"Cure III\" <me>", sut.Command);
        }
    }
}
