using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.MVVM
{
    public class ListItem<T> : ObservableObject
    {
        private T item;

        public ListItem() { }

        public ListItem(T item)
        {
            this.item = item;
        }

        public T Item 
        {             
            get {  return Item;  }
            
            set {  item = value;  RaisePropertyChanged("Item");  }
        }
    }
}
