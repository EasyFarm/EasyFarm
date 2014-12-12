using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.BehaviorTree
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BehaviorAttribute : System.Attribute
    {
        public BehaviorType Influences;
        public BehaviorAttribute(params BehaviorType[] categories)
        {
            foreach (BehaviorType BT in categories) { Influences |= BT; }
        }

        // Check if an applied class exhibits a certain behavior.
        public bool CheckInfluence(BehaviorType check)
        {
            return ((Influences & check) == check);
        }
    }

    [Flags] // Avoids a lot of (uint) nonsense.
    public enum BehaviorType : uint
    {
        None = 0,
        Branch = 1, // This class manages sub-classes.
        Any = 2, // Always matches at least once.
        Standing = 4,
        Tank = 8,
        LowHP = 16,
        Suicidal = 32,
        Chatty = 64,
        Curing = 128,
        Buffing = 256,
        NonParty = 512,

        // Notice how common subtypes can be created in one value:
        Idle = Any | Chatty
    }    
}
