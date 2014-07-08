using EasyFarm.Classes;
using EasyFarm.Classes.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    public class UnitFilteringViewModel : ViewModelBase
    {
        public UnitFilteringViewModel(FarmingTools farmingTools) : base(farmingTools) 
        {
            RestoreDefaultsCommand = new DelegateCommand(RestoreDefaults);
        }

        public ICommand RestoreDefaultsCommand { get; set; }
        
        private void RestoreDefaults()
        {
            DetectionDistance = 17;
            HeightThreshold = 5;
            MinMeleeDistance = 3;
            MaxMeleeDistance = 5;
            App.InformUser("Defaults have been restored.");
        }

        public double DetectionDistance
        {
            get { return farmingTools.UserSettings.MiscSettings.DetectionDistance; }
            set 
            { 
                SetProperty<double>(ref farmingTools.UserSettings.MiscSettings.DetectionDistance, value);
                App.InformUser("Detection Distance Set: {0}.", value);
            }
        }

        public double HeightThreshold
        {
            get { return farmingTools.UserSettings.MiscSettings.HeightThreshold; }
            set
            {
                SetProperty<double>(ref farmingTools.UserSettings.MiscSettings.HeightThreshold, value);
                App.InformUser("Height Threshold Set: {0}.", value);
            }
        }

        public double MaxMeleeDistance
        {
            get { return farmingTools.UserSettings.MiscSettings.MaxMeleeDistance; }
            set
            {
                SetProperty<double>(ref farmingTools.UserSettings.MiscSettings.MaxMeleeDistance, value);
                App.InformUser("Max Melee Distance Set: {0}.", value);
            }
        }

        public double MinMeleeDistance
        {
            get { return farmingTools.UserSettings.MiscSettings.MinMeleeDistance; }
            set
            {
                SetProperty<double>(ref farmingTools.UserSettings.MiscSettings.MinMeleeDistance, value);
                App.InformUser("Min Melee Distance Set: {0}.", value);
            }
        }
    }
}
