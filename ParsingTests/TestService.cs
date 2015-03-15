using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Parsing.Services;
using Parsing.Types;
using System.Linq;
using Parsing.Extraction;
using System.Xml.Linq;
using Parsing.Mapping;
using Parsing.Augmenting;
using Parsing.Abilities;

namespace ParsingTests
{
    [TestClass]
    public class TestService
    {
        [TestMethod]
        public void TestCreateAbility()
        {           
            AbilityService Retriever = new AbilityService(
                Path.Combine(Environment.CurrentDirectory, "Resources"));

            // Test create ability for all major skill types. 
            var cure = Retriever.CreateAbility("Cure");
            var provoke = Retriever.CreateAbility("Provoke");
            var ragingAxe = Retriever.CreateAbility("Raging Axe");

            Assert.AreEqual(8, cure.MPCost);
            Assert.AreEqual(AbilityType.Jobability, provoke.AbilityType);
            Assert.AreEqual(AbilityType.Weaponskill, ragingAxe.AbilityType);
        }
    }
}
