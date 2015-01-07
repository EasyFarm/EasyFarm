using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Internal list of view models. 
        /// </summary>
        private ObservableCollection<ViewModelBase> _viewModels;

        /// <summary>
        /// List of dynamically found view models. 
        /// </summary>
        public ObservableCollection<ViewModelBase> ViewModels
        {
            get { return _viewModels; }
            set
            {
                SetProperty(ref _viewModels, value);
            }
        }

        /// <summary>
        /// Interal stating index for the currently focused tab.
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// Index for the currently focused tab. 
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        public MainViewModel()
        {
            var locator = new Locator<ViewModelAttribute, ViewModelBase>();

            // Get all enabled view models. 
            ViewModels = new ObservableCollection<ViewModelBase>(
                locator.GetEnabledViewModels()
                .Where(x => x != null)
                .OrderBy(x => x.VMName));
        }
    }
}
