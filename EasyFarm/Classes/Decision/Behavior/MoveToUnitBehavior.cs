using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Moves to a creature.
    /// </summary>
    public class MoveToUnitBehavior : Behavior
    {
        private GameEngine _engine;

        const int DIST_MIN = 3;

        public MoveToUnitBehavior(ref GameEngine engine)
            : base()
        {
            this._engine = engine;
        }

        public override TerminationStatus Execute()
        {
            // Save the old tolerance
            var OldTolerance = _engine.Session.Instance.Navigator.DistanceTolerance;

            // Use the new one
            _engine.Session.Instance.Navigator.DistanceTolerance = DIST_MIN;

            // Got to the npc
            _engine.Session.Instance.Navigator.GotoNPC(_engine.TargetData.TargetUnit.ID, 10);

            // Restore the old tolerance.
            _engine.Session.Instance.Navigator.DistanceTolerance = OldTolerance;

            return TerminationStatus.Success;
        }

        public override bool CanExecute()
        {
            // Run to the unit while we are out of distance. 
            return _engine.Session.Instance.Navigator
                .DistanceTo(_engine.TargetData.Position) >= DIST_MIN;
        }
    }
}
