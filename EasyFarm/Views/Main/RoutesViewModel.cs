
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

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

using EasyFarm.Classes;
using EasyFarm.UserSettings;
using FFACETools;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Routes")]
    public class RoutesViewModel : ViewModelBase
    {
        /// <summary>
        /// Recorder to record new waypoints into our path. 
        /// </summary>
        private readonly DispatcherTimer _waypointRecorder;

        /// <summary>
        /// Used by the recorder to avoid duplicate, successive waypoints.
        /// (Identicle waypoints are allowed, just not in succession.)
        /// </summary>
        private FFACE.Position _lastPosition = new FFACE.Position();

        /// <summary>
        /// Saves the route to file on user request. 
        /// </summary>
        private SettingsManager _settingsManager;

        public RoutesViewModel()
        {
            _waypointRecorder = new DispatcherTimer();
            _waypointRecorder.Tick += RouteRecorder_Tick;
            _waypointRecorder.Interval = new TimeSpan(0, 0, 1);

            ClearRouteCommand = new DelegateCommand(ClearRoute);
            RecordRouteCommand = new DelegateCommand<Object>(RecordRoute);
            SaveCommand = new DelegateCommand(SaveRoute);
            LoadCommand = new DelegateCommand(LoadRoute);

            _settingsManager = new SettingsManager(
                "ewl",
                "EasyFarm Waypoint List"
            );
        }

        /// <summary>
        /// Exposes the list of waypoints to the user. 
        /// </summary>
        public ObservableCollection<Waypoint> Route
        {
            get { return Config.Instance.Waypoints; }
            set { SetProperty(ref Config.Instance.Waypoints, value); }
        }

        /// <summary>
        /// Binding for the record command for the GUI.
        /// </summary>
        public ICommand RecordRouteCommand { get; set; }

        /// <summary>
        /// Binding for the clear command for the GUI. 
        /// </summary>
        public ICommand ClearRouteCommand { get; set; }

        /// <summary>
        /// Binding for the save command for the GUI. 
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Binding for the load command for the GUI. 
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// Clears the waypoint list. 
        /// </summary>
        public void ClearRoute()
        {
            Route.Clear();
        }

        /// <summary>
        /// Pauses and resumes the path recorder based on
        /// its current state. 
        /// </summary>
        /// <param name="recordButton"></param>
        public void RecordRoute(Object recordButton)
        {
            if (!_waypointRecorder.IsEnabled)
            {
                _waypointRecorder.Start();
                var button = recordButton as Button;
                if (button != null)
                {
                    button.Content = "Recording!";
                }
            }
            else
            {
                _waypointRecorder.Stop();
                var button = recordButton as Button;
                if (button != null)
                {
                    button.Content = "Record";
                }
            }
        }

        /// <summary>
        /// Saves the route data. 
        /// </summary>
        private void SaveRoute()
        {
            _settingsManager.Save<ObservableCollection<Waypoint>>(Route);
        }

        /// <summary>
        /// Loads the route data. 
        /// </summary>
        private void LoadRoute()
        {
            var path = _settingsManager.Load<ObservableCollection<Waypoint>>();
            
            if (path == null)
            {
                ViewModelBase.InformUser("Path could not be loaded. ");
                return;
            }

            Route = path;
        }

        /// <summary>
        /// Records waypoints to the waypoint path. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouteRecorder_Tick(object sender, EventArgs e)
        {
            // Add a new waypoint only when we are not standing at 
            // our last position. 
            var point = FFACE.Player.Position;
            if (point.Equals(_lastPosition)) return;

            // Add the new waypoint. 
            Config.Instance.Waypoints.Add(new Waypoint(point));
            _lastPosition = point;
        }
    }
}

namespace EasyFarm.UserSettings
{
    public partial class Config
    {
        /// <summary>
        /// List of all waypoints that make up the bots path
        /// </summary>
        public ObservableCollection<Waypoint> Waypoints;
    }
}