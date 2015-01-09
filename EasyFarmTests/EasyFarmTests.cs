
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
using EasyFarm.UserSettings;
using Moq;
using ZeroLimits.XITool.Classes;
using FFACETools;
using EasyFarm.FarmingTool;
using ZeroLimits.XITool.Test;
using EasyFarm.ViewModels;
using System.Collections.Generic;
using EasyFarm.GameData;
using ZeroLimits.FarmingTool;
using System.Text.RegularExpressions;

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

        [TestMethod]
        public void TestWeaponSkillTrigger()
        {
            WeaponSkill skill = new WeaponSkill()
            {
                Distance = 4,
                Enabled = true,
                LowerHealth = 25,
                UpperHealth = 75,
                Ability = new Ability() { Name = "Raging Axe" }
            };

            TestUnit unit = new TestUnit()
            {
                HPPCurrent = 50,
                Distance = 3
            };

            // Test for success
            Assert.IsTrue(ActionFilters.WeaponSkillFilter(null)(skill, unit));

            // Test for failure on low hp. 
            unit.HPPCurrent = 0;
            Assert.IsFalse(ActionFilters.WeaponSkillFilter(null)(skill, unit));

            // Test for failure on high hp. 
            unit.HPPCurrent = 100;
            Assert.IsFalse(ActionFilters.WeaponSkillFilter(null)(skill, unit));
        }
    }
}
