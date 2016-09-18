using EasyFarm.ViewModels;

namespace EasyFarm.Views
{
    /// <summary>
    /// Interaction logic for TargetingView.xaml
    /// </summary>
    public partial class TargetingView
    {
        public TargetingView()
        {
            InitializeComponent();
            DataContext = new TargetingViewModel();
        }
    }
}
