using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.MVVM
{
    public class AbilityListItem<T> : ViewModelBase
    {
        private T item;

        public AbilityListItem() { }

        public AbilityListItem(T item)
        {
            this.item = item;
        }

        public T Item
        {
            get { return item; }
            set {
                item = value;
                OnPropertyChanged("Item");
            }
        }
    }
}
