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
        public List<FFACE.Position> Waypoints
        {
            get
            {
                if ( Session != null && _waypoints.Count > 0 && (DistanceToLastWaypoint() <= 5))
                    _waypoints = _waypoints.Reverse<FFACE.Position>().ToList();

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
        private double DistanceToLastWaypoint()
        {
            return Session.Navigator.DistanceTo(_waypoints.Last());
        }

        public double DistanceToFirstWaypoint()
        {
            return Session.Navigator.DistanceTo(_waypoints.First());
        }

        public void GotoWaypoint(FFACE.Position position)
        {
            Session.Navigator.Goto(position, false);
        }

        public void ClearWaypoints()
        {
            Waypoints.Clear();
        }

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