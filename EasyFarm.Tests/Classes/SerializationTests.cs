using System;
using System.IO;
using EasyFarm.Classes;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class SerializationTests
    {
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
