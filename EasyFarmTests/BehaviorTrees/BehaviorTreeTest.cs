using System.Collections.Generic;
using EasyFarm.Tests.Behaviors;
using NUnit.Framework;

namespace EasyFarm.Tests.BehaviorTrees
{
    [TestFixture]
    public class BehaviorTreeTest
    {
        [TestFixture]
        public class BuildTree
        {
            [Test]
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

            [Test]
            public void BuildsTreeWithOnetype()
            {
                var behaviors = BehaviorTree.BuildTree(BehaviorTypes.Healing);
                Assert.AreEqual(1, behaviors.Count);
            }

            [Test]
            public void BuildsTreeWithTwoTypes()
            {
                var behaviors = BehaviorTree.BuildTree(BehaviorTypes.Healing, BehaviorTypes.Buffing);
                Assert.AreEqual(2, behaviors.Count);
            }
        }
    }
}