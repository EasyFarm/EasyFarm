using System;
using System.IO;
using System.Linq;
using EasyFarm.Classes;
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
        public void SerializationIsBackwardsCompatibleWithXml()
        {
            // Fixture setup
            var config = new Config();

            var path = FindFilePath();
            var xmlPersister = new XmlPersister();
            xmlPersister.Serialize(path, config);

            // Exercise system            
            var result = Record.Exception(() => Serialization.Deserialize<Config>(path));

            // Verify outcome
            Assert.IsNotType<AggregateException>(result);

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
