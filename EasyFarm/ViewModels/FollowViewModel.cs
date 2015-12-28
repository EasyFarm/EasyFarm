using EasyFarm.Classes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFarm.Infrastructure;

namespace EasyFarm.ViewModels
{
    [ViewModel("Follow")]
    public class FollowViewModel : BindableBase, IViewModel
    {
        public string ViewName { get; set; }

        public string Name
        {
            get { return Config.Instance.FollowedPlayer; }
            set
            {
                Config.Instance.FollowedPlayer = value;
                EventPublisher.InformUser("Now following {0}.", value);
            }
        }

        public double Distance
        {
            get { return Config.Instance.FollowDistance; }
            set
            {
                SetProperty(ref Config.Instance.FollowDistance, value);
                EventPublisher.InformUser(string.Format("Follow Distance: {0}.", value));
            }
        }
    }
}
