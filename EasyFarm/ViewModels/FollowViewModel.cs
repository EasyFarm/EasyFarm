using EasyFarm.Classes;
using Prism.Mvvm;
using EasyFarm.Infrastructure;

namespace EasyFarm.ViewModels
{
    public class FollowViewModel : BindableBase, IViewModel
    {
        public string ViewName { get; set; }

        public FollowViewModel()
        {
            ViewName = "Follow";
        }

        public string Name
        {
            get { return Config.Instance.FollowedPlayer; }
            set
            {
                Config.Instance.FollowedPlayer = value;
                AppServices.InformUser("Now following {0}.", value);
            }
        }

        public double Distance
        {
            get { return Config.Instance.FollowDistance; }
            set
            {
                SetProperty(ref Config.Instance.FollowDistance, value);
                AppServices.InformUser(string.Format("Follow Distance: {0}.", value));
            }
        }
    }
}
