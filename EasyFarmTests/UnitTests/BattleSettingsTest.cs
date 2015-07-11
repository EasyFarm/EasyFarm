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
using EasyFarm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyFarm.Tests.UnitTests
{
    [TestClass]
    public class BattleSettingsTest
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
            new BattleSettingsViewModel {ShouldApproach = false};

            // Set approach to false in the vm which should also set it
            // to false in the config. 

            // Assert it is indeed false. 
            Assert.IsFalse(Config.Instance.IsApproachEnabled);
        }

        [TestMethod]
        public void EngageReflectedInConfigTest()
        {
            // Create the view model. 
            new BattleSettingsViewModel {ShouldEngage = false};

            // Set approach to false in the vm which should also set it
            // to false in the config. 

            // Assert it is indeed false. 
            Assert.IsFalse(Config.Instance.IsEngageEnabled);
        }
    }
}