
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

using EasyFarm.UserSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarmTests
{
    [TestClass]
    public class ConfigTests
    {
        [TestInitialize]
        public void Setup()
        { 
            // Wipe all static changed data for 
            // every config test. 
            Config.Instance = new Config();
        }

        [TestMethod]
        public void TestConfigSingletonInstance()
        {
            var conf = Config.Instance;
            conf.FilterInfo.PartyFilter = true;
            Assert.AreEqual(conf.FilterInfo.PartyFilter, Config.Instance.FilterInfo.PartyFilter);
            Assert.AreSame(conf, Config.Instance);            
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
        }

        [TestMethod]
        public void TestConfigPersistanceWithGetInstance()
        {
            Config.Instance.FilterInfo.PartyFilter = true;
            Config.Instance.DebugEnabled = true;
            Config.Instance.SaveSettings();
            Config.Instance.LoadSettings();
            Assert.IsTrue(Config.Instance.FilterInfo.PartyFilter);
        }
    }
}
