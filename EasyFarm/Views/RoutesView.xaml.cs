
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EasyFarm.Views
{
    /// <summary>
    /// Interaction logic for RoutesView.xaml
    /// </summary>
    public partial class RoutesView : UserControl
    {
        DispatcherTimer WaypointRecorder = new DispatcherTimer();
        FFACE.Position LastPosition = new FFACE.Position();
        ViewModel Bindings;

        public RoutesView()
        {
            WaypointRecorder.Tick += new EventHandler(RouteRecorder_Tick);
            WaypointRecorder.Interval = new TimeSpan(0, 0, 1);
            this.Loaded += RoutesView_Loaded;
            InitializeComponent();
        }

        void RoutesView_Loaded(object sender, RoutedEventArgs e)
        {
            // Perform on load once so that we do not add duplicate waypoint entries into the waypoint list. 
            if (Bindings != null) return;

            this.Bindings = this.DataContext as ViewModel;

            foreach (var w in Bindings.Engine.Config.Waypoints)
            {
                lstWaypoints.Items.Add("X: " + w.X + "Z: " + w.Z);
            }
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
    }
}
