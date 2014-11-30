using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.ViewModels
{
    public class ViewModelAttribute : Attribute
    {
        public String Name { get; set; }

        public bool Enabled { get; set; }

        public ViewModelAttribute(String name, bool enabled = true)
        {
            this.Enabled = enabled;
            this.Name = name;
        }
    }
}
