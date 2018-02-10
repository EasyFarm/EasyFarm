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
using System.IO;
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Persistence;
using EasyFarm.UserSettings;
using MemoryAPI;
using Ploeh.AutoFixture;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class SerializationTests
    {
        [Fact]
        public void CanSerializeBattleAbilityWithNullName()
        {
            // Fixture setup
            var config = FindConfigContainingBattleAbilityWithNullName();
            var path = FindFilePath();

            // Exercise system        
            var result = Record.Exception(() => Serialization.Serialize(path, config));

            // Verify outcome
            Assert.Null(result);

            // Teardown
            File.Delete(path);
        }

        private static Config FindConfigContainingBattleAbilityWithNullName()
        {
            var fixture = new Fixture();
            var config = fixture.Build<Config>()
                .With(x => x.BattleLists, new BattleLists(fixture.CreateMany<BattleList>()))
                .Create();

            var battleAbility = fixture.Build<BattleAbility>()
                .With(x => x.Name, null)
                .Create();

            config.BattleLists[0].Actions.Add(battleAbility);
            return config;
        }

        [Fact]
        public void SerializationWithUndefinedEnumValueReturnsThatValue()
        {
            // Fixture setup
            var expectedZone = (Zone) 10000;
            var config = new Config {Route = {Zone = expectedZone}};

            var path = FindFilePath();
            Serialization.Serialize(path, config);

            // Exercise system            
            var result = Serialization.Deserialize<Config>(path);

            // Verify outcome
            var actualZone = result.Route.Zone;
            Assert.Equal(expectedZone, actualZone);

            // Teardown
            File.Delete(path);
        }       

        [Fact]
        public void SerializationWhenDeserializationFailsThrowsException()
        {
            // Fixture setup
            var path = FindFilePath();
            File.WriteAllText(path, @"Not a valid format!");

            // Exercise system
            var result = Record.Exception(() => Serialization.Deserialize<Config>(path));

            // Verify outcome
            Assert.IsType<AggregateException>(result);

            // Teardown
            File.Delete(path);
        }

        private static string FindFilePath()
        {
            return Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());
        }
    }
}
