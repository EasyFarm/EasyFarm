/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using System.Linq;
using System.Windows;
using EasyFarm.Classes;
using EasyFarm.Parsing;

namespace EasyFarm.Views
{
    /// <summary>
    ///     Interaction logic for AbilitySelectionBox.xaml
    /// </summary>
    public partial class AbilitySelectionBox
    {
        public AbilitySelectionBox(string name)
        {
            InitializeComponent();
            CompleteSelectionButton.Click += CompleteSelectionButton_Click;
            AbilityListBox.ItemsSource = App.AbilityService.GetAbilitiesWithName(name).Select(x => new BattleAbility(x));
            ShowDialog();
        }

        public Ability SelectedAbility { get; set; }

        private void CompleteSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedAbility = ((BattleAbility)AbilityListBox.SelectedValue).Ability;
            Close();
        }
    }
}