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
