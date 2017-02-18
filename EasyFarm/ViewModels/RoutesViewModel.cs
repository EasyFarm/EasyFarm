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

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using EasyFarm.States;
using Prism.Commands;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.ViewModels
{
    public class RoutesViewModel : ViewModelBase
    {        
        private readonly SettingsManager _settings;

        private string _recordHeader;

        public RoutesViewModel()
        {            
            _settings = new SettingsManager("ewl", "EasyFarm Waypoint List");            

            ClearCommand = new DelegateCommand(ClearRoute);
            RecordCommand = new DelegateCommand(Record);
            SaveCommand = new DelegateCommand(Save);
            LoadCommand = new DelegateCommand(Load);
            ResetNavigatorCommand = new DelegateCommand(ResetNavigator);

            RecordHeader = "Record";
            ViewName = "Routes";
        }

        private void PathRecorder_OnPositionAdded(Position position)
        {
            Application.Current.Dispatcher.Invoke(() => Route.Add(position));
        }

        public string RecordHeader
        {
            get { return _recordHeader; }
            set { SetProperty(ref _recordHeader, value); }
        }

        /// <summary>
        ///     Exposes the list of waypoints to the user.
        /// </summary>
        public ObservableCollection<Position> Route
        {
            get { return Config.Instance.Route.Waypoints; }
            set { SetProperty(ref Config.Instance.Route.Waypoints, value); }
        }

        /// <summary>
        ///     Binding for the record command for the GUI.
        /// </summary>
        public ICommand RecordCommand { get; set; }

        /// <summary>
        ///     Binding for the clear command for the GUI.
        /// </summary>
        public ICommand ClearCommand { get; set; }

        /// <summary>
        ///     Binding for the save command for the GUI.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        ///     Binding for the load command for the GUI.
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// A command to stop the player from running navigator 
        /// throwing an error. 
        /// </summary>
        public ICommand ResetNavigatorCommand { get; set; }

        /// <summary>
        ///     Clears the waypoint list.
        /// </summary>
        private void ClearRoute()
        {
            Config.Instance.Route.Reset();
        }

        /// <summary>
        ///     Pauses and resumes the path recorder based on
        ///     its current state.
        /// </summary>
        private void Record()
        {            
            // Return when the user has not selected a process. 
            if (FFACE == null)
            {
                AppServices.InformUser("No process has been selected.");
                return;
            }

            if (FFACE.Player.Zone != Config.Instance.Route.Zone && Config.Instance.Route.Zone != Zone.Unknown)
            {
                AppServices.InformUser("Cannot record waypoints from a different zone.");
                return;
            }

            Config.Instance.Route.Zone = FFACE.Player.Zone;

            if (!PathRecorder.IsRecording)
            {
                PathRecorder.OnPositionAdded += PathRecorder_OnPositionAdded;
                PathRecorder.Start();
                RecordHeader = "Recording!";
            }
            else
            {
                PathRecorder.OnPositionAdded -= PathRecorder_OnPositionAdded;
                PathRecorder.Stop();
                RecordHeader = "Record";
            }
        }

        /// <summary>
        ///     Saves the route data.
        /// </summary>
        private void Save()
        {
            AppServices.InformUser(_settings.TrySave(Config.Instance.Route) ? "Path has been saved." : "Failed to save path.");
        }

        /// <summary>
        ///     Loads the route data.
        /// </summary>
        private void Load()
        {
            var route = _settings.TryLoad<Route>();

            var isRouteLoaded = route != null;

            if (isRouteLoaded)
            {
                Config.Instance.Route = route;
                AppServices.InformUser("Path has been loaded.");
            }
            else
            {
                AppServices.InformUser("Failed to load the path.");
            }
        }

        /// <summary>
        /// Stops the player from running continously. 
        /// </summary>
        private void ResetNavigator()
        {
            // Return when the user has not selected a process. 
            if (FFACE == null)
            {
                AppServices.InformUser("No process has been selected.");
                return;
            }

            FFACE.Navigator.Reset();
        }
    }
}