using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using EasyFarm.Classes;
using System.Linq;
using System.Diagnostics;

namespace EasyFarmTests.MachineComponentTests
{
    [TestClass]
    public class MachineComponentTests
    {
        /// <summary>
        /// Verifies the logic behind the filtering in all 
        /// targeted and buffing components. The user's expect the 
        /// ordering to be maintained in each list. 
        /// </summary>
        [TestMethod]
        public void TestComponentAbilityFiltering()
        {
            // targeted test data
            var provoke = new TestBattleAbility("Provoke", true, false, false);
            var fire = new TestBattleAbility("Fire", true, false, false);

            // Buffs test data
            var protect = new TestBattleAbility("Protect", true, true, true);
            var shell = new TestBattleAbility("Shell", true, true, true);

            // Enabled test data
            var water = new TestBattleAbility("Water", false, false, false);

            var testData = new List<TestBattleAbility>()
            {
                provoke, 
                protect, 
                shell, 
                fire, 
                water
            };

            // Create the desired outcome with water removed since
            // it won't be enabled. 
            var desiredOutcome = testData.ToList();
            desiredOutcome.Remove(water);

            var ordering = testData
                .Where(x => x.Enabled)
                .Where(x => x.IsBuff && x.HasEffectWore || !x.IsBuff);

            Assert.IsTrue(Enumerable.SequenceEqual(ordering, desiredOutcome));
        }
    }
}