using EasyFarm.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyFarm.Views
{
    /// <summary>
    /// Interaction logic for AbilitySelectionBox.xaml
    /// </summary>
    public partial class AbilitySelectionBox : Window
    {
        public Ability SelectedAbility { get; set; }

        public AbilitySelectionBox(String name)
        {
            InitializeComponent();
            this.CompleteSelectionButton.Click += CompleteSelectionButton_Click;
            this.AbilityListBox.ItemsSource = new AbilityService().GetAbilitiesWithName(name);
            this.ShowDialog();
        }

        void CompleteSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedAbility = AbilityListBox.SelectedValue as Ability;
            this.Close();
        }
    }
}
