#define Debug

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.Engine;
using FFACETools;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EasyFarm.FSM
{
    class TravelState : BaseState
    {
        int position = 0;

        public TravelState(ref GameState GameState) : base(ref GameState) { }

        public override bool CheckState()
        {
            return GameState.IsTraveling;
        }

        public override void EnterState()
        {
            if (GameState.Player.IsResting())
            {
                GameState.Player.RestingOff();
            }

            if (GameState.Player.IsEngaged())
            {
                GameState.Player.Disengage();
            }

            GameState.FFInstance.Instance.Navigator.DistanceTolerance = 1;
            GameState.FFInstance.Instance.Navigator.HeadingTolerance = 1;
        }

        public override void RunState()
        {
            if (position >= GameState.Config.Waypoints.Length)
            {
                GameState.Config.Waypoints = GameState.Config.Waypoints.Reverse().ToArray();
                position = 0;
            }

            GameState.Pathing.GotoWaypoint(GameState.Config.Waypoints[position]);
            position++;
        }

        public override void ExitState()
        {
            // Stop us from moving
            GameState.FFInstance.Instance.Navigator.Reset();
        }
    }
}
