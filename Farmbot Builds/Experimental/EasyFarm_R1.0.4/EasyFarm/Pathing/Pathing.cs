using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using System.Xml.Serialization;
using EasyFarm.Engine;
using FFACETools;
using System.Diagnostics;
using System.Collections;

namespace EasyFarm.PathingTools
{
    public class Pathing
    {
        #region Members
        static DispatcherTimer PathMonitor = null;
        static FFACE.Position LastPosition = null;
        static GameState GameState = null;
        #endregion

        #region Contructors
        private Pathing()
        {
            PathMonitor = new DispatcherTimer();
            LastPosition = new FFACE.Position();
        }

        public Pathing(ref GameState State)
            : this()
        {
            GameState = State;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Returns the nearest point to us on the waypoint
        /// route. It may return null if out of range.
        /// </summary>
        /// <param name="Route"></param>
        /// <returns></returns>
        public FFACE.Position GetNearestPoint(FFACE.Position[] Route)
        {
            return Route.Where(x => GameState.FFInstance.Instance.Navigator.DistanceTo(x) < 50).Min();
        }

        /// <summary>
        /// Returns the path for the player to run.
        /// It may be reversed depending on if the player
        /// is nearing the end of the path.
        /// 
        /// Alters the internal list of waypoints!
        /// </summary>
        /// <returns></returns>
        public FFACE.Position[] GetPath()
        {
            return GetPath(GameState.Config.Waypoints);
        }

        /// <summary>
        /// Returns the path for the player to run.
        /// It may be reversed depending on if the player
        /// is nearing the end of the path.
        /// 
        /// Alters the internal list of waypoints!
        /// </summary>
        /// <returns></returns>
        public FFACE.Position[] GetPath(FFACE.Position[] Route)
        {
            if (Route.Length <= 0)
            {
                return Route;
            }

            if (GameState.FFInstance.Instance.Navigator.DistanceTo(Route.Last()) < 5)
            {
                return Route.Reverse().ToArray();
            }

            return Route;
        }

        /// <summary>
        /// Moves player to specified waypoint
        /// </summary>
        /// <param name="position"></param>
        public void GotoWaypoint(FFACE.Position position)
        {
            GameState.FFInstance.Instance.Navigator.Goto(position, false);
        }

        /// <summary>
        /// Clears all waypoints
        /// </summary>
        public void ClearWaypoints()
        {
            GameState.Config.Waypoints = new FFACE.Position[0];
        }

        /// <summary>
        /// Adds a waypoint to the current
        /// waypoint list.
        /// </summary>
        public void AddWaypoint()
        {
            FFACE.Position Current = GameState.FFInstance.Instance.Player.Position;

            if (!LastPosition.Equals(Current))
            {
                // Copy and resize array
                FFACE.Position[] Waypoints = new FFACE.Position[GameState.Config.Waypoints.Length + 1];
                GameState.Config.Waypoints.CopyTo(Waypoints, 0);
                GameState.Config.Waypoints = Waypoints;

                // Add new waypoint at end
                GameState.Config.Waypoints[Waypoints.Length - 1] = Current;
            }
        }
        #endregion

        public FFACE.Position[] GetRemainingPath(FFACE.Position[] Route)
        {
            FFACE.Position NearestPoint = GetNearestPoint(Route);

            if (NearestPoint == null)
            {
                return new FFACE.Position[0];
            }

            return GameState.Config.Waypoints.SkipWhile(element => element != NearestPoint).ToArray();
        }

        public FFACE.Position[] GetRemainingPath()
        {
            return GetRemainingPath(GetPath());
        }

        public double Distance(FFACE.Position A, FFACE.Position B)
        {
            double Distance =
                Math.Sqrt(
                Math.Pow((A.X - B.X), 2) +
                Math.Pow((A.Z - B.Z), 2)
            );

            return Distance;
        }        
    }
}