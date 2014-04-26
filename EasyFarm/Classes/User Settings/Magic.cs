using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    public class Magic
    {
        public bool Enabled = false;
        public int High = 100;
        public int Low = 50;

        public bool ShouldRest(int magic, Status status)
        {
            return (Enabled && (IsMagicLow(magic) || IsMagicHigh(magic, status) && status == Status.Healing));
        }

        public bool IsMagicLow(int magic)
        {
            return magic <= Low;
        }

        public bool IsMagicHigh(int magic, Status status)
        {
            return magic < High;
        }
    }
}
