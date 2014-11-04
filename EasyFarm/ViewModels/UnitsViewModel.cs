using EasyFarm.UserSettings;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.ViewModels
{
    public class UnitsViewModel : ViewModelBase
    {
        public ICollectionView Units { get; private set; }

        public UnitsViewModel()
        {
            Units = new ListCollectionView(
                ViewModelBase.FarmingTools.UnitService.FilteredArray.ToList());
            Units.CurrentChanged += Units_CurrentChanged;

            AddTargetCommand = new DelegateCommand(AddToTargets);
        }

        private void AddToTargets()
        {
            var name = (Units.CurrentItem as Unit).Name;
            if(!Config.Instance.FilterInfo.TargetedMobs.Contains(name))
            Config.Instance.FilterInfo.TargetedMobs.Add(name);
        }

        void Units_CurrentChanged(object sender, EventArgs e)
        {
            Unit unit = Units.CurrentItem as Unit;
        }

        public ICommand AddTargetCommand { get; set; }
    }
}
