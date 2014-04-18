using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.ViewModels
{
    class Waypoint<T>
    {
        public FFACE.Position Position { get; set; }

        public override string ToString()
        {
            return "X: " + Position.X + "Z: " + Position.Z;
        }
    }
}
