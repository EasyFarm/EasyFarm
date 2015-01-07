
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

using EasyFarm.GameData;
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
using ZeroLimits.XITool.Classes;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Routes")]
    public class RoutesViewModel : ViewModelBase
    {
        readonly DispatcherTimer _waypointRecorder = new DispatcherTimer();
        FFACE.Position _lastPosition = new FFACE.Position();
        
        public RoutesViewModel() 
        {
            _waypointRecorder.Tick += RouteRecorder_Tick;
            _waypointRecorder.Interval = new TimeSpan(0, 0, 1);

            ClearRouteCommand = new DelegateCommand(ClearRoute);
            RecordRouteCommand = new DelegateCommand<Object>(RecordRoute);
            SaveCommand = new DelegateCommand(SaveRoute);
            LoadCommand = new DelegateCommand(LoadRoute);
        }

        public ObservableCollection<Waypoint> Route
        {
            get { return Config.Instance.Waypoints; }
            set { SetProperty(ref Config.Instance.Waypoints, value); }
        }

        public ICommand RecordRouteCommand { get; set; }

        public ICommand ClearRouteCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand LoadCommand { get; set; }

        public void ClearRoute()
        {
            Route = new ObservableCollection<Waypoint>();
        }

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

        private void SaveRoute()
        {
            var sfd = new SaveFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                DefaultExt = ".wpl",
                Filter = "Waypoint lists (.wpl)|*.wpl"
            };

            if (sfd.ShowDialog() == true)
            {
                Serialization.Serialize(sfd.SafeFileName, Route);
            }
        }

        private void LoadRoute()
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                DefaultExt = ".wpl",
                Filter = "Waypoint lists (.wpl)|*.wpl"
            };

            if (ofd.ShowDialog() == true)
            {
                Route = Serialization.Deserialize(ofd.SafeFileName, Route);
            }
        }

        void RouteRecorder_Tick(object sender, EventArgs e)
        {
            var point = FFACE.Player.Position;

            if (point.Equals(_lastPosition))
            {
                return;
            }
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