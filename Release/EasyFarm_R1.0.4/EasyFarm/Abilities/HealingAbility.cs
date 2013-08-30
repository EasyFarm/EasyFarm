using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.PlayerTools
{
    public class Healing : Ability
    {
        public Healing() { }
        public Healing(string name, int threshold = 0): base(name) 
        {
            HPThreshold = threshold;
        }
        public int HPThreshold;
        public bool IsEnabled;
    }
}
