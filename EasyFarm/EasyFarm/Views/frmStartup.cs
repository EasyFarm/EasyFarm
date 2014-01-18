using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using FFACETools;
using System.IO;

namespace EasyFarm.UtilityTools
{
    public partial class frmStartup : Form
    {
        public string AppName = "pol";
        private FFACE _session;
        private List<Process> _processes;
        private Process _process;

        public List<Process> POL_Processes 
        {
            get { return Process.GetProcessesByName(AppName).ToList(); }
            set { _processes = value; }
        }

        public Process POL_Process
        {
            get
            {
                return POL_Processes
                    .Where(x => x.MainWindowTitle.Equals(SessionsListBox.SelectedItem))
                    .FirstOrDefault();
            }

            set { _process = value; }
        }

        public FFACE FFXI_Session
        {
            get { return new FFACE(POL_Process.Id); }
            set { _session = value; }
        }

        public frmStartup()
        {
            InitializeComponent();
        }

        private void Startup_Load(object sender, EventArgs e)
        {
            if (POL_Processes.Count <= 0)
            {
                MessageBox.Show("FFXI Instances not detected, shutting down...");
                System.Environment.Exit(0);
            }

            //Cull all the FFXI Processes, and add their names to my listbox on the startup form
            var Query = POL_Processes.Select(x=> x.MainWindowTitle);
            
            foreach (var item in Query)
            {
                if (!SessionsListBox.Items.Contains(item))
                {
                    SessionsListBox.Items.Add(item);
                }
            }
        }

        //Triggers when a user makes a selection
        private void SessionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SessionsListBox.SelectedIndex == -1)
                return;

            this.Close();
        }
    }
}