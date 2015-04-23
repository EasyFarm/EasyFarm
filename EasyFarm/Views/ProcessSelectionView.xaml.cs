using System.Windows;
using EasyFarm.ViewModels;

namespace EasyFarm.Views
{
    /// <summary>
    ///     Interaction logic for ProcessSelectionView.xaml
    /// </summary>
    public partial class ProcessSelectionView : Window
    {
        public ProcessSelectionView()
        {
            InitializeComponent();
            DataContext = new ProcessSelectionViewModel();
        }
    }
}