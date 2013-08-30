using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using EasyFarm.PlayerTools;
using EasyFarm.UnitTools;
using FFACETools;

namespace EasyFarm
{
    // Members and Constructors
    public partial class MainWindow : Window
    {
        #region Members
        
        Player Player;
        FFACE Session;
        DispatcherTimer WaypointRecorder = new DispatcherTimer();
        DispatcherTimer ArrowRestocker = new DispatcherTimer();
        DispatcherTimer RecoveryPathTimer = new DispatcherTimer();
        FFACE.Position LastPosition = new FFACE.Position();

        #endregion

        #region Constructors
        public MainWindow()
        {                       
            InitializeComponent();
            
            Session = Utilities.CreateFFACE();
            Player = new Player(Session);

            // ArrowRestocker.Tick += new EventHandler(ArrowRestocker_Tick);
            WaypointRecorder.Tick += new EventHandler(WaypointRecorder_Tick);
            // ArrowRestocker.Interval = new TimeSpan(0, 0, 1);
            WaypointRecorder.Interval = new TimeSpan(0, 0, 1);
            // RecoveryPathTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            // RecoveryPathTimer.Tick += new EventHandler(RecoveryPathTimer_Tick);
            Player.OnFinish += new PlayerTools.Player.Handler(Player_OnStopped);
            Player.OnStart += new PlayerTools.Player.Handler(Player_OnStart);

            //btnDebug_Click(null, null);
        }
        #endregion

        // This section contains methods for discovering bugs
        // and investigating new methods for the bot.
        // currently, we have one view that shows 
        // creature data.

        #region Debugging Aids
        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {
            NPCDialogDebug npcdd = new NPCDialogDebug(Session);
            npcdd.Show();

            DebugSpells ds = new DebugSpells(Session);
            ds.Show();

            DebugCreatures dc = new DebugCreatures(Player.Units, Player);
            dc.Show();
        }
        #endregion

        // Shows the given grid to the user
        // and sets the status bar and header labels.

        #region Left Side Expanders
        private void TargetsButton_Click(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdTargets);
            CategoryName.Content = "Targets";
            CategoryDescription.Content = "Set the mobs to kill.";
        }

        private void IgnoredButton_Click(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdIgnored);
            CategoryName.Content = "Ignored Mobs";
            CategoryDescription.Content = "Set the mobs to ignore";
        }

        private void RouteButton_Click(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdRoute);
            CategoryName.Content = "Routes";
            CategoryDescription.Content = "Set the farm route.";
        }

        private void RecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdRecovery);
            CategoryName.Content = "Recovery Path";
            CategoryDescription.Content = "Create a path to your route.";
        }

        private void EventsButton_Click(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdEvents);
            CategoryName.Content = "Battle Abilities";
            CategoryDescription.Content = "Set the abilities to use in battle.";
        }

        private void WeaponButton_Click(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdWeapon);
            CategoryName.Content = "Weaponskills";
            CategoryDescription.Content = "Set the weaponskill to use in battle.";
        }

        private void RestingButton_Click(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdResting);
            CategoryName.Content = "Resting";
            CategoryDescription.Content = "Set the healing on and off values.";
        }

        private void HealingButton_Click(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdHealing);
            CategoryName.Content = "Healing Abilities";
            CategoryDescription.Content = "Set the abilities used to heal and remove status effects in battle.";
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
                Player.Units.TargetNames.Add(cboTargets.Text);
            }
        }

        private void btnDeleteTarget_Click(object sender, RoutedEventArgs e)
        {
            if (cboTargets.SelectedItem == null)
                return;

            Player.Units.TargetNames.Remove(cboTargets.SelectedItem.ToString());
            cboTargets.Items.Remove(cboTargets.SelectedItem);
        }

        private void btnClearTarget_Click(object sender, RoutedEventArgs e)
        {
            Player.Units.TargetNames.Clear();
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
                Player.Units.IgnoredMobs.Add(cboIgnoreList.Text);
            }
        }

        private void btnClearIgnored_Click(object sender, RoutedEventArgs e)
        {
            Player.Units.IgnoredMobs.Clear();
            cboIgnoreList.Items.Clear();
        }

        private void btnDeleteIgnored_Click(object sender, RoutedEventArgs e)
        {
            if (cboIgnoreList.SelectedItem == null)
                return;
            Player.Units.IgnoredMobs.Remove(cboIgnoreList.SelectedItem.ToString());
            cboIgnoreList.Items.Remove(cboIgnoreList.SelectedItem);
        }
        #endregion

        #region Monster Events - Engage Conditions
        // Events for the condition on when to target mobs
        private void chkKillAggro_Checked(object sender, RoutedEventArgs e)
        {
            if (Player != null)
            {
                Player.KillAggro = chkKillAggro.IsChecked.Value;
            }
        }

        private void chkKillUnclaimed_Checked(object sender, RoutedEventArgs e)
        {
            if (Player != null)
            {
                Player.KillUnclaimed = chkKillUnclaimed.IsChecked.Value;
            }
        }

        private void chkKillPartyClaimed_Checked(object sender, RoutedEventArgs e)
        {
            if (Player != null)
            {
                Player.KillPartyClaimed = chkKillPartyClaimed.IsChecked.Value;
            }
        }

        private void chkKillAggro_Unchecked(object sender, RoutedEventArgs e)
        {
            Player.KillAggro = chkKillAggro.IsChecked.Value;
        }

        private void chkKillPartyClaimed_Unchecked(object sender, RoutedEventArgs e)
        {
            Player.KillPartyClaimed = chkKillPartyClaimed.IsChecked.Value;
        }

        private void chkKillUnclaimed_Unchecked(object sender, RoutedEventArgs e)
        {
            Player.KillUnclaimed = chkKillUnclaimed.IsChecked.Value;
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
            Player.Pathing.ClearWaypoints();
            lstWaypoints.Items.Clear();
        }

        private void WaypointRecorder_Tick(object sender, EventArgs e)
        {
            if (!lstWaypoints.Items.Contains("X:" + Session.Player.PosX + ", Z:" + Session.Player.PosZ))
            {
                lstWaypoints.Items.Add("X:" + Session.Player.PosX + ", Z:" + Session.Player.PosZ);
                Player.Pathing.AddWaypoint();
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
                    Player.Weaponskill = Weaponskill;
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
            if (Player == null)
            {
                return;
            }

            int Health = (int)sldHealthThreshold.Value;
            Player.SetWeaponSkillHP(Health);
            lblStatusBar.Content = "Use at health: " + Health + "%";
        }
        #endregion

        // Deals with resting

        #region Resting Events
        private void btnSetResting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Player.StandUPHPPValue = Convert.ToInt32(txbStandUp.Text);
                Player.SitdownHPPValue = Convert.ToInt32(txbSitDown.Text);
                lblStatusBar.Content = "Sitdown: " + Player.SitdownHPPValue +
                    " Standup: " + Player.StandUPHPPValue;
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
                        Player.StartingResponses.Add(Action);
                    else if (radBattleAction.IsChecked == true)
                        Player.CombatResponses.Add(Action);
                    else
                        Player.EndingResponses.Add(Action);
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
                    RemoveActionFromList(Player.StartingResponses, lstAbilities);
                else if (radBattleAction.IsChecked.Value)
                    RemoveActionFromList(Player.CombatResponses, lstAbilities);
                else
                    RemoveActionFromList(Player.EndingResponses, lstAbilities);

                lstAbilities.Items.Remove(lstAbilities.SelectedItem);
            }
        }

        private void btnClearAbilities_Click(object sender, RoutedEventArgs e)
        {
            if (radStartingAction.IsChecked.Value)
                Player.StartingResponses.Clear();
            else if (radBattleAction.IsChecked.Value)
                Player.CombatResponses.Clear();
            else
                Player.EndingResponses.Clear();

            lstAbilities.Items.Clear();
        }

        private void radStart_Checked(object sender, RoutedEventArgs e)
        {
            if (lstAbilities != null)
                UpdateActionLists(Player.StartingResponses, lstAbilities);
        }

        private void radBattle_Checked(object sender, RoutedEventArgs e)
        {
            UpdateActionLists(Player.CombatResponses, lstAbilities);
        }

        private void radEnd_Checked(object sender, RoutedEventArgs e)
        {
            UpdateActionLists(Player.EndingResponses, lstAbilities);
        }
        #endregion

        // Deals with healing ability events
        #region Healing Ability Events
        private void btnAddHealing_Click(object sender, RoutedEventArgs e)
        {
            String input = txbHealingAbility.Text;
            String threshold = txbHealingThreshold.Text;
            
            var Ability = new Healing();

            if (!string.IsNullOrWhiteSpace(input) &&
                !string.IsNullOrWhiteSpace(threshold))
            {
                try
                {
                    Ability = new Healing(input, Convert.ToInt32(threshold));
                }
                catch (FormatException)
                {
                    lblStatusBar.Content = "Error : Bad Value -> Healing Threshold";
                }
            }
            else
                Ability = new Healing(input);

            if (Ability.IsValidName && !lstHealingActions.Items.Contains(Ability.Name))
            {
                if (radHealing.IsChecked.Value && Ability.HPThreshold > 0)
                {
                    Player.HealingResponses.Add(Ability);
                    lstHealingActions.Items.Add(Ability.Name);
                }
                else if (radRemoval.IsChecked.Value)
                {
                    Player.DebuffResponses.Add(Ability);
                    lstHealingActions.Items.Add(Ability.Name);
                }
            }
            else
                lblStatusBar.Content = "Error Adding Ability: " + input;
        }

        private void btnDeleteHealing_Click(object sender, RoutedEventArgs e)
        {
            if (lstHealingActions.SelectedItem != null)
            {
                if (radHealing.IsChecked.Value)
                    RemoveActionFromList(Player.HealingResponses, lstHealingActions);
                else
                    RemoveActionFromList(Player.DebuffResponses, lstHealingActions);

                lstHealingActions.Items.Remove(lstHealingActions.SelectedItem);
            }
        }

        private void btnClearHealing_Click(object sender, RoutedEventArgs e)
        {
            if (radHealing.IsChecked.Value)
                Player.HealingResponses.Clear();
            else
                Player.DebuffResponses.Clear();

            lstHealingActions.Items.Clear();
        }

        private void radRemoval_Checked(object sender, RoutedEventArgs e)
        {
            if (lstHealingActions != null)
                UpdateActionLists(Player.DebuffResponses, lstHealingActions);
        }

        private void radHealing_Checked(object sender, RoutedEventArgs e)
        {
            if (lstHealingActions != null)
                UpdateActionLists(Player.HealingResponses, lstHealingActions);
        }
        #endregion

        // Depreciated -- No intent to implement...
        #region Recovery Path Events
        void RecoveryPathTimer_Tick(object sender, EventArgs e)
        {
            RecoveryPathTimer.Stop();
            PrintRecoveryPath();

            if (!(LastPosition.Equals(Session.Player.Position)))
            {
                Player.RecoveryPath.Path.Add(new InteractionPoint(Session.Player.Position, new List<KeyCode>(), ""));
                LastPosition = Session.Player.Position;
            }

            RecoveryPathTimer.Start();
        }

        private void btnRecordRecoveryPath_Click(object sender, RoutedEventArgs e)
        {
            if (!RecoveryPathTimer.IsEnabled)
            {
                RecoveryPathTimer.Start();
                lblStatusBar.Content = "Recording Recovery Path";
                lstRecoverPath.Items.Clear();
            }
            else
            {
                RecoveryPathTimer.Stop();
                lblStatusBar.Content = "Recording Stopped";
            }
        }

        private void btnRecordTarget_Click(object sender, RoutedEventArgs e)
        {
            var TargetsName = string.Empty;

            if (!string.IsNullOrWhiteSpace(txtRecordTarget.Text))
                TargetsName = txtRecordTarget.Text;

            // If we have found the targetee, and there is only one of them
            if (Player.Units.MobArray.Where(Mob => Mob.Name == TargetsName).Count() == 1)
            {
                // Target the Target
                while (Session.Target.Name != TargetsName)
                {
                    Player.TargetNPC(Player.Units.MobArray
                        .Where(Mob => Mob.Name == TargetsName)
                        .SingleOrDefault());
                }

                // Save Target
                if (RecoveryPathTimer.IsEnabled)
                {
                    Player.RecoveryPath.Path.Last().Target.Clear().Append(TargetsName);
                }
            }
        }

        // Events to record keys
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            var Key = KeyCode.NP_EnterKey;
            AddKey(Key);
            Session.Windower.SendKeyPress(Key);
        }

        private void btnEscape_Click(object sender, RoutedEventArgs e)
        {
            var Key = KeyCode.EscapeKey;
            AddKey(Key);
            Session.Windower.SendKeyPress(Key);
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            var Key = KeyCode.MinusKey;
            AddKey(Key);
            Session.Windower.SendKeyPress(Key);
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            var Key = KeyCode.UpArrow;
            AddKey(Key);
            Session.Windower.SendKeyPress(Key);
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            var Key = KeyCode.LeftArrow;
            AddKey(Key);
            Session.Windower.SendKeyPress(Key);
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            var Key = KeyCode.DownArrow;
            AddKey(Key);
            Session.Windower.SendKeyPress(Key);
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            var Key = KeyCode.RightArrow;
            AddKey(Key);
            Session.Windower.SendKeyPress(Key);
        }
        #endregion

        /*////////////////////////////////////////////////////////////////////*/

        #region Tab Events
        private void tabWeaponskills_GotFocus(object sender, RoutedEventArgs e)
        {
            lblStatusBar.Content = "Distance: " + sldDistance.Value;
        }

        private void tabCreatures_GotFocus(object sender, RoutedEventArgs e)
        {
            lblStatusBar.Content = "";
        }

        private void tabWaypoints_GotFocus(object sender, RoutedEventArgs e)
        {
            lblStatusBar.Content = "";
        }

        private void tabAbilities_GotFocus(object sender, RoutedEventArgs e)
        {
            lblStatusBar.Content = "";
        }

        private void tabRestingHealing_GotFocus(object sender, RoutedEventArgs e)
        {
            lblStatusBar.Content = "";
        }
        #endregion

        #region Menu Events
        private void mnuStart_Click(object sender, RoutedEventArgs e)
        {
            Session.Navigator.Reset();

            if (!Player.IsWorking)
                Player.Run();
            else
                Player.Pause();
        }

        private void mnuDebugCreatures_Click(object sender, RoutedEventArgs e)
        {
            var dbgMobs = new DebugCreatures(Player.Units, Player);
        }
        #endregion

        #region Mainform Events
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            #region Serialize
            Utilities.Serialize("UserPref.xml", Player);
            #endregion

            // Stop Running
            if (Session != null && Session.Player.GetLoginStatus == LoginStatus.LoggedIn)
                Session.Navigator.Reset();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region Deserialize and Initialize Gui
            // Deserialize
            var Temp = Utilities.Deserialize("UserPref.xml", Player);

            Player.TransferDeserializedFields(Temp);

            // Waypoints
            foreach (var point in Player.Pathing.Waypoints)
                lstWaypoints.Items.Add("X:" + point.X + ", Z:" + point.Z);
            // Targets
            foreach (var monster in Player.Units.TargetNames)
                cboTargets.Items.Add(monster);
            // Ignored
            foreach (var monster in Player.Units.IgnoredMobs)
                cboIgnoreList.Items.Add(monster);
            // Events
            foreach (var action in Player.StartingResponses)
                lstAbilities.Items.Add(action.Name);
            // Healing
            foreach (var action in Player.HealingResponses)
                lstAbilities.Items.Add(action.Name);
            // Monster Options
            chkKillAggro.IsChecked = Player.KillAggro;
            chkKillPartyClaimed.IsChecked = Player.KillPartyClaimed;
            chkKillUnclaimed.IsChecked = Player.KillUnclaimed;
            // Weaponskills
            txbWeaponskill.Text = Player.Weaponskill.Name;
            sldDistance.Value = Player.Weaponskill.MaxDistance;
            sldHealthThreshold.Value = Player.WeaponSkillHP;
            // Resting
            txbStandUp.Text = Player.StandUPHPPValue.ToString();
            txbSitDown.Text = Player.SitdownHPPValue.ToString();
            #endregion

            // Stop Running
            if (Session != null && Session.Player.GetLoginStatus == LoginStatus.LoggedIn)
                Session.Navigator.Reset();
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

        private void PrintRecoveryPath()
        {
            lstRecoverPath.Items.Clear();
            var KeysString = new StringBuilder("Keys: ");

            foreach (var Point in Player.RecoveryPath.Path)
            {
                KeysString.Clear();

                foreach (var k in Point.Keys)
                {
                    KeysString.Append(k.ToString() + " ");
                }

                lstRecoverPath.Items.Add("X: " + Point.Position.X + "Y: " +
                    Point.Position.Y + "Z: " + Point.Position.Z + "H: " + Point.Position.H);
                lstRecoverPath.Items.Add(KeysString.ToString());
                lstRecoverPath.Items.Add("Target: " + Point.Target);
            }
        }

        private void AddKey(KeyCode pressedKey)
        {
            if (RecoveryPathTimer.IsEnabled)
                Player.RecoveryPath.Path.Last().Keys.Add(pressedKey);
        }

        private void SetGridVisibility(Grid passedControl)
        {
            if (passedControl != null)
            {
                foreach (Grid Grid in stpGrids.Children)
                {
                    if (Grid.Equals(passedControl))
                        Grid.Visibility = Visibility.Visible;
                    else
                        Grid.Visibility = Visibility.Collapsed;
                }
            }
        }
        #endregion

        #region Expander Events
        private void expIgnoredList_Expanded(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdIgnored);
        }

        private void expTargetsList_Expanded(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdTargets);
        }

        private void expResting_Expanded(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdResting);
        }

        private void expHealing_Expanded(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdHealing);
        }

        private void expEvents_Expanded(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdEvents);
        }

        private void expWeaponSkills_Expanded(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdWeapon);
        }

        private void expWaypoints_Expanded(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdRoute);
        }

        private void expRecovery_Expanded(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdRecovery);
        }

        private void expHealing_Collapsed(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdHealing);
        }

        private void expResting_Collapsed(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdResting);
        }

        private void expTargetsList_Collapsed(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdTargets);
        }

        private void expIgnoredList_Collapsed(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdIgnored);
        }

        private void expRecovery_Collapsed(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdRecovery);
        }

        private void expWaypoints_Collapsed(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdRoute);
        }

        private void expEvents_Collapsed(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdEvents);
        }

        private void expWeaponSkills_Collapsed(object sender, RoutedEventArgs e)
        {
            SetGridVisibility(grdWeapon);
        }
        #endregion

        #region Timer Events
        private void ArrowRestocker_Tick(object sender, EventArgs e)
        {
            int ArrowID = Utilities.GetItemIDs("Iron Arrow").Keys.First();
            int PouchID = Utilities.GetItemIDs("Iron Quiver").Keys.First();

            if (Session.Item.GetEquippedItemCount(EquipSlot.Ammo) <= 0)
            {
                if (Session.Item.GetItemCount(ArrowID, InventoryType.Inventory) > 0)
                {
                    Session.Windower.SendString("/equip ammo \"Iron Arrow\"");
                    System.Threading.Thread.Sleep(1000);
                }
                else if (Session.Item.GetItemCount(PouchID, InventoryType.Inventory) > 0)
                {
                    Session.Windower.SendString("/item \"Iron Quiver\" <me>");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
        #endregion

        ////////////////////////////////////////////////////////////////////
    }
}
