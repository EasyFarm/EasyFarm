using EasyFarm.ViewModels;

namespace EasyFarm.Views
{
    /// <summary>
    ///     Interaction logic for ProcessSelectionView.xaml
    /// </summary>
    public partial class ProcessSelectionView
    {
        public ProcessSelectionView()
        {
            InitializeComponent();
            DataContext = new ProcessSelectionViewModel(this);
        }
    }
}