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
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parsing.Services;
using Parsing.Types;

namespace ParsingTests
{
    [TestClass]
    public class TestService
    {
        [TestMethod]
        public void TestCreateAbility()
        {
            var retriever = new AbilityService(
                Path.Combine(Environment.CurrentDirectory, "Resources"));

            // Test create ability for all major skill types. 
            var cure = retriever.CreateAbility("Cure");
            var provoke = retriever.CreateAbility("Provoke");
            var ragingAxe = retriever.CreateAbility("Raging Axe");

            Assert.AreEqual(8, cure.MpCost);
            Assert.AreEqual(AbilityType.Jobability, provoke.AbilityType);
            Assert.AreEqual(AbilityType.Weaponskill, ragingAxe.AbilityType);
        }
    }
}