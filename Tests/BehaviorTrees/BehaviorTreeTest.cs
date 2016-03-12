using System.Collections.Generic;
using EasyFarm.Tests.Behaviors;
using Xunit;

namespace EasyFarm.Tests.BehaviorTrees
{
    public class BehaviorTreeTest
    {
        public class BuildTree
        {
            [Fact]
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

            [Fact]
            public void BuildsTreeWithOnetype()
            {
                var behaviors = BehaviorTree.BuildTree(BehaviorTypes.Healing);
                Assert.Equal(1, behaviors.Count);
            }

            [Fact]
            public void BuildsTreeWithTwoTypes()
            {
                var behaviors = BehaviorTree.BuildTree(BehaviorTypes.Healing, BehaviorTypes.Buffing);
                Assert.Equal(2, behaviors.Count);
            }
        }
    }
}