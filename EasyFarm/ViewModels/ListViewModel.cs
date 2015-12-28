using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EasyFarm.Infrastructure;

namespace EasyFarm.ViewModels
{
    /// <summary>
    /// View model for list controls.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ListViewModel<T> : ViewModelBase
    {
        /// <summary>
        /// The current value to be added (Textboxes) or has been selected (ListControls)
        /// </summary>
        public virtual T Value { get; set; }

        /// <summary>
        /// The values of the list control.
        /// </summary>
        public virtual ObservableCollection<T> Values { get; set; }

        public ListViewModel()
        {
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand(Delete);
            ClearCommand = new DelegateCommand(Clear);
        }

        /// <summary>
        /// Add command for view binding.
        /// </summary>
        public ICommand AddCommand { get; set; }

        /// <summary>
        /// Delete command for view binding.
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// Clear command for view binding .
        /// </summary>
        public ICommand ClearCommand { get; set; }

        /// <summary>
        /// Clear the items in the list control.
        /// </summary>
        protected virtual void Clear()
        {
            Values.Clear();
        }

        /// <summary>
        /// Delete existing items in the list control.
        /// </summary>
        protected virtual void Delete()
        {
            if (!Values.Contains(Value)) return;
            Values.Remove(Value);
        }

        /// <summary>
        /// Add a new value to the list control.
        /// </summary>
        protected virtual void Add()
        {
            if (Values.Contains(Value)) return;
            Values.Add(Value);
        }
    }
}