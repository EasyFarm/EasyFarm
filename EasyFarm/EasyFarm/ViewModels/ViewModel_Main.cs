using EasyFarm.Engine;
using EasyFarm.MVVM;
using EasyFarm.ProcessTools;
using EasyFarm.UtilityTools;
using FFACETools;
using MvvmFoundation.Wpf;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;

namespace EasyFarm
{
    public partial class ViewModel : ObservableObject
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

            // Main Commands
            this.OnStart = new RelayCommand<object>(Action => Start(), Condition => { return true; });

            // Target Commands
            this.AddTargetUnitCommand = new RelayCommand<object>(
                    Action => AddUnit(Targets, TargetsName),
                    Condition => IsAddable(Targets, TargetsName));
            this.DeleteTargetUnitCommand = new RelayCommand<object>(
                    Action => DeleteUnit(Targets, TargetsName),
                    Condition => !IsAddable(Targets, TargetsName));
            this.ClearTargetUnitsCommand = new RelayCommand<object>(Action => ClearUnits(Targets));

            // Battle Commands
            AddActionCommand = new RelayCommand<object>(AddAction, IsBattleActionAddable);
            DeleteActionCommand = new RelayCommand<object>(DeleteAction, IsBattleActionRemovable);
            ClearActionsCommand = new RelayCommand(ClearActions);
            
            // WS Commands
            this.SetWeaponSkillCommand = new RelayCommand(SetWeaponSkill);
            
            // Ignore Commands
            this.AddIgnoredUnitCommand = new RelayCommand<object>(
                Action => AddUnit(Ignored, IgnoredName),
                Condition => IsAddable(Ignored, IgnoredName));
            this.DeleteIgnoredUnitCommand = new RelayCommand<object>(
                Action => DeleteUnit(Ignored, IgnoredName),
                Condition => !IsAddable(Ignored, IgnoredName));
            

            // Healing Commands
            this.AddHealingCommand = new RelayCommand(AddHealingItem);
            this.DeleteHealingCommand = new RelayCommand<object>(DeleteHealing);
            this.ClearHealingCommand = new RelayCommand(ClearHealing);
            
            // Routes Commands
            this.RecordRouteCommand = new RelayCommand(RecordRoute);
            this.ClearHealingCommand = new RelayCommand(ClearRoute);
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
            RaisePropertyChanged("KillPartyClaimed");
            RaisePropertyChanged("KillUnclaimed");
            RaisePropertyChanged("KillAggro");            
            RaisePropertyChanged("TargetsName");
            RaisePropertyChanged("IgnoredName");
        }

        public ICommand OnStart { get; set; }

        public String StatusBarText
        {
            get 
            {
                return Engine.Config.StatusBarText;
            }

            set 
            { 
                Engine.Config.StatusBarText = value;
                RaisePropertyChanged("StatusBarText");
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