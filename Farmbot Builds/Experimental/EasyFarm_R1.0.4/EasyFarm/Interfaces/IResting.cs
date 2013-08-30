using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Interfaces
{
    public interface IResting
    {
        void Rest();
    }

    public enum RestType
    {
        HP,
        MP,
        Mixed
    }
}
