using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq; // For lambda use.
using System.Reflection;
using FFACETools; // To be able to view inside ourselves.

namespace EasyFarm.BehaviorTree
{
    // Delegate: For getting messages out of any inherited class:
    public delegate void MessageDelegate(string one_message);
    // Delegate: For switching any class on or off in the Tree:
    public delegate void FilterDelegate(BehaviorType filter);

    // The beginning of all Behavior Tree classes:
    public abstract class TreeBase : IComparable<TreeBase>
    {
        // What are my important Behavior influences?
        public BehaviorType Influences { get; private set; }

        // How much do I think I should be running right now?
        public double MyBelief { get; set; }
        // ...and how much does my parent believe me?
        public double ParentBelief { get; set; }
        // ...and should I even be running at all?
        public bool Enabled = true;

        // List of any other classes I might like.
        public List<TreeBase> Children;

        // Constructor: How important am I to whoever created me?
        // What are my influences?
        public TreeBase(double MyValue, BehaviorType behaviormask)
        {
            ParentBelief = MyValue;
            Influences = behaviormask;
        }

        // An event for notifying any Children we have they may need to
        // turn themselves on or off.
        public event FilterDelegate OnFilter = delegate { };

        // And a function that matches FilterDelegate and can be called
        // from the OnFilter event:
        public void FilterSwitch(BehaviorType filter)
        {
            // Disable ourselves if none of our Influences match filter.
            Enabled = ((Influences & filter) > 0);
            // If we have Children listening let them know also.
            OnFilter(filter);
        }

        // Non-optional ability: Do something.
        public abstract void Execute(ref GameState WORLD);

        // Optional ability: Should I build a List of children classes?
        public virtual void BuildChildren(ref List<TreeBase> possibles, double Threshhold) { }

        // Sending debug messages or outputs.
        public event MessageDelegate OnMessage = delegate { };
        public virtual void Print(string message) { OnMessage(message); }

        // Comparing this TreeBase against another for importance.
        public int CompareTo(TreeBase other)
        {
            return -(MyBelief * ParentBelief)
                    .CompareTo(other.MyBelief * other.ParentBelief);
        }

        // Making a copy of this TreeBase
        public TreeBase Clone()
        {
            TreeBase copy = (TreeBase)this.MemberwiseClone();
            copy.MyBelief = MyBelief;
            copy.ParentBelief = ParentBelief;
            copy.Influences = Influences;
            return copy;
        }
    }
}
