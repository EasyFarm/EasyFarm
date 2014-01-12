using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using EasyFarm.PlayerTools;
using EasyFarm.UnitTools;

namespace EasyFarm
{
    /// <summary>
    /// Interaction logic for DebugCreatures.xaml
    /// </summary>
    public partial class DebugCreatures : Window
    {
        DispatcherTimer Timer = new DispatcherTimer();
        Units units;
        Player player;
        
        public DebugCreatures(Units units, Player player)
        {
            InitializeComponent();
            this.units = units;
            this.player = player;

            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 1);
            Timer.Start();
            this.Show();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var mob in units.ValidMobs)
            {
                if (!lstMobNames.Items.Contains(mob.Name + ":" + mob.ID) && mob.Name!="")
                {
                    lstMobNames.Items.Add("{0}:{1}"
                        .Replace("{0}", mob.Name)
                        .Replace("{1}", mob.ID.ToString()));
                }
            }
        }

        private void lstMobNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstMobData.Items.Clear();
            Unit Mob = Unit.CreateUnit(0);

            var Query = from i in units.ValidMobs
                        let SplitSelectedItem = lstMobNames.SelectedItem.ToString().Split(':')
                        where i.Name == SplitSelectedItem[0] && i.ID.ToString() == SplitSelectedItem[1]
                        select i;

            if (Query.Count() > 0)
                Mob = Query.First();

            lstMobData.Items.Add("Name: " + Mob.Name);
            lstMobData.Items.Add("IsActive: " + Mob.IsActive);
            lstMobData.Items.Add("ID: " + Mob.ID);
            lstMobData.Items.Add("Claimed ID: " + Mob.ClaimedID);
            lstMobData.Items.Add("NPCBit: " + Mob.NPCBit);
            lstMobData.Items.Add("NPCType: " + Mob.NPCType);
            lstMobData.Items.Add("Status: " + Mob.Status);
            lstMobData.Items.Add("HPPCurrent: " + Mob.HPPCurrent);
            lstMobData.Items.Add("Distance: " + Mob.Distance);
            lstMobData.Items.Add("IsDead: " + Mob.IsDead);
            lstMobData.Items.Add("HasAggroed: " + Mob.HasAggroed);
            lstMobData.Items.Add("MyClaim: " + Mob.MyClaim);
            lstMobData.Items.Add( "PartyClaim: " + Mob.PartyClaim);
            lstMobData.Items.Add("IsClaimed: " + Mob.IsClaimed);
            lstMobData.Items.Add("PetID: " + Mob.PetID);
            lstMobData.Items.Add("Position: " + Mob.Position);
            lstMobData.Items.Add("PosH: " + Mob.PosH);
            lstMobData.Items.Add("PosX: " + Mob.PosX);
            lstMobData.Items.Add("PosY: " + Mob.PosY);
            lstMobData.Items.Add("PosZ: " + Mob.PosZ);
            lstMobData.Items.Add("TPCurrent: " + Mob.TPCurrent);
        }
    }
}
