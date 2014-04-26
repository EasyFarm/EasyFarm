using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Faces heading toward a creature
    /// </summary>
    public class FaceMobBehavior : Behavior
    {
        private GameEngine _engine;

        public FaceMobBehavior(ref GameEngine engine)
            : base()
        {
            this._engine = engine;
        }

        public override TerminationStatus Execute()
        {
            _engine.Session.Instance.Navigator.FaceHeading(_engine.TargetData.Position);
            return TerminationStatus.Success;
        }

        public override bool CanExecute()
        {
            return _engine.TargetData.IsValid;
        }
    }
}
