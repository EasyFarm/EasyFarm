using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.ViewModels
{
    public class DesignViewModel
    {
        public DesignViewModel()
        {
            this.BattlesData = new BattlesViewModel()
            {
                VMName = "Battle",
                ActionName = "Provoke",
                PullSelected = true,
                PullList = new ObservableCollection<Ability> { 
                    new Ability()
                    { 
                        Name = "Provoke",
                        Postfix = "<t>",
                        Prefix = "/jobability"
                    }
                }
            };

            this.HealingData = new HealingViewModel();
            this.IgnoredData = new IgnoredViewModel();
            this.LogData = new LogViewModel()
            {
                VMName = "Log"
                // LogItems = new ObservableCollection<string>() { "Easyfarm has started."}
            };
            this.RestingData = new RestingViewModel();
            this.RoutesData = new RoutesViewModel();
            this.TargetsData = new TargetsViewModel();
            this.WeaponsData = new WeaponsViewModel();
        }

        public BattlesViewModel BattlesData { get; set; }
        public HealingViewModel HealingData { get; set; }
        public IgnoredViewModel IgnoredData { get; set; }
        public LogViewModel LogData { get; set; }
        public RestingViewModel RestingData { get; set; }
        public RoutesViewModel RoutesData { get; set; }
        public TargetsViewModel TargetsData { get; set; }
        public WeaponsViewModel WeaponsData { get; set; }
    }
}
