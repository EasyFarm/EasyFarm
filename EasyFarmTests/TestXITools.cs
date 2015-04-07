
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

using EasyFarm.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parsing.Abilities;
using Parsing.Services;
using System;
using System.Diagnostics;

namespace XI_Tools_Tests
{
    [TestClass]
    public class TestXITools
    {
        [TestInitialize]
        public void SetUp()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        }

        [TestClass]
        public class AbilityTests
        {           
            [TestMethod]
            public void TestToString()
            {
                Ability test = new Ability();
                test.Prefix = "/magic";
                test.English = "Cure";
                test.Targets = "Self";
                var cure = new AbilityService("resources").CreateAbility("Cure");
                StringAssert.Equals(test.ToString(), cure.ToString());
            }
        }
    }
}