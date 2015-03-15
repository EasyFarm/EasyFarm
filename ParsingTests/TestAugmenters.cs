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
                Path.Combine(Environment.CurrentDirectory, "Resources"));

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
