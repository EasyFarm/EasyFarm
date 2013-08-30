using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.Interfaces;

namespace EasyFarm.Profiles
{
    class CasterProfile : IProfile
    {
        public CasterProfile()
        {
            this.CastType = Interfaces.CastType.Stationary;
            this.RestType = Interfaces.RestType.Mixed;
            this.MinDistance = 0;
            this.MaxDistance = 21;
        }

        public CastType CastType { get; set; }

        public RestType RestType { get; set; }

        public double MinDistance { get; set; }

        public double MaxDistance { get; set; }
    }
}
