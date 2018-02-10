// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////

using System.Windows;
using System.Windows.Forms;
using EasyFarm.Properties;
using EasyFarm.UserSettings;

namespace EasyFarm.Classes
{
    public class SystemTray
    {
        private static Window _window;
        private static readonly NotifyIcon TrayIcon = new NotifyIcon { Icon = Resources.trayicon };

        public static void ConfigureTray(Window window)
        {
            _window = window;
            _window.StateChanged += (s, e) => Minimize(_window.Title, @"EasyFarm has been minimized.");
            TrayIcon.Click += (s, e) => Unminimize();
            TrayIcon.BalloonTipClicked += (s, e) => Unminimize();
        }

        public static void Minimize(string title, string text)
        {
            if (Config.Instance.MinimizeToTray)
            {
                if (_window.WindowState != WindowState.Minimized) return;
                _window.Visibility = Visibility.Hidden;
                _window.ShowInTaskbar = false;
                TrayIcon.Visible = true;
                TrayIcon.ShowBalloonTip(5, title, text, ToolTipIcon.Info);
            }
        }

        public static void Unminimize()
        {
            _window.Show();
            _window.WindowState = WindowState.Normal;
            _window.Activate();
            _window.Topmost = true;
            _window.Topmost = false;
            _window.Focus();
            TrayIcon.Visible = false;
            _window.ShowInTaskbar = true;
        }
    }
}