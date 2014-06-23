using EasyFarm.Classes;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.ViewModels
{
    public class UnitFilteringViewModel : ViewModelBase
    {
        public UnitFilteringViewModel(ref GameEngine Engine, IEventAggregator eventAggregator)
            : base(ref Engine, eventAggregator) { }

        public double DetectionDistance
        {
            get { return _engine.UserSettings.MiscSettings.DetectionDistance; }
            set 
            { 
                SetProperty<double>(ref _engine.UserSettings.MiscSettings.DetectionDistance, value);
                InformUser("Detection Distance Set: {0}.", value);
            }
        }

        public double HeightThreshold
        {
            get { return _engine.UserSettings.MiscSettings.HeightThreshold; }
            set
            {
                SetProperty<double>(ref _engine.UserSettings.MiscSettings.HeightThreshold, value);
                InformUser("Height Threshold Set: {0}.", value);
            }
        }

        public double MaxMeleeDistance
        {
            get { return _engine.UserSettings.MiscSettings.MaxMeleeDistance; }
            set
            {
                SetProperty<double>(ref _engine.UserSettings.MiscSettings.MaxMeleeDistance, value);
                InformUser("Max Melee Distance Set: {0}.", value);
            }
        }

        public double MinMeleeDistance
        {
            get { return _engine.UserSettings.MiscSettings.MinMeleeDistance; }
            set
            {
                SetProperty<double>(ref _engine.UserSettings.MiscSettings.MinMeleeDistance, value);
                InformUser("Min Melee Distance Set: {0}.", value);
            }
        }
    }
}
