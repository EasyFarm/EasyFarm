
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
        private readonly DispatcherTimer _pathRecorder;

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
            _pathRecorder = new DispatcherTimer();
            _pathRecorder.Tick += RouteRecorder_Tick;
            _pathRecorder.Interval = new TimeSpan(0, 0, 1);

            ClearCommand = new DelegateCommand(ClearRoute);
            RecordCommand = new DelegateCommand(RecordRoute);
            SaveCommand = new DelegateCommand(SaveRoute);
            LoadCommand = new DelegateCommand(LoadRoute);            

            _settingsManager = new SettingsManager(
                "ewl",
                "EasyFarm Waypoint List"
            );

            RecordHeader = "Record";
        }

        private string _recordHeader;

        public string RecordHeader
        {
            get { return _recordHeader; }
            set { SetProperty(ref _recordHeader, value); }
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
        public ICommand RecordCommand { get; set; }

        /// <summary>
        /// Binding for the clear command for the GUI. 
        /// </summary>
        public ICommand ClearCommand { get; set; }

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
        public void RecordRoute()
        {
            if (!_pathRecorder.IsEnabled)
            {
                _pathRecorder.Start();
                RecordHeader = "Recording!";
            }
            else
            {
                _pathRecorder.Stop();
                RecordHeader = "Record";
            }
        }

        /// <summary>
        /// Saves the route data. 
        /// </summary>
        private void SaveRoute()
        {
            try
            {
                _settingsManager.Save<ObservableCollection<Waypoint>>(Route);
                ViewModelBase.InformUser("Path has been saved.");    
            }
            catch (InvalidOperationException)
            {
                ViewModelBase.InformUser("Failed to save path.");
            }            
        }

        /// <summary>
        /// Loads the route data. 
        /// </summary>
        private void LoadRoute()
        {
            try
            {
                // Load the path
                var path = _settingsManager.Load<ObservableCollection<Waypoint>>();

                // Did we fail to load the settings?
                if (path == null)
                {
                    ViewModelBase.InformUser("Failed to load the path.");
                    return;
                }

                ViewModelBase.InformUser("Path has been loaded.");

                Route = path;
            }
            catch (InvalidOperationException)
            {
                ViewModelBase.InformUser("Failed to load the path.");
            }            
        }

        /// <summary>
        /// Records a new path for the player to travel. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouteRecorder_Tick(object sender, EventArgs e)
        {
            // Add a new waypoint only when we are not standing at 
            // our last position. 
            var playerPosition = FFACE.Player.Position;
            
            // Update the path if we've changed out position. Rotating our heading does not
            // count as the player moving. 
            if (playerPosition.X != _lastPosition.X || playerPosition.Z != _lastPosition.Z)
            {
                // Add the new waypoint. 
                Config.Instance.Waypoints.Add(new Waypoint(playerPosition));
                _lastPosition = playerPosition;
            }
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