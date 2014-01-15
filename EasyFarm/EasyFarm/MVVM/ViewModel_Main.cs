
using EasyFarm.Engine;
using EasyFarm.MVVM;
using EasyFarm.ProcessTools;
using EasyFarm.UtilityTools;
using FFACETools;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;

namespace EasyFarm
{
    public partial class ViewModel : ViewModelBase
    {
        public GameEngine Engine;
        DispatcherTimer WaypointRecorder = new DispatcherTimer();
        FFACE.Position LastPosition = new FFACE.Position();

        public ViewModel()
        {
            InitializeEngine();
            // Engine = new GameEngine(Utilities.GetPolProcess());
            // Stop the bot from running around
            Engine.FFInstance.Instance.Navigator.Reset();
            // Used for recording waypoints.
            WaypointRecorder.Tick += new EventHandler(RouteRecorder_Tick);
            WaypointRecorder.Interval = new TimeSpan(0, 0, 1);
        }
        
        private void InitializeEngine()
        {
            // Let user select ffxi process
            frmStartup ProcessSelectionScreen = new frmStartup();
            ProcessSelectionScreen.ShowDialog();

            // Validate the selection
            var m_process = ProcessSelectionScreen.POL_Process;

            if (m_process == null)
            {
                System.Windows.Forms.MessageBox.Show("No valid process was selected: Exiting now.");
                Environment.Exit(0);
            }

            // Set up the game engine if valid.
            Engine = new GameEngine(ProcessSelectionScreen.POL_Process);

            StatusBarText = "Bot Loaded: " + Engine.FFInstance.Instance.Player.Name;
        }

        public void RefreshConfig()
        {
            OnPropertyChanged("KillPartyClaimed");
            OnPropertyChanged("KillUnclaimed");
            OnPropertyChanged("KillAggro");            
            OnPropertyChanged("TargetsName");
            OnPropertyChanged("IgnoredName");
        }

        public ICommand OnStart
        {
            get
            {
                return new RelayCommand(Action => Start(), Condition => { return true; });
            }
        }

        public String StatusBarText
        {
            get 
            {
                return Engine.Config.StatusBarText;
            }

            set 
            { 
                Engine.Config.StatusBarText = value;
                OnPropertyChanged("StatusBarText");
            }
        }

        void Start()
        {
            Engine.FFInstance.Instance.Navigator.Reset();
            if (!Engine.IsWorking)
            {
                StatusBarText = "Running!";
                Engine.Start();
            }
            else
            {
                StatusBarText = "Paused!";
                Engine.Stop();
            }
        }

        bool IsAddable(IList Units, String name)
        {
            return !Units.Contains(name) && !String.IsNullOrWhiteSpace(name);
        }

        void AddUnit(IList Units, String name)
        {
            Units.Add(name);
        }

        void DeleteUnit(IList Units, String name)
        {
            Units.Remove(name);
        }

        void ClearUnits(IList Units)
        {
            Units.Clear();
        }
    }
}