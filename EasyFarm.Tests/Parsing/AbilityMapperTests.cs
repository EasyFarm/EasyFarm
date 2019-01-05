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
using System;
using EasyFarm.Parsing;
using EliteMMO.API;
using Xunit;
using TargetType = EliteMMO.API.TargetType;

namespace EasyFarm.Tests.Parsing
{
    public class AbilityMapperTests
    {
        private readonly EliteAPI.IAbility input = new EliteAPI.IAbility();
        private readonly Ability expected = new Ability();
        private readonly AbilityMapper sut = new AbilityMapper();

        [Fact]
        public void Map_English()
        {
            // Setup fixture
            expected.English = "Cure";
            input.Name = new[] {"Cure"};
            // Exercise system
            var result = sut.Map(input);
            // Verify outcome
            Assert.Equal(expected.English, result.English);
            // Teardown
        }

        [Fact]
        public void Map_Engish_SourceNull_TargetEmpty()
        {
            // Setup fixture
            expected.English = "";
            input.Name = null;
            // Exercise system
            var result = sut.Map(input);
            // Verify outcome
            Assert.Equal(expected.English, result.English);
            // Teardown
        }

        [Fact]
        public void Map_Distance()
        {
            // Setup fixture
            expected.Distance = 21;
            input.Range = (char)21;
            // Exercise system
            var result = sut.Map(input);
            // Verify outcome
            Assert.Equal(expected.Distance, result.Distance);
            // Teardown
        }

        [Fact]
        public void Map_Index()
        {
            // Setup fixture
            expected.Index = 1;
            input.TimerID = 1;
            // Exercise system
            var result = sut.Map(input);
            // Verify outcome
            Assert.Equal(expected.Index, result.Index);
            // Teardown
        }

        [Fact]
        public void Map_Prefix()
        {
            // Setup fixture
            expected.Prefix = "/jobability";
            // Exercise system
            var result = sut.Map(input);
            // Verify outcome
            Assert.Equal(expected.Prefix, result.Prefix);
            // Teardown
        }

        [Fact]
        public void Map_TPCost()
        {
            // Setup fixture
            expected.TpCost = 1;
            input.TP = 1;
            // Exercise system
            var result = sut.Map(input);
            // Verify outcome
            Assert.Equal(expected.TpCost, result.TpCost);
            // Teardown
        }

        [Theory]
        [InlineData(TargetType.None, EasyFarm.Parsing.TargetType.Unknown)]
        [InlineData(TargetType.Self, EasyFarm.Parsing.TargetType.Self)]
        [InlineData(TargetType.Player, EasyFarm.Parsing.TargetType.Player)]
        [InlineData(TargetType.PartyMember, EasyFarm.Parsing.TargetType.Party)]
        [InlineData(TargetType.AllianceMember, EasyFarm.Parsing.TargetType.Ally)]
        [InlineData(TargetType.Npc, EasyFarm.Parsing.TargetType.Npc)]
        [InlineData(TargetType.Enemy, EasyFarm.Parsing.TargetType.Enemy)]
        [InlineData(TargetType.None, EasyFarm.Parsing.TargetType.Unknown)]
        public void Map_Targets(TargetType inputTargetType, EasyFarm.Parsing.TargetType expectedTargetType)
        {
            // Setup fixture
            expected.TargetType = expectedTargetType;
            input.ValidTargets = (UInt16) inputTargetType;
            // Exercise system
            var result = sut.Map(input);
            // Verify outcome
            Assert.Equal(expected.TargetType, result.TargetType);
            // Teardown
        }

        [Theory]
        [InlineData(TargetType.Unknown, 64)]
        [InlineData(TargetType.CorpseOnly, 128)]
        [InlineData(TargetType.Corpse, 157)]
        [InlineData(TargetType.Self | TargetType.Player, 3)]
        public void Map_TargetType_UnmappedValues(TargetType inputTargetType, EasyFarm.Parsing.TargetType expectedTargetType)
        {
            // Setup fixture
            expected.TargetType = expectedTargetType;
            input.ValidTargets = (UInt16)inputTargetType;
            // Exercise system
            var result = sut.Map(input);
            // Verify outcome
            Assert.Equal(expected.TargetType, result.TargetType);
            // Teardown
        }
    }
}
