using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.BehaviorTree
{
    public class MaxAttack : TreeBase
    {
        public MaxAttack(double myValue, BehaviorType behaviorMask)
            : base(myValue, behaviorMask)
        {
            Children = new List<TreeBase>();
        }

        public override void Execute(ref GameState WORLD)
        {
            if ((WORLD.ContinueMask & ContinueType.Finished) > 0) return;

            WORLD.Actors.Add(this);

            Children.Sort();

            foreach (var TB in Children)
            {
                if (TB.Enabled == false) { continue; }
                TB.Execute(ref WORLD);
            }

            if ((WORLD.ContinueMask & ContinueType.Finished) > 0)
            {
                // My Children did a good job so I must be a good parent.
                MyBelief += MyBelief > 4.0 ? 0 : 0.10;
            }
            // ...or maybe I am a bad parent after all...
            else { MyBelief -= MyBelief < 0.05 ? 0 : 0.05; }
        }

        public override void BuildChildren(ref List<TreeBase> possibles, double Threshhold)
        {
            // amount every time it gets called until there are no more matches.
            double ChildMatchCount, ParentMatchCount;
            BehaviorType ParentTraits = Influences;
            foreach (TreeBase TB in possibles)
            {
                // We need to NOT count the "Branch" Influence or Branches will
                // absolutely start loving each other and keep adding themselves.
                BehaviorType ChildTraits = TB.Influences;
                if ((ParentTraits & BehaviorType.Branch) > 0)
                {
                    ParentTraits &= BehaviorType.Branch;
                }

                if ((ChildTraits & BehaviorType.Branch) > 0)
                {
                    ChildTraits &= BehaviorType.Branch;
                }

                // Count how many Influences we share with this candidate.
                // Notice the <<= 1 in the loop? Every time the loop goes
                // around this shifts the value one bit higher:
                // 0 = 0, 1 = 1, 10 = 2, 100 = 4, 1000 = 8, 10000 = 16...
                // This matches our bit values in the BehaviorTypes enum.
                ChildMatchCount = ParentMatchCount = 0;
                for (uint test = 0; test <= uint.MaxValue; test <<= 1)
                {
                    if ((ParentTraits & (BehaviorType)test) > 0)
                    {
                        ParentMatchCount++; // Found something the Parent has.
                        // Does the candidate Child have it also?
                        if ((ChildTraits & (BehaviorType)test) > 0)
                        {
                            ChildMatchCount++;
                        }
                    }
                }

                // Now does the possible child share enough common Influences to
                // qualify to be added..? We ignore that they both might be Branches.
                if (ChildMatchCount / ParentMatchCount < Threshhold)
                {
                    continue;
                }

                // Yes it does. Make a _copy_ of the list's item.
                TreeBase AddMe = TB.Clone();
                AddMe.ParentBelief = ChildMatchCount / ParentMatchCount;
                OnFilter += AddMe.FilterSwitch; // Child listens for filters.
                AddMe.OnMessage += Print; // And we listen for Child messages.
                Children.Add(AddMe);

                // Recursion: Is this child ALSO a Branch? If you prefer in your
                // version of the Branch class you could skip this sort of deep
                // copying. That would result in very "shallow", easy to predict
                // bot decisions since the Tree doesn't go very far.
                if ((TB.Influences & BehaviorType.Branch) > 0)
                {
                    // This Child needs its own Children. In this Branch
                    // class implementation each recursion raises the
                    // Threshhold 5% until it can't possibly be met.
                    TB.BuildChildren(ref possibles, Threshhold + 0.05);
                }
            }
        }
    }
}
