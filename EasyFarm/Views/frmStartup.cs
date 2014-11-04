
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

using EasyFarm.FarmingTool;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyFarm.Views
{
    /// <summary>
    /// Allows the user to select his desired pol process and saves
    /// an FFACE session related to his selected process. 
    /// </summary>
    public partial class frmStartup : Form
    {
        /// <summary>
        /// Name of the application to monitor
        /// </summary>
        public string AppName = "pol";

        /// <summary>
        /// Monitors new processes or exited processes. 
        /// Will fire events when processes enter and exit the system. 
        /// </summary>
        public ProcessWatcher ProcessWatcher;
        
        /// <summary>
        /// Form initialization.
        /// </summary>
        public frmStartup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Setup the process monitor to the pol program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Startup_Load(object sender, EventArgs e)
        {
            ProcessWatcher = new ProcessWatcher(AppName);
            ProcessWatcher.Entry += ProcessWatcher_Entry;
            ProcessWatcher.Exit += ProcessWatcher_Exit;
            ProcessWatcher.Start();
        }

        /// <summary>
        /// Removes processes from the list box when they exit. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProcessWatcher_Exit(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                this.SessionsListBox.Items.Remove(ProcessFormat((e as ProcessEventArgs).Process));
            });
        }

        /// <summary>
        /// Adds processes to the list box when they are added to the system. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProcessWatcher_Entry(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                this.SessionsListBox.Items.Add(ProcessFormat((e as ProcessEventArgs).Process));
            });
        }

        /// <summary>
        /// Exits the form when a user picks a process. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SessionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SessionsListBox.SelectedIndex == -1) return;
            this.Close();
        }

        /// <summary>
        /// Returns the process as a string in a consistent fashion. 
        /// Windows Title: ID
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public String ProcessFormat(Process process)
        {
            return process.MainWindowTitle + ": " + process.Id;
        }

        /// <summary>
        /// The currently selected pol instance. 
        /// </summary>
        public Process SelectedProcess
        {
            get
            {
                if (SessionsListBox.SelectedItem == null) return null;
                
                return Process.GetProcessesByName(AppName)
                    .Where(x => SessionsListBox.SelectedItem.ToString().Contains(x.Id.ToString()))
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// The currently selected FFACE Instance. 
        /// </summary>
        public FFACE SelectedSession
        {
            get { return new FFACE(SelectedProcess.Id); }
        }
    }
}