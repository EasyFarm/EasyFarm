
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

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
