
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

using EasyFarm.Classes;
using Parsing.Abilities;
using System;
using System.Windows;


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
            this.AbilityListBox.ItemsSource = App.AbilityService.GetAbilitiesWithName(name);
            this.ShowDialog();
        }

        void CompleteSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedAbility = AbilityListBox.SelectedValue as Ability;
            this.Close();
        }
    }
}
