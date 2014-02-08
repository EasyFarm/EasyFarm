using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class BaseModel : ObservableObject
    {
        public Dictionary<String, Object> Objects = new Dictionary<string, object>();

        public T Get<T>(String name)
        { 
            return (T) Objects[name] == null ? default(T) : (T)Objects[name];
        }

        public void Set<T>(String name, T value)
        {
            Objects[name] = value;
            RaisePropertyChanged(name);
        }
    }
}
