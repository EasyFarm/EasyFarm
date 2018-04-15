// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using EasyFarm.Parsing;
using MahApps.Metro.Controls.Dialogs;

namespace EasyFarm.Views
{
    /// <summary>
    ///     Interaction logic for SelectActionDialog.xaml
    /// </summary>
    public partial class SelectActionDialog
    {
        public SelectActionDialog(IList<Ability> abilities)
        {
            InitializeComponent();
            CompleteSelectionButton.Click += async (s, e) => await CompleteSelectionButton_Click(s, e);
            AbilityListBox.ItemsSource = abilities;
        }

        public Ability SelectedAbility { get; set; }

        private async Task CompleteSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedAbility = (Ability)AbilityListBox.SelectedValue;
            await DialogCoordinator.Instance.HideMetroDialogAsync(Application.Current.MainWindow.DataContext, this);
        }
    }
}