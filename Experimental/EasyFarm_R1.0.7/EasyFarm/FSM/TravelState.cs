using System.Linq;
using EasyFarm.Engine;
using System.Collections.ObjectModel;
using FFACETools;
using EasyFarm.UtilityTools;
using System;

namespace EasyFarm.FSM
{
    class TravelState : BaseState
    {
        int position = 0;

        public TravelState(ref GameEngine gameEngine) : base(ref gameEngine) { }

        public override bool CheckState()
        {
            return gameEngine.Player.shouldTravel && gameEngine.IsWorking;
        }

        public override void EnterState()
        {
            gameEngine.Player.RestingOff();
            gameEngine.Player.Disengage();

            gameEngine.FFInstance.Instance.Navigator.DistanceTolerance = 1;
            gameEngine.FFInstance.Instance.Navigator.HeadingTolerance = 1;
        }

        public override void RunState()
        {
            if (position >= gameEngine.Config.Waypoints.Count)
            {
                gameEngine.Config.Waypoints = new ObservableCollection<FFACE.Position>(gameEngine.Config.Waypoints.Reverse());
                position = 0;
            }

            if (gameEngine.FFInstance.Instance.Navigator.DistanceTo(gameEngine.Config.Waypoints[position]) > 10)
            {
                FFACE.Position point = null;

                foreach (var pos in gameEngine.Config.Waypoints)
                {
                    if (point == null) { point = pos; }
                    
                    else if (gameEngine.FFInstance.Instance.Navigator.DistanceTo(pos) <
                        gameEngine.FFInstance.Instance.Navigator.DistanceTo(point))
                        point = pos;
                }

                if (point != null)
                {
                    position = gameEngine.Config.Waypoints.IndexOf(point);
                }
            }

            gameEngine.Pathing.GotoWaypoint(gameEngine.Config.Waypoints[position]);
            position++;
        }

        public override void ExitState()
        {
            gameEngine.FFInstance.Instance.Navigator.Reset();
        }
    }
}
