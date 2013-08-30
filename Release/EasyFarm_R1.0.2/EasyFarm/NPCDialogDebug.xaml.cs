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
using System.Windows.Threading;

namespace EasyFarm
{
    /// <summary>
    /// Interaction logic for NPCDialogDebug.xaml
    /// </summary>
    public partial class NPCDialogDebug : Window
    {
        FFACETools.FFACE Session;
        DispatcherTimer ChatlogUpdater = new DispatcherTimer();

        public NPCDialogDebug(FFACETools.FFACE session)
        {
            InitializeComponent();
            this.Session = session;            
            ChatlogUpdater.Tick += new EventHandler(ChatlogUpdater_Tick);
            ChatlogUpdater.Interval = new TimeSpan(0, 0, 0, 0, 500);
            ChatlogUpdater.Start();
        }

        void ChatlogUpdater_Tick(object sender, EventArgs e)
        {
            var nextLine = new FFACE.ChatTools.ChatLine();
            while ((nextLine = Session.Chat.GetNextLine()) != null)
                    DialogListbox.Items.Add(nextLine.Text.ToString());        
        }

        private void DisplayDebugInfo_Click(object sender, RoutedEventArgs e)
        {
            DialogListbox.Items.Clear();
            DialogListbox.Items.Add("Dialog ID: " + Session.Menu.DialogID);
            DialogListbox.Items.Add("Dialog Option Count: " + Session.Menu.DialogOptionCount);
            DialogListbox.Items.Add("Dialog Option Index: " + Session.Menu.DialogOptionIndex);            
            DialogListbox.Items.Add("Dialog Text Question: " + Session.Menu.DialogText.Question.ToString());
            DialogListbox.Items.Add("Dialog Text Options: ");
            foreach (var i in Session.Menu.DialogText.Options)
                DialogListbox.Items.Add(i);
            DialogListbox.Items.Add("Help: " + Session.Menu.Help);
            DialogListbox.Items.Add("IsOpen: " + Session.Menu.IsOpen);
            DialogListbox.Items.Add("Last Trade Menu Status: " + Session.Menu.lastTradeMenuStatus);
            DialogListbox.Items.Add("Name: " + Session.Menu.Name);
            DialogListbox.Items.Add("Selection: " + Session.Menu.Selection);
            DialogListbox.Items.Add("Shop Quantity: " + Session.Menu.ShopQuantity);
            DialogListbox.Items.Add("Shop Quantity Max: " + Session.Menu.ShopQuantityMax);            
            DialogListbox.Items.Add("Get Next Line: ");
            //var nextLine = Session.Chat.GetNextLine(LineSettings.CleanAndConvertAll);
        }
    }
}
