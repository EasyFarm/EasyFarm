using System;
using System.Linq;

namespace EasyFarm.Tests.BehaviorTrees
{
    public class BehaviorTypeAttribute : Attribute
    {
        private readonly BehaviorTypes[] _behaviors;

        public BehaviorTypeAttribute(params BehaviorTypes[] behaviors)
        {
            _behaviors = behaviors;
        }

        public bool HasBehaviors(params BehaviorTypes[] behaviors)
        {
            return _behaviors.Any(behaviors.Contains);
        }
    }
}