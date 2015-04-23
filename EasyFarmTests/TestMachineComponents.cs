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

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyFarmTests.MachineComponentTests
{
    [TestClass]
    public class TestMachineComponents
    {
        /// <summary>
        ///     Verifies the logic behind the filtering in all
        ///     targeted and buffing components. The user's expect the
        ///     ordering to be maintained in each list.
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

            var testData = new List<TestBattleAbility>
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

            Assert.IsTrue(ordering.SequenceEqual(desiredOutcome));
        }
    }
}