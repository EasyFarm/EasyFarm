using EasyFarm.Engine;
using EasyFarm.PlayerTools;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace EasyFarm
{
    // Members and Constructors
    public partial class MainWindow : Window
    {
        private ViewModel Bindings;
        DispatcherTimer WaypointRecorder = new DispatcherTimer();
        FFACE.Position LastPosition = new FFACE.Position();

        public MainWindow(ref ViewModel Bindings)
        {
            this.Bindings = Bindings;
            WaypointRecorder.Tick += new EventHandler(RouteRecorder_Tick);
            WaypointRecorder.Interval = new TimeSpan(0, 0, 1);
            InitializeComponent();
        }

        public void ClearRoute(object s, EventArgs e)
        {
            Bindings.Engine.Config.Waypoints.Clear();
            lstWaypoints.Items.Clear();
        }

        void RecordRoute(object s, EventArgs e)
        {
            if (!WaypointRecorder.IsEnabled)
            {
                WaypointRecorder.Start();
                Bindings.StatusBarText = "Recording!";
            }
            else
            {
                WaypointRecorder.Stop();
                Bindings.StatusBarText = "Recording Stopped!";
            }
        }

        void RouteRecorder_Tick(object sender, EventArgs e)
        {
            var Point = Bindings.Engine.FFInstance.Instance.Player.Position;
            if (!Point.Equals(LastPosition))
            {
                Bindings.Engine.Config.Waypoints.Add(Point);
                lstWaypoints.Items.Add("X: " + Point.X + " Z: " + Point.Z);
                LastPosition = Point;
            }
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            foreach (var w in Bindings.Engine.Config.Waypoints)
            {
                lstWaypoints.Items.Add("X: " + w.X + "Z: " + w.Z);
            }
        }
    }
}
