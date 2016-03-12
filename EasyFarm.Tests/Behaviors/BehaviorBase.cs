using System;
using EasyFarm.Tests.BehaviorTrees;

namespace EasyFarm.Tests.Behaviors
{
    public class BehaviorBase : Attribute, IBehavior
    {
        public virtual bool Check() => true;

        public virtual void Run() { }
    }
}