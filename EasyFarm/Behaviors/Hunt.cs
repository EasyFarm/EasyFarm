using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentBehaviourTree;

namespace EasyFarm.Behaviors
{
    public class Hunt
    {
        private BehaviourTreeBuilder _behaviorTreeBuilder;
        private IBehaviourTreeNode _root;

        private int _target = -1;

        public Hunt()
        {
            _behaviorTreeBuilder = new BehaviourTreeBuilder();

            _root = _behaviorTreeBuilder
                .Sequence("Find and Navigate to Applicable Targets")
                    .Do("Find Target", t => FindTarget())
                    .Selector("Move to next Path Node")
                        .Condition("No valid mobs", t => _target == -1)
                        //.Do("Move to next Node", t => )
                    .Do("Run to Target", t => RunToTarget())
                .End()
                .Build();
        }


        private BehaviourTreeStatus FindTarget()
        {
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus RunToNextNode()
        {
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus RunToTarget()
        {
            return BehaviourTreeStatus.Success;
        }
    }
}
