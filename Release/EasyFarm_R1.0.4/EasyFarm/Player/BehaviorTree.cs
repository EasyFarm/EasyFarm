using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using EasyFarm.PathingTools;
using EasyFarm.UnitTools;
using FFACETools;
using TreeSharp;

namespace EasyFarm.PlayerTools
{
    class BehaviorTree
    {
        private FFACE Session;
        private Pathing Pathing;
        private Player Agent;
        private Units Units;

        IDisposable PathSubscription;
        IObservable<FFACE.Position> Path;

        public BehaviorTree(FFACE Session)
        {
            this.Session = Session;
            this.Pathing = new Pathing(Session);
            this.Agent = new Player(Session);
            this.Units = new Units(Session);
        }

        protected Composite Travel()
        { 
            return (
                new PrioritySelector(
                    // Ensure we have the path.
                    new Decorator(ret => (Path == null),
                        new TreeSharp.Action(delegate
                        {
                            Path = Pathing.GetRemainingPath().ToObservable();
                        })),

                    new Decorator(ret => (Session.Navigator.DistanceTo(Path.) > 3), 
                        new Sequence(
                            new TreeSharp.Action(delegate {
                                subscription = source.Subscribe(
                                    x => Session.Navigator.Goto(x, false)

                                Session.Navigator.Goto(Path.Peek(), true);
                            }),
                            new 
                )
             )
        }
    }
}
