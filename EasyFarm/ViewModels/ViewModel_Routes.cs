
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
*////////////////////////////////////////////////////////////////////

﻿using EasyFarm.MVVM;
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

        public ICommand RecordRouteCommand { get; set; }

        public ICommand ClearRouteCommand { get; set; }

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
            Engine.Pathing.AddWaypoint();
        }
    }
}