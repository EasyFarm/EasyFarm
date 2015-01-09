using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyFarm.UserSettings;
using EasyFarm.Views.Settings;

namespace EasyFarmTests
{
    [TestClass]
    public class BattleSettingsTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Wipe all static changed data for 
            // every config test. 
            Config.Instance = new Config();
        }

        [TestMethod]
        public void ApproachReflectedInConfigTest()
        {
            // Create the view model. 
            BattleSettingsViewModel BattleSettingsVM = 
                new BattleSettingsViewModel();

            // Set approach to false in the vm which should also set it
            // to false in the config. 
            BattleSettingsVM.ShouldApproach = false;

            // Assert it is indeed false. 
            Assert.IsFalse(Config.Instance.IsApproachEnabled);
        }

        [TestMethod]
        public void EngageReflectedInConfigTest()
        {
            // Create the view model. 
            BattleSettingsViewModel BattleSettingsVM =
                new BattleSettingsViewModel();

            // Set approach to false in the vm which should also set it
            // to false in the config. 
            BattleSettingsVM.ShouldEngage = false;

            // Assert it is indeed false. 
            Assert.IsFalse(Config.Instance.IsEngageEnabled);
        }
    }
}
