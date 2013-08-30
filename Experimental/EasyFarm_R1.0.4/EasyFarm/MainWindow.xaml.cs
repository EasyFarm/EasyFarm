using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using EasyFarm.Engine;
using EasyFarm.PathingTools;
using EasyFarm.PlayerTools;
using EasyFarm.UnitTools;
using EasyFarm.UtilityTools;
using FFACETools;

namespace EasyFarm
{
    // Members and Constructors
    public partial class MainWindow : Window
    {
        #region Members
        GameEngine m_gameEngine = null;
        Process    m_process    = null;

        DispatcherTimer WaypointRecorder = new DispatcherTimer();
        FFACE.Position LastPosition = new FFACE.Position();

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            
            // Let user select ffxi process
            frmStartup ProcessSelectionScreen = new frmStartup();
            ProcessSelectionScreen.ShowDialog();

            // Validate the selection
            m_process = ProcessSelectionScreen.POL_Process;

            if (m_process == null)
            {
                System.Windows.Forms.MessageBox.Show("No valid process was selected: Exiting now.");
                Environment.Exit(0);
            }

            // Set up the game engine if valid.
            m_gameEngine = new GameEngine(ProcessSelectionScreen.POL_Process);
            WaypointRecorder.Tick += new EventHandler(WaypointRecorder_Tick); ;
            WaypointRecorder.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion

        // This section contains methods for discovering bugs
        // and investigating new methods for the bot.
        // currently, we have one view that shows 
        // creature data.

        #region Debugging Aids
        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {
            FFACE Session = m_gameEngine.FFInstance.Instance;
            Units Units = m_gameEngine.GameState.Units;
            Player Player = m_gameEngine.GameState.Player;

            NPCDialogDebug npcdd = new NPCDialogDebug(Session);
            npcdd.Show();

            DebugSpells ds = new DebugSpells(Session);
            ds.Show();

            DebugCreatures dc = new DebugCreatures(Units, Player);
            dc.Show();
        }
        #endregion

        // Shows the given grid to the user
        // and sets the status bar and header labels.

        #region Left Side Expanders
        private void TargetsTabVisible(object sender, RoutedEventArgs e)
        {
            SetHeaderContent("Targets", "Set the mobs to kill.");
        }

        private void IgnoredTabVisible(object sender, RoutedEventArgs e)
        {
            SetHeaderContent("Ignored Mobs", "Set the mobs to ignore");
        }

        private void RouteTabVisible(object sender, RoutedEventArgs e)
        {
            SetHeaderContent("Routes", "Set the farm route.");
        }

        private void EventsTabVisible(object sender, RoutedEventArgs e)
        {
            SetHeaderContent("Battle Abilities", "Set the abilities to use in battle.");
        }

        private void WeaponsTabVisible(object sender, RoutedEventArgs e)
        {
            SetHeaderContent("Weaponskills", "Set the weaponskill to use in battle.");
        }

        private void RestingTabVisible(object sender, RoutedEventArgs e)
        {
            SetHeaderContent("Resting", "Set the healing on and off values.");
        }

        private void HealingTabVisible(object sender, RoutedEventArgs e)
        {
            SetHeaderContent("Healing Abilities",
                "Set the abilities used to heal and remove status effects in battle.");
        }

        private void PetsTabVisible(object sender, RoutedEventArgs e)
        {
            SetHeaderContent("Pet Options",
                "Configurations for pets in and out of battle.");
        }

        private void SetHeaderContent(String title, String description)
        {
            CategoryName.Content = title;
            CategoryDescription.Content = description;
        }
        #endregion

        /*////////////////////////////////////////////////////////////////////*/

        // Used to pump data to main form.
        // Like when the user presses start is prints
        // running in the status bar.

        #region Player Events
        void Player_OnStart()
        {
            lblStatusBar.Content = "Running";
        }

        void Player_OnStopped()
        {
            lblStatusBar.Content = "Paused";
        }
        #endregion

        /*////////////////////////////////////////////////////////////////////*/

        // Deals with the target and monster grids.
        // Options like kill aggro and ignore target
        // are here.

        #region Monsters Events
        #region Monster Events - Targeted
        private void btnAddTarget_Click(object sender, RoutedEventArgs e)
        {
            if (!cboTargets.Items.Contains(cboTargets.Text) && !string.IsNullOrWhiteSpace(cboTargets.Text))
            {
                cboTargets.Items.Add(cboTargets.Text);
                m_gameEngine.GameState.Config.TargetsList.Add(cboTargets.Text);
            }
        }

        private void btnDeleteTarget_Click(object sender, RoutedEventArgs e)
        {
            if (cboTargets.SelectedItem == null)
                return;

            m_gameEngine.GameState.Config.TargetsList.Remove(cboTargets.SelectedItem.ToString());
            cboTargets.Items.Remove(cboTargets.SelectedItem);
        }

        private void btnClearTarget_Click(object sender, RoutedEventArgs e)
        {
            m_gameEngine.GameState.Config.TargetsList.Clear();
            cboTargets.Items.Clear();
        }
        #endregion

        #region Monster Events - Ignored
        // Events for the ignored list
        private void btnAddIgnored_Click(object sender, RoutedEventArgs e)
        {
            if (!cboIgnoreList.Items.Contains(cboIgnoreList.Text) &&
                !string.IsNullOrWhiteSpace(cboIgnoreList.Text))
            {
                cboIgnoreList.Items.Add(cboIgnoreList.Text);
                m_gameEngine.GameState.Config.IgnoredList.Add(cboIgnoreList.Text);
            }
        }

        private void btnClearIgnored_Click(object sender, RoutedEventArgs e)
        {
            m_gameEngine.GameState.Config.IgnoredList.Clear();
            cboIgnoreList.Items.Clear();
        }

        private void btnDeleteIgnored_Click(object sender, RoutedEventArgs e)
        {
            if (cboIgnoreList.SelectedItem == null)
                return;
            m_gameEngine.GameState.Config.IgnoredList.Remove(cboIgnoreList.SelectedItem.ToString());
            cboIgnoreList.Items.Remove(cboIgnoreList.SelectedItem);
        }
        #endregion

        #region Monster Events - Engage Conditions
        // Events for the condition on when to target mobs
        private void chkKillAggro_Checked(object sender, RoutedEventArgs e)
        {
            if (m_gameEngine != null)
            {
                m_gameEngine.GameState.Config.BattleAggro = chkKillAggro.IsChecked.Value;
            }
        }

        private void chkKillUnclaimed_Checked(object sender, RoutedEventArgs e)
        {
            if (m_gameEngine != null)
            {
                m_gameEngine.GameState.Config.BattleUnclaimed = chkKillUnclaimed.IsChecked.Value;
            }            
        }

        private void chkKillPartyClaimed_Checked(object sender, RoutedEventArgs e)
        {
            if (m_gameEngine != null)
            {
                m_gameEngine.GameState.Config.BattlePartyClaimed = chkKillPartyClaimed.IsChecked.Value;
            }            
        }

        private void chkKillAggro_Unchecked(object sender, RoutedEventArgs e)
        {
            m_gameEngine.GameState.Config.BattleAggro = chkKillAggro.IsChecked.Value;
        }

        private void chkKillPartyClaimed_Unchecked(object sender, RoutedEventArgs e)
        {
            m_gameEngine.GameState.Config.BattlePartyClaimed = chkKillPartyClaimed.IsChecked.Value;
        }

        private void chkKillUnclaimed_Unchecked(object sender, RoutedEventArgs e)
        {
            m_gameEngine.GameState.Config.BattleUnclaimed = chkKillUnclaimed.IsChecked.Value;
        }
        #endregion
        #endregion

        // Deals with waypoint recording 

        #region Waypoint Events
        private void btnRecordWaypoints_Click(object sender, RoutedEventArgs e)
        {
            if (!WaypointRecorder.IsEnabled)
            {
                WaypointRecorder.Start();
                lblStatusBar.Content = "Recording Waypoints...";
            }
            else
            {
                WaypointRecorder.Stop();
                lblStatusBar.Content = "Recording Stopped";
            }
        }

        private void btnClearWaypoints_Click(object sender, RoutedEventArgs e)
        {
            Pathing Pathing = m_gameEngine.GameState.Pathing;
            Pathing.ClearWaypoints();
            lstWaypoints.Items.Clear();
        }

        private void WaypointRecorder_Tick(object sender, EventArgs e)
        {
            FFACE Session = m_gameEngine.FFInstance.Instance;
            Pathing Pathing = m_gameEngine.GameState.Pathing;

            if (!lstWaypoints.Items.Contains("X:" + Session.Player.PosX + ", Z:" + Session.Player.PosZ))
            {
                lstWaypoints.Items.Add("X:" + Session.Player.PosX + ", Z:" + Session.Player.PosZ);
                Pathing.AddWaypoint();
            }
        }
        #endregion

        // Deals with weaponskills.

        #region Weaponskill Events
        private void btnSetWeaponskill_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txbWeaponskill.Text))
            {
                var Weaponskill = new WeaponAbility(txbWeaponskill.Text, sldDistance.Value);

                if (Weaponskill.IsValidName)
                {
                    m_gameEngine.GameState.Config.Weaponskill = Weaponskill;
                    lblStatusBar.Content = "Weaponskill set.";
                }
            }
        }

        private void sldDistance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblStatusBar != null)
                lblStatusBar.Content = "Distance: " + sldDistance.Value;
        }

        private void sldHealthThreshold_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (m_gameEngine != null)
            {
                int Health = (int)sldHealthThreshold.Value;
                m_gameEngine.GameState.Config.WeaponSkillHP = Health;
                lblStatusBar.Content = "Use at health: " + Health + "%";
            }
        }
        #endregion

        // Deals with resting

        #region Resting Events
        private void btnSetResting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_gameEngine.GameState.Config.StandUPValue = Convert.ToInt32(txbStandUp.Text);
                m_gameEngine.GameState.Config.RestingValue = Convert.ToInt32(txbSitDown.Text);
                lblStatusBar.Content =
                    "Sitdown: " + m_gameEngine.GameState.Config.RestingValue +
                    " Standup: " + m_gameEngine.GameState.Config.StandUPValue;
            }
            catch (FormatException)
            {
                lblStatusBar.Content = "Error : Bad Value -> Stand Up or Sit Down Values";
            }
        }
        #endregion

        // Deals with pre, mid, post battle events

        #region Battle Ability Events
        private void btnAddAbility_Click(object sender, RoutedEventArgs e)
        {
            String input = txbAbilityName.Text;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var Action = Ability.CreateAbility(input);

                if (!string.IsNullOrWhiteSpace(Action.Name) && !lstAbilities.Items.Contains(Action.Name))
                {
                    lstAbilities.Items.Add(Action.Name);

                    if (radStartingAction.IsChecked == true)
                        m_gameEngine.GameState.Config.Actions[ActionType.Enter].Add(Action);
                    else if (radBattleAction.IsChecked == true)
                        m_gameEngine.GameState.Config.Actions[ActionType.Battle].Add(Action);
                    else
                        m_gameEngine.GameState.Config.Actions[ActionType.Exit].Add(Action);
                }
                else
                {
                    lblStatusBar.Content = "Error Adding Ability: " + input;
                }
            }
        }

        private void btnDeleteAbility_Click(object sender, RoutedEventArgs e)
        {
            if (lstAbilities.SelectedItem != null)
            {
                if (radStartingAction.IsChecked.Value)
                    RemoveActionFromList(m_gameEngine.GameState.Config.Actions[ActionType.Enter], lstAbilities);
                else if (radBattleAction.IsChecked.Value)
                    RemoveActionFromList(m_gameEngine.GameState.Config.Actions[ActionType.Battle], lstAbilities);
                else
                    RemoveActionFromList(m_gameEngine.GameState.Config.Actions[ActionType.Exit], lstAbilities);

                lstAbilities.Items.Remove(lstAbilities.SelectedItem);
            }
        }

        private void btnClearAbilities_Click(object sender, RoutedEventArgs e)
        {
            if (radStartingAction.IsChecked.Value)
                m_gameEngine.GameState.Config.Actions[ActionType.Enter].Clear();
            else if (radBattleAction.IsChecked.Value)
                m_gameEngine.GameState.Config.Actions[ActionType.Battle].Clear();
            else
                m_gameEngine.GameState.Config.Actions[ActionType.Exit].Clear();

            lstAbilities.Items.Clear();
        }

        private void radStart_Checked(object sender, RoutedEventArgs e)
        {
            if (lstAbilities != null)
                UpdateActionLists(m_gameEngine.GameState.Config.Actions[ActionType.Enter], lstAbilities);
        }

        private void radBattle_Checked(object sender, RoutedEventArgs e)
        {
            UpdateActionLists(m_gameEngine.GameState.Config.Actions[ActionType.Battle], lstAbilities);
        }

        private void radEnd_Checked(object sender, RoutedEventArgs e)
        {
            UpdateActionLists(m_gameEngine.GameState.Config.Actions[ActionType.Exit], lstAbilities);
        }
        #endregion

        /*////////////////////////////////////////////////////////////////////*/

        #region Menu Events
        private void mnuStart_Click(object sender, RoutedEventArgs e)
        {
            FFACE Session = m_gameEngine.FFInstance.Instance;

            Session.Navigator.Reset();

            if (!m_gameEngine.IsWorking)
            {
                m_gameEngine.Start();
            }
            else
            {
                m_gameEngine.Stop();
            }
        }
        #endregion

        #region Mainform Events
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            String Filename = m_gameEngine.FFInstance.Instance.Player.Name + "_UserPref.xml";

            #region Serialize
            if (m_gameEngine == null)
            {
                return;
            }

            Utilities.Serialize(Filename, m_gameEngine.GameState.Config);
            #endregion
            
            // Stop bot from running around
            m_gameEngine.FFInstance.Instance.Navigator.Reset();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_gameEngine == null)
            {
                return;
            }

            #region Deserialize and Initialize Gui
            // Deserialize            
            LoadSettingsFromFile();
            #endregion

            // Stop bot from running around
            m_gameEngine.FFInstance.Instance.Navigator.Reset();
        }

        private void LoadSettingsFromFile()
        {
            String Filename = m_gameEngine.FFInstance.Instance.Player.Name + "_UserPref.xml";
            m_gameEngine.GameState.Config = Utilities.Deserialize(Filename, m_gameEngine.GameState.Config);            

            // Lists
            for (int i = 0; i < m_gameEngine.GameState.Config.Waypoints.Length; i++)
            {
                lstWaypoints.Items.Add("X: " + m_gameEngine.GameState.Config.Waypoints[i].X + ", Z: " + m_gameEngine.GameState.Config.Waypoints[i].Z);
            }
            
            m_gameEngine.GameState.Config.TargetsList.ForEach(target => cboTargets.Items.Add(target));
            m_gameEngine.GameState.Config.IgnoredList.ForEach(ignore => cboIgnoreList.Items.Add(ignore));
            m_gameEngine.GameState.Config.Actions[ActionType.Enter].ForEach(ability => lstAbilities.Items.Add(ability.Name));

            // Monster Options
            chkKillAggro.IsChecked = m_gameEngine.GameState.Config.BattleAggro;
            chkKillPartyClaimed.IsChecked = m_gameEngine.GameState.Config.BattlePartyClaimed;
            chkKillUnclaimed.IsChecked = m_gameEngine.GameState.Config.BattleUnclaimed;

            // Weaponskills
            txbWeaponskill.Text = m_gameEngine.GameState.Config.Weaponskill.Name;
            sldDistance.Value = m_gameEngine.GameState.Config.Weaponskill.MaxDistance;
            sldHealthThreshold.Value = m_gameEngine.GameState.Config.WeaponSkillHP;

            // Resting
            txbStandUp.Text = m_gameEngine.GameState.Config.StandUPValue.ToString();
            txbSitDown.Text = m_gameEngine.GameState.Config.RestingValue.ToString();
        }
        #endregion

        #region Misc
        private void UpdateActionLists(List<Ability> playersActions, ListBox listbox)
        {
            listbox.Items.Clear();
            foreach (var Action in playersActions)
                listbox.Items.Add(Action.Name);
        }

        private void RemoveActionFromList(List<Ability> playersActions, ListBox listbox)
        {
            var ChosenAbility = new Ability();

            foreach (var SomeAction in playersActions)
                if (SomeAction.Name == listbox.SelectedItem.ToString())
                    ChosenAbility = SomeAction;

            playersActions.Remove(ChosenAbility);
        }
        #endregion

        #region Pet Options
        private void ResummonPet_Checked(object sender, RoutedEventArgs e)
        {
            m_gameEngine.GameState.Config.ResummonPet = true;
        } 
        #endregion

        private void ResummonPet_Unchecked(object sender, RoutedEventArgs e)
        {
            m_gameEngine.GameState.Config.ResummonPet = false;
        }

        private void SetResummonCommand_Click(object sender, RoutedEventArgs e)
        {
            //m_gameEngine.GameState.Player.StartResummonPetTimer();
        }

        ////////////////////////////////////////////////////////////////////
    }
}
