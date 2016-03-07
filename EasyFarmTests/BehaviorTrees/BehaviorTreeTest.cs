using System.Collections.Generic;
using EasyFarm.Tests.Behaviors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyFarm.Tests.BehaviorTrees
{
    [TestClass]
    public class BehaviorTreeTest
    {
        [TestClass]
        public class BuildTree
        {
            [TestMethod]
            public void BehaviorsInCorrectOrder()
            {
                var behaviors = BehaviorTree.BuildTree(BehaviorTypes.Battle);

                var expectedTypes = new List<object>()
                {
                    typeof (StandUp),
                    typeof (Approach),
                    typeof (StandUp),
                    typeof (SpotTarget),
                    typeof (TargetEnemy),
                    typeof (Engage),
                    typeof (BattleBehavior)
                };

                CustomAssertions.AssertTypesEqual(expectedTypes, behaviors);
            }

            [TestMethod]
            public void BuildsTreeWithOnetype()
            {
                var behaviors = BehaviorTree.BuildTree(BehaviorTypes.Healing);
                Assert.AreEqual(1, behaviors.Count);
            }

            [TestMethod]
            public void BuildsTreeWithTwoTypes()
            {
                var behaviors = BehaviorTree.BuildTree(BehaviorTypes.Healing, BehaviorTypes.Buffing);
                Assert.AreEqual(2, behaviors.Count);
            }
        }
    }
}