
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
using EasyFarm.UserSettings;
using FFACETools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parsing.Abilities;
using System.Collections.Generic;

namespace EasyFarmTests
{
    [TestClass]
    public class EasyFarmTests
    {
        [TestMethod]
        public void TestUnitFilter()
        {
            // Reset config
            var conf = new Config();

            // Add point at (10, 10)
            Config.Instance.Waypoints.Add(new Waypoint() { X = 20, Z = 20 });

            TestUnit Unit = new TestUnit()
            {
                Status = Status.Standing,
                Distance = 0,
                IsActive = true,
                NPCType = NPCType.Mob,
                YDifference = 0,
                Name = "TestMob",
                ID = 777,
                HPPCurrent = 100,
                // New filtering code uses IsRendered vs
                // NPCBit. 
                IsRendered = true
            };

            // Generic filtering. 
            Assert.IsFalse(UnitFilters.MobFilter(null)(Unit));

            // Test waypoint wander distance. 
            Config.Instance.Waypoints.Add(new Waypoint() { X = 0, Z = 0 });

            Assert.IsTrue(UnitFilters.MobFilter(null)(Unit));

            // Test is Claimed Filter. 
            Unit.IsClaimed = true;
            Assert.IsFalse(UnitFilters.MobFilter(null)(Unit));

            // Test Claimed Filter 
            Config.Instance.ClaimedFilter = true;
            Assert.IsTrue(UnitFilters.MobFilter(null)(Unit));
        }
    }
}
