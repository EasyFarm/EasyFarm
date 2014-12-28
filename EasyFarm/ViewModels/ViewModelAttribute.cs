using System;

namespace EasyFarm.ViewModels
{
    public class ViewModelAttribute : Attribute
    {
        public String Name { get; set; }

        public bool Enabled { get; set; }

        public ViewModelAttribute(String name, bool enabled = true)
        {
            Enabled = enabled;
            Name = name;
        }
    }
}
