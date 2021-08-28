using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MemoryAPI.Navigation;

namespace EasyFarm.Navigation
{
    public static class NavUtils
    {
        public static bool IsBetween(Position A, Position B, Position C)
        {
            return Position.Dot((B - A).Normalized, (C - B).Normalized) < 0f && Position.Dot((A - B).Normalized, (C - A).Normalized) < 0f;
        }
    }
}
