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

using System.Windows;
using System.Windows.Controls;
using EasyFarm.Classes;
using EasyFarm.ViewModels;

namespace EasyFarm.Views
{
    /// <summary>
    ///     Interaction logic for BattlesView.xaml
    /// </summary>
    public partial class BattlesView
    {
        public BattlesView()
        {
            InitializeComponent();
        }

        private void Master_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var model = DataContext as BattlesViewModel;
            if (model == null) return;

            model.SelectedAbility = e.NewValue as BattleAbility;
            model.SelectedList = e.NewValue as BattleList;

            Details.DataContext = (e.NewValue as BattleAbility);
        }        
    }
}