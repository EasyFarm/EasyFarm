﻿
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

using EasyFarm.UserSettings;
using Microsoft.Practices.Prism.Commands;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.ViewModels
{
    [ViewModelAttribute("Units", false)]
    public class UnitsViewModel : ViewModelBase
    {
        public ICollectionView Units { get; private set; }

        public UnitsViewModel()
        {
            Units = new ListCollectionView(FTools.UnitService.FilteredArray.ToList());
            Units.CurrentChanged += Units_CurrentChanged;

            AddTargetCommand = new DelegateCommand(AddToTargets);
        }

        private void AddToTargets()
        {
            var unit = Units.CurrentItem as Unit;
            if (unit == null)
            {
                return;
            }

            var name = unit.Name;
            if (!Config.Instance.FilterInfo.TargetedMobs.Contains(name))
            {
                Config.Instance.FilterInfo.TargetedMobs.Add(name);
            }
        }

        void Units_CurrentChanged(object sender, EventArgs e)
        {
        }

        public ICommand AddTargetCommand { get; set; }
    }
}
