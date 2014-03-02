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
        DateTime runTime = DateTime.Now;

        public TravelState(ref GameEngine gameEngine) : base(ref gameEngine) { }

        public override bool CheckState()
        {
            return gameEngine.PlayerData.shouldTravel && gameEngine.IsWorking;
        }

        public override void EnterState()
        {
            gameEngine.Resting.Off();
            gameEngine.Combat.Disengage();

            gameEngine.FFInstance.Instance.Navigator.DistanceTolerance = 1;
            gameEngine.FFInstance.Instance.Navigator.HeadingTolerance = 1;
        }

        public override void RunState()
        {
            // If we've reached the end of the path....
            if (position >= gameEngine.Config.Waypoints.Count)
            {
                // Turn around and run the path in reverse with the old end being the new starting point
                gameEngine.Config.Waypoints = new ObservableCollection<FFACE.Position>(gameEngine.Config.Waypoints.Reverse());
                position = 0;
            }

            // If we are more than 10 yalms away from the nearest point...
            if (gameEngine.FFInstance.Instance.Navigator.DistanceTo(gameEngine.Config.Waypoints[position]) > 10)
            {
                // Find the closest point and ...
                FFACE.Position point = null;
                foreach (var pos in gameEngine.Config.Waypoints)
                {
                    if (point == null) { point = pos; }
                    
                    else if (gameEngine.FFInstance.Instance.Navigator.DistanceTo(pos) <
                        gameEngine.FFInstance.Instance.Navigator.DistanceTo(point))
                        point = pos;
                }

                // Get its index in the array of points, then ...
                if (point != null)
                {
                    position = gameEngine.Config.Waypoints.IndexOf(point);
                }
            }            

            // Run to the waypoint
            gameEngine.FFInstance.Instance.Navigator.Goto(gameEngine.Config.Waypoints[position], true, 10);
            // Set our position to the next point in the list
            position++;
        }

        public override void ExitState()
        {
            gameEngine.FFInstance.Instance.Navigator.Reset();
        }
    }
}
