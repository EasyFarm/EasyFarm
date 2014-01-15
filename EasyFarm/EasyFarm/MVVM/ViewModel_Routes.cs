using EasyFarm.MVVM;
using EasyFarm.PathingTools;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using System.Linq;
using System.Windows.Data;

namespace EasyFarm
{
    partial class ViewModel
    {
        public ObservableCollection<FFACE.Position> Route
        {
            get { return Engine.Config.Waypoints; }
            set
            {
                Engine.Config.Waypoints = value;
            }
        }

        public ICommand RecordRouteCommand
        {
            get
            {
                return new RelayCommand(Action => RecordRoute(), Condition => { return true; });
            }
        }

        public ICommand ClearRouteCommand
        {
            get
            {
                return new RelayCommand(Action => ClearRoute(), Condition => { return true; });
            }
        }

        void ClearRoute()
        {
            Route = new ObservableCollection<FFACE.Position>();
        }

        void RecordRoute()
        {
            if (!WaypointRecorder.IsEnabled)
            {
                WaypointRecorder.Start();
                StatusBarText = "Recording!";
            }
            else
            {
                WaypointRecorder.Stop();
                StatusBarText = "Recording Stopped!";
            }
        }

        void RouteRecorder_Tick(object sender, EventArgs e)
        {
            Engine.GameState.Pathing.AddWaypoint();
        }
    }
}