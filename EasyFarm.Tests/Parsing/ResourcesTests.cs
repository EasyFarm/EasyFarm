using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EasyFarm.Parsing;
using Xunit;
using Xunit.Sdk;

namespace EasyFarm.Tests.Parsing
{
    public class ResourcesTests
    {
        private static readonly AbilityService AbilityService = new AbilityService("Resources");

        /// <summary>
        /// Ensure that each resource class has the correct data type mappings by trying to convert
        /// Resource objects to ResourceClass specific ones.
        /// If there is a type converson problem, these tests will fail.
        /// </summary>
        /// <param name="resourceClass"></param>
        [Theory]
        [MemberData(nameof(ResourceClasses))]
        public void EnsureResourceIsParsable(Type resourceClass)
        {
            // Setup fixture
            var sut = CreateSut();
            // Exercise system
            var result = sut.Find(resourceClass);
            // Verify outcome
            Assert.NotEmpty(result);
            // Teardown
        }

        [Fact]
        public void Spell_CureReturnsExpectedResults()
        {
            // Setup fixture
            var sut = CreateSut();
            // Exercise system
            var result = sut.GetAbilitiesWithName("Cure").FirstOrDefault(x => x.AbilityType == AbilityType.Magic);
            // Verify outcome
            Assert.NotNull(result);
            Assert.Equal("Cure", result.English);
            Assert.Equal(1, result.Index);
            // Teardown
        }

        [Fact]
        public void Item_EchoDropsReturnsExpectedResults()
        {
            // Setup fixture
            var sut = CreateSut();
            // Exercise system
            var result = sut.GetAbilitiesWithName("Echo Drops").FirstOrDefault();
            // Verify outcome
            Assert.NotNull(result);
            Assert.Equal("Echo Drops", result.English);
            Assert.Equal(AbilityType.Item, result.AbilityType);
            // Teardown
        }

        [Fact]
        public void WeaponSkill_FastBladeReturnsExpectedResults()
        {
            // Setup fixture
            var sut = CreateSut();
            // Exercise system
            var result = sut.GetAbilitiesWithName("Fast Blade")
                .FirstOrDefault(x => x.AbilityType == AbilityType.Weaponskill);
            // Verify outcome
            Assert.NotNull(result);
            Assert.Equal("Fast Blade", result.English);
            Assert.Equal(AbilityType.Weaponskill, result.AbilityType);
            Assert.Equal(900, result.Index);
            Assert.Equal(1000, result.TpCost);
            // Teardown
        }

        [Fact]
        public void GetAbilitiesWithName_ShouldNotBeCaseSensitive()
        {
            // Setup fixture
            var sut = CreateSut();
            // Exercise system
            var result = sut.GetAbilitiesWithName("fast blade");
            // Verify outcome
            Assert.NotEmpty(result);
            // Teardown
        }

        private static AbilityService CreateSut()
        {
            return AbilityService;
        }

        public static IEnumerable<object[]> ResourceClasses
        {
            get
            {
                return Assembly.GetAssembly(typeof(App)).ExportedTypes
                    .Where(type => type.FullName.Contains("Resources+"))
                    .Where(type => type.IsClass)
                    .Select(type => new object[] { type })
                    .ToList();
            }
        }
    }

    public class CustomAssert
    {
        public static void Incomplete()
        {
            throw new XunitException("This test is incomplete.");
        }
    }
}
