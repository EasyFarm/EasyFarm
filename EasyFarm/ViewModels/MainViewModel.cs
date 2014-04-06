using EasyFarm.Classes;
using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EasyFarm.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel(ref GameEngine Engine)
            : base(ref Engine)
        {
            StatusBarText = "Bot Loaded: " + Engine.FFInstance.Instance.Player.Name;
            StartCommand = new RelayCommand(Start);
        }

        public String StatusBarText
        {
            get { return GameEngine.Config.StatusBarText; }
            set
            {
                GameEngine.Config.StatusBarText = value;
                this.RaisePropertyChanged("StatusBarText");
            }
        }

        /*
        public String CategoryDescription
        {
            get { return this.Get<String>("CategoryDescription"); }
            set { this.Set<String>("CategoryDescription", value); }
        }
        */

        /*
        public String CategoryName
        {
            get { return this.Get<String>("CategoryName"); }
            set { this.Set<String>("CategoryName", value); }
        }
        */

        public ICommand StartCommand { get; set; }

        public void Start()
        {
            if (GameEngine.IsWorking)
            {
                StatusBarText = "Paused";
                GameEngine.Stop();
            }
            else
            {
                StatusBarText = "Running";
                GameEngine.Start();
            }
        }
    }
}
