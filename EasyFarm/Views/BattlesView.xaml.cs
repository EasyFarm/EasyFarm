
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
using EasyFarm.ViewModels;
using System.Windows.Controls;

namespace EasyFarm.Views
{
    /// <summary>
    /// Interaction logic for BattlesView.xaml
    /// </summary>
    public partial class BattlesView : UserControl
    {
        public BattlesView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// We are using custom code to assign a selected item since TreeView's selected item property is read-only. 
        /// There were a few solutions to this problem, but this is the simplest.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext != null)
            {
                // Update to the new selected list. 
                ((BattlesViewModel)DataContext).SelectedList = (e.NewValue as BattleList);

                // Update to the new selected ability. 
                ((BattlesViewModel)DataContext).SelectedAbility = (e.NewValue as BattleAbility);
                details.DataContext = (e.NewValue as BattleAbility);
            }
        }
    }
}
