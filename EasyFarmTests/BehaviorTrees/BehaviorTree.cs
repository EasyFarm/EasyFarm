using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EasyFarm.Tests.Behaviors;

namespace EasyFarm.Tests.BehaviorTrees
{
    public class BehaviorTree
    {
        public static List<BehaviorBase> BuildTree(params BehaviorTypes[] behaviorTypes)
        {
            return Assembly.GetAssembly(typeof(BehaviorTree)).GetTypes()
                .Where(x => CustomAttributeExtensions.GetCustomAttributes<BehaviorTypeAttribute>((MemberInfo) x)
                    .Any(behaviorAttribute => behaviorAttribute.HasBehaviors(behaviorTypes)))
                .Where(x => x.BaseType == typeof(BehaviorBase))
                .Select(Activator.CreateInstance)
                .Cast<BehaviorBase>()
                .SelectMany(BuildChildren)
                .ToList();
        }

        private static Stack<BehaviorBase> BuildChildren(BehaviorBase parent)
        {
            var behaviors = new Stack<BehaviorBase>();

            behaviors.Push(parent);

            var parentBehaviors = parent.GetType().GetCustomAttributes<BehaviorBase>().ToList();

            foreach (var child in parentBehaviors.Select(BuildChildren).SelectMany(children => children).Reverse())
            {
                behaviors.Push(child);
            }

            return behaviors;
        }
    }
}