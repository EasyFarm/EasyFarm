
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
using Parsing.Augmenting;
using Parsing.Services;
using System.IO;
using Parsing.Types;
using Parsing.Abilities;

namespace ParsingTests
{
    [TestClass]
    public class TestAugmenters
    {
        static AbilityService Retriever = new AbilityService(
                Path.Combine(Environment.CurrentDirectory, "resources"));

        // Test create ability for all major skill types. 
        Ability cure = Retriever.CreateAbility("Cure");
        Ability raise = Retriever.CreateAbility("Raise");
        Ability provoke = Retriever.CreateAbility("Provoke");
        Ability ragingAxe = Retriever.CreateAbility("Raging Axe");

        [TestMethod]
        public void TestCategoryTypeAugmenter()
        {
            Assert.AreEqual(CategoryType.WhiteMagic, cure.CategoryType);
            Assert.AreEqual(CategoryType.JobAbility, provoke.CategoryType);
            Assert.AreEqual(CategoryType.WeaponSkill, ragingAxe.CategoryType);
        }

        [TestMethod]
        public void TestElementTypeAugmenter()
        {
            Assert.AreEqual(ElementType.Light, cure.ElementType);
            Assert.AreEqual(ElementType.None, provoke.ElementType);
            Assert.AreEqual(ElementType.None, ragingAxe.ElementType);
        }

        [TestMethod]
        public void TestSkillTypeAugmenter()
        {
            Assert.AreEqual(SkillType.HealingMagic, cure.SkillType);
            Assert.AreEqual(SkillType.Ability, provoke.SkillType);
            Assert.AreEqual(SkillType.Ability, ragingAxe.SkillType);
        }

        [TestMethod]
        public void TestTargetTypeAugmenter()
        {
            Assert.IsTrue(cure.TargetType.HasFlag(TargetType.Player));
            Assert.IsTrue(provoke.TargetType.HasFlag(TargetType.Enemy));
            Assert.IsTrue(ragingAxe.TargetType.HasFlag(TargetType.Enemy));
            Assert.IsTrue(raise.TargetType.HasFlag(TargetType.Corpse));
        }
    }
}
