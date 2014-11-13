
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

namespace EasyFarmTests
{
    [TestClass]
    public class EasyFarmTests
    {
        [TestMethod]
        public void TestConfigSingletonInstance()
        {
            var conf = Config.Instance;
            conf.FilterInfo.PartyFilter = true;
            Assert.AreEqual(conf.FilterInfo.PartyFilter, Config.Instance.FilterInfo.PartyFilter);
            Assert.AreSame(conf, Config.Instance);

            // Wipe all static changed data. 
            conf = new Config();
        }

        [TestMethod]
        public void TestConfigPersistence()
        {
            var conf = new Config();
            conf.DebugEnabled = true;
            conf.FilterInfo.PartyFilter = false;
            conf.SaveSettings();
            conf.LoadSettings();
            Assert.IsFalse(conf.FilterInfo.PartyFilter);

            // Wipe all static changed data. 
            conf = new Config();
        }

        [TestMethod]
        public void TestConfigPersistanceWithGetInstance()
        {
            Config.Instance.FilterInfo.PartyFilter = true;
            Config.Instance.DebugEnabled = true;
            Config.Instance.SaveSettings();
            Config.Instance.LoadSettings();
            Assert.IsTrue(Config.Instance.FilterInfo.PartyFilter);

            // Wipe all static changed data. 
            Config.Instance = new Config();
        }

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
            Config.Instance.FilterInfo.ClaimedFilter = true;
            Assert.IsTrue(UnitFilters.MobFilter(null)(Unit));            
        }
    }
}
