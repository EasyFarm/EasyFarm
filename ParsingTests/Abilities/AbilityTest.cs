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
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parsing.Abilities;
using Parsing.Services;
using Parsing.Types;

namespace EasyFarm.ParsingTests.Abilities
{
    public class AbilityTest
    {
        [TestClass]
        public class ToString : AbilityTest
        {
            [TestMethod]
            public void TestToString()
            {
                var test = new Ability
                {
                    Prefix = "/magic",
                    English = "Cure",
                    TargetType = TargetType.Self
                };

                var expectedCommand = "/magic \"Cure\" <me>";
                Assert.AreEqual(expectedCommand, test.ToString());
            }
        }
    }
}