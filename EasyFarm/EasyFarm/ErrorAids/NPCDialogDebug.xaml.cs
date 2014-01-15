using System;
using System.Windows;
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
        FFACE.MenuTools Data;
        DispatcherTimer ChatlogUpdater = new DispatcherTimer();

        public NPCDialogDebug()
        {

        }

        public NPCDialogDebug(FFACETools.FFACE session)
        {
            InitializeComponent();
            this.Session = session;
            this.Data = this.Session.Menu;

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
            DialogListbox.Items.Add("Dialog ID: " + Data.DialogID);
            DialogListbox.Items.Add("Dialog Option Count: " + Data.DialogOptionCount);
            DialogListbox.Items.Add("Dialog Option Index: " + Data.DialogOptionIndex);            
            DialogListbox.Items.Add("Dialog Text Question: " + Data.DialogText.Question.ToString());
            DialogListbox.Items.Add("Dialog Text Options: ");
            foreach (var i in Data.DialogText.Options)
                DialogListbox.Items.Add(i);
            DialogListbox.Items.Add("Help: " + Data.Help);
            DialogListbox.Items.Add("IsOpen: " + Data.IsOpen);
            DialogListbox.Items.Add("Last Trade Menu Status: " + Data.lastTradeMenuStatus);
            DialogListbox.Items.Add("Name: " + Data.Name);
            DialogListbox.Items.Add("Selection: " + Data.Selection);
            DialogListbox.Items.Add("Shop Quantity: " + Data.ShopQuantity);
            DialogListbox.Items.Add("Shop Quantity Max: " + Data.ShopQuantityMax);
            DialogListbox.Items.Add("Menu Index: " + Data.MenuIndex);
        }
    }
}
