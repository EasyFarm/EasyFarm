using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
