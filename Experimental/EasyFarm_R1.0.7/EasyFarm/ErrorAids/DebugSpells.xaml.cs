using System;
using System.Windows;
using FFACETools;
using System.Windows.Forms;

namespace EasyFarm
{
    /// <summary>
    /// Interaction logic for DebugActions.xaml
    /// </summary>
    public partial class DebugSpells : Window
    {
        FFACE Session = null;
        FFACE.PlayerTools Data;

        public DebugSpells(FFACE Session)
        {
            this.Session = Session;
            this.Data = Session.Player;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create and start timer
            Timer timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 30;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            OutputTextBox.Text =
                ("CastCountDown: " + Data.CastCountDown +
                "\nCastMax: " + Data.CastMax +
                "\nCastPercent: " + Data.CastPercent +
                "\nCastPercentEx: " + Data.CastPercentEx +
                "\nMPCurrent: " + Data.MPCurrent +
                "\nMPMax: " + Data.MPMax +
                "\nMPPCurrent: " + Data.MPPCurrent);
        }
    }
}
