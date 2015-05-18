
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
*//////////////////////////////////////////////////////////////////// 

using EasyFarm.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace EasyFarm.Views
{
    // Members and Constructors
    public partial class MasterView : MetroWindow
    {
        private const String TRAY_ICON_FILE_NAME = "trayicon.ico";

        private NotifyIcon m_trayIcon = new NotifyIcon();

        public MasterView()
        {
            InitializeComponent();
            try
            {
                this.DataContext = new MasterViewModel();            
            }
            catch (Exception)
            {                
                throw;
            }            

            if (File.Exists(TRAY_ICON_FILE_NAME))
            {
                m_trayIcon.Icon = new System.Drawing.Icon(TRAY_ICON_FILE_NAME);                
            }

            m_trayIcon.Click += TrayIcon_Click;
        }

        protected override void OnStateChanged(System.EventArgs e)
        {
            // Perform tray icon information update here to 
            // receive current title bar information. 
            m_trayIcon.Text = this.WindowTitle;
            m_trayIcon.BalloonTipText = "EasyFarm has been minimized. ";
            m_trayIcon.BalloonTipTitle = this.WindowTitle;

            if (mnuMinimizeToTray.IsChecked && this.WindowState == WindowState.Minimized)
            {
                this.m_trayIcon.Visible = true;
                this.m_trayIcon.ShowBalloonTip(30);
                this.ShowInTaskbar = false;
            }
        }

        public void TrayIcon_Click(object sender, System.EventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.ShowInTaskbar = true;
            this.m_trayIcon.Visible = false;
        }
    }
}
