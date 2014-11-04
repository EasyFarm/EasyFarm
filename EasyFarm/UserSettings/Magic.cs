using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.UserSettings
{
    public class Magic
    {
        public bool Enabled = false;
        public int High = 100;
        public int Low = 50;

        public bool ShouldRest(int magic, Status status)
        {
            return (Enabled && (IsMagicLow(magic) || !IsMagicHigh(magic) && status == Status.Healing));
        }

        public bool IsMagicLow(int magic)
        {
            return magic <= Low;
        }

        public bool IsMagicHigh(int magic)
        {
            return magic >= High;
        }
    }
}
