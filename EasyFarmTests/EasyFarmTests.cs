using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyFarm.UserSettings;
using Moq;
using ZeroLimits.XITool.Classes;
using FFACETools;
using EasyFarm.FarmingTool;
using ZeroLimits.XITool.Test;

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
        }

        [TestMethod]
        public void TestConfigPersistence()
        {
            var conf = Config.Instance;
            conf.FilterInfo.PartyFilter = false;
            conf.SaveSettings();
            conf.LoadSettings();
            Assert.AreEqual(conf.FilterInfo.PartyFilter, false);
        }

        [TestMethod]
        public void TestConfigPersistanceWithGetInstance()
        {
            Config.Instance.FilterInfo.PartyFilter = true;
            Config.Instance.SaveSettings();
            Config.Instance.LoadSettings();
            Assert.AreEqual(Config.Instance.FilterInfo.PartyFilter, true);
        }

        [TestMethod]
        public void TestUnitFilter()
        {
            TestUnit Unit = new TestUnit()
            {
                Status = Status.Dead1
            };

            FFACE FFACE = (new Mock<FFACE>(null).Object) as FFACE;

            Assert.AreEqual(UnitFilters.MobFilter(FFACE)(Unit), true);
        }
    }
}
