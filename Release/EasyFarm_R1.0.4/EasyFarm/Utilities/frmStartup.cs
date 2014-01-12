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
        public List<Process> POL_Processes { get; set; }
        public FFACETools.FFACE FFXI_Session { get; set; }
        public Process POL_Process { get; set; }

        public frmStartup()
        {
            InitializeComponent();
        }

        private void Startup_Load(object sender, EventArgs e)
        {
            POL_Processes = new List<Process>();

            //Makes sure FFXI Is running
            POL_Processes.AddRange(Process.GetProcessesByName(AppName));
            if (POL_Processes.Count <= 0)
            {
                MessageBox.Show("FFXI Instances not detected, shutting down...");
                System.Environment.Exit(0);
            }

            //Cull all the FFXI Processes, and add their names to my listbox on the startup form
            var Query = from i in POL_Processes select i.MainWindowTitle;
            foreach (var item in Query)
                if (!SessionsListBox.Items.Contains(item))
                    SessionsListBox.Items.Add(item);
        }

        //Triggers when a user makes a selection
        private void SessionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Check to see if the user made a selection... or something can't remember right now XD
            if (SessionsListBox.SelectedIndex == -1)
                return;

            //Go through all the FFXI Processes, and where a processes Title matches something in the list box, select that item.
            //returns a process in query
            POL_Process = (from i in POL_Processes
                           where i.MainWindowTitle.Equals(SessionsListBox.SelectedItem.ToString())
                           select i).First();
            FFXI_Session = new FFACE(POL_Process.Id);

            this.Close();
        }
    }
}