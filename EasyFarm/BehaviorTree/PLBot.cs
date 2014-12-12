using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.BehaviorTree
{
    [Behavior(BehaviorType.Standing, BehaviorType.Curing,
            BehaviorType.Buffing, BehaviorType.NonParty)]
    public class PLBot : TreeBase
    {
        public PLBot(double myValue, BehaviorType behaviorMask) :
            base(myValue, behaviorMask) { }

        public override void Execute(ref GameState WORLD)
        {
            if ((WORLD.ContinueMask & ContinueType.Finished) > 0) return;

            WORLD.Actors.Add(this);

            UnitService us = new UnitService(WORLD.FFACE);
            var ae = new AbilityExecutor(WORLD.FFACE);
            var @as = new AbilityService();

            if (!WORLD.FFACE.Player.KnowsSpell(SpellList.Cure))
            {
                MyBelief -= MyBelief <= 0.05 ? 0 : 0.05;
            }

            if (us.PCArray.Any(x => x.HPPCurrent <= 100))
            {
                var Target = us.PCArray
                    .OrderBy(x => x.HPPCurrent)
                    .FirstOrDefault();

                if (Target == null) return;

                if (ae.UseAbility(@as.CreateAbility("Cure"),
                    Constants.SPELL_CAST_LATENCY,
                    Constants.GLOBAL_SPELL_COOLDOWN))
                {
                    WORLD.ContinueMask |= ContinueType.Finished;
                    MyBelief += MyBelief >= 4.00 ? 0 : 0.05;
                }
                else 
                {
                    WORLD.ContinueMask |= ContinueType.KeepGoing;
                    MyBelief -= MyBelief <= 0.10 ? 0 : 0.01;
                }
            }
        }
    }
}
