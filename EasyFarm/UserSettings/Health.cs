using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.UserSettings
{
    public class Health
    {
        public bool Enabled = false;
        public int High = 100;
        public int Low = 50;

        public bool ShouldRest(int health, Status status)
        {
            // Rest while low and while not high
            return (Enabled && (IsHealthLow(health) || !IsHealthHigh(health) && status == Status.Healing));
        }

        public bool IsHealthLow(int health)
        {
            return health <= Low;
        }

        public bool IsHealthHigh(int health)
        {
            return health >= High;
        }
    }
}
