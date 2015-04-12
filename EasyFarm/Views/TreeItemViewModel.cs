using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.ViewModels
{
    /// <summary>
    /// Models named tree view items with a value. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeItemViewModel<T> : BindableBase
    {
        /// <summary>
        /// Internal backing for the tree view item name. 
        /// </summary>
        private string _name;

        /// <summary>
        /// Private internal backing for the tree view item's content. 
        /// </summary>
        private T _value;        

        /// <summary>
        /// Set tree view item data to the given values. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public TreeItemViewModel(string name, T value)
        {
            _name = name;
            _value = value;
        }

        /// <summary>
        /// The displayed tree view item name. 
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// The content of the tree view item. 
        /// </summary>
        public T Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
    }
}
