using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFACETools;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace EasyFarm.PathingTools
{
    [Serializable]
    public class Pathing
    {
        #region Members
        List<FFACE.Position> _waypoints = new List<FFACE.Position>();
        [NonSerialized]
        FFACE Session;
        [NonSerialized]
        DispatcherTimer PathMonitor = new DispatcherTimer();
        [NonSerialized]
        FFACE.Position LastPosition = new FFACE.Position();
        #endregion

        #region Properties
        /// <summary>
        /// Returns the unaltered waypoint list.
        /// </summary>
        public List<FFACE.Position> Waypoints
        {
            get
            {
                return _waypoints;
            }
            set
            {
                if (Waypoints != null)
                    _waypoints = value;
            }
        } 
        #endregion

        #region Contructors
        private Pathing()
        {

        }

        public Pathing(FFACE session)
        {
            Session = session;
            Waypoints = new List<FFACE.Position>();           
            Session.Navigator.HeadingTolerance = 1;
            Session.Navigator.DistanceTolerance = 1;
        } 
        #endregion

        #region Functions
        /// <summary>
        /// Returns the nearest point to us on the waypoint
        /// route. It may return null if out of range.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public FFACE.Position GetNearestPoint(List<FFACE.Position> list)
        {
            // Grab part of route that is within 10 yalms.
            var Route = new List<FFACE.Position>();
            Route.AddRange(list.Where(element => Session.Navigator.DistanceTo(element) < 50));

            // Grab the closest point that is in that path.
            var NearestPoint = (from element in Route
                                orderby Session.Navigator.DistanceTo(element)
                                select element).FirstOrDefault();

            return NearestPoint;
        }

        /// <summary>
        /// Returns the path for the player to run.
        /// It may be reversed depending on if the player
        /// is nearing the end of the path.
        /// 
        /// Alters the internal list of waypoints!
        /// </summary>
        /// <returns></returns>
        public List<FFACE.Position> GetPath()
        {
            if (_waypoints.Count > 0 && (DistanceToLastWaypoint() <= 5))
                _waypoints = _waypoints.Reverse<FFACE.Position>().ToList();

            return _waypoints;
        }

        /// <summary>
        /// Calculates distance to last waypoint.
        /// </summary>
        /// <returns></returns>
        private double DistanceToLastWaypoint()
        {
            double distance = Session.Navigator.DistanceTo(_waypoints.Last());
            return distance;
        }

        /// <summary>
        /// Calculates the distance to the first waypoint.
        /// </summary>
        /// <returns></returns>
        public double DistanceToFirstWaypoint()
        {
            return Session.Navigator.DistanceTo(_waypoints.First());
        }

        /// <summary>
        /// Moves player to specified waypoint
        /// </summary>
        /// <param name="position"></param>
        public void GotoWaypoint(FFACE.Position position)
        {
            Session.Navigator.Goto(position, false);
        }

        /// <summary>
        /// Clears all waypoints
        /// </summary>
        public void ClearWaypoints()
        {
            Waypoints.Clear();
        }

        /// <summary>
        /// Adds a waypoint to the current
        /// waypoint list.
        /// </summary>
        public void AddWaypoint()
        {
            var CurrentPosition = Session.Player.Position;

            if (!CurrentPosition.Equals(LastPosition))
            {
                Waypoints.Add(CurrentPosition);
                LastPosition = CurrentPosition;
            }
        }
        #endregion                           
    }
}