using System;
using System.Windows;
using System.Windows.Forms;
using EasyFarm.Properties;

namespace EasyFarm.Classes
{
    public class SystemTray : ISystemTray
    {
        private readonly Window _window;
        private readonly NotifyIcon _trayIcon;

        public SystemTray(Window window, NotifyIcon trayIcon)
        {
            _window = window;
            _trayIcon = trayIcon;
            _trayIcon.Icon = Resources.trayicon;
        }

        public void Minimize(string title, string text)
        {
            if (_window.WindowState != WindowState.Minimized) return;
            _window.Visibility = Visibility.Hidden;
            _window.ShowInTaskbar = false;
            _trayIcon.Visible = true;
            _trayIcon.ShowBalloonTip(5, title, text, ToolTipIcon.Info);
        }

        public void ConfigureSystemTray(
            Action minimizeAction,
            Action unminimizeAction)
        {
            _window.StateChanged += (s, e) => minimizeAction();
            _trayIcon.Click += (s, e) => unminimizeAction();
            _trayIcon.BalloonTipClicked += (s, e) => unminimizeAction();
        }

        public void Unminimize()
        {
            _window.Show();
            _window.WindowState = WindowState.Normal;
            _window.Activate();
            _window.Topmost = true;
            _window.Topmost = false;
            _window.Focus();
            _trayIcon.Visible = false;
            _window.ShowInTaskbar = true;
        }
    }
}