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
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parsing.Abilities;
using Parsing.Services;
using Parsing.Types;

namespace ParsingTests
{
    [TestClass]
    public class TestAugmenters
    {
        private static readonly AbilityService Retriever = new AbilityService(
            Path.Combine(Environment.CurrentDirectory, "resources"));

        // Test create ability for all major skill types. 
        private readonly Ability _cure = Retriever.CreateAbility("Cure");
        private readonly Ability _provoke = Retriever.CreateAbility("Provoke");
        private readonly Ability _ragingAxe = Retriever.CreateAbility("Raging Axe");

        /// <summary>
        ///     Test to see if the category type was properly processed.
        /// </summary>
        [TestMethod]
        public void TestCategoryTypeAugmenter()
        {
            Assert.AreEqual(CategoryType.WhiteMagic, _cure.CategoryType);
            Assert.AreEqual(CategoryType.JobAbility, _provoke.CategoryType);
            Assert.AreEqual(CategoryType.WeaponSkill, _ragingAxe.CategoryType);
        }

        /// <summary>
        ///     Test to see if the element type was properly processed.
        /// </summary>
        [TestMethod]
        public void TestElementTypeAugmenter()
        {
            Assert.AreEqual(ElementType.Light, _cure.ElementType);
            Assert.AreEqual(ElementType.None, _provoke.ElementType);
            Assert.AreEqual(ElementType.None, _ragingAxe.ElementType);
        }

        /// <summary>
        ///     Test to see if the skill type was properly retrieved.
        /// </summary>
        [TestMethod]
        public void TestSkillTypeAugmenter()
        {
            Assert.AreEqual(SkillType.HealingMagic, _cure.SkillType);
            Assert.AreEqual(SkillType.Ability, _provoke.SkillType);
            Assert.AreEqual(SkillType.Ability, _ragingAxe.SkillType);
        }

        /// <summary>
        ///     Check if the TargetType field was properly processed.
        /// </summary>
        [TestMethod]
        public void TestTargetTypeAugmenter()
        {
            // Refactor borked test... 
            Assert.IsTrue(_cure.TargetType.HasFlag(TargetType.Self));
            Assert.IsTrue(_provoke.TargetType.HasFlag(TargetType.Enemy));
            Assert.IsTrue(_ragingAxe.TargetType.HasFlag(TargetType.Enemy));
        }
    }
}