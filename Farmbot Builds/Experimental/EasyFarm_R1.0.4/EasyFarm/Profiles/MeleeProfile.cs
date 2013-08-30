using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.Interfaces;

namespace EasyFarm.Profiles
{
    class MeleeProfile : IProfile
    {
        public MeleeProfile()
        {
            this.CastType = Interfaces.CastType.Mobile;
            this.RestType = Interfaces.RestType.HP;
            this.MinDistance = 0;
            this.MaxDistance = 3.5;
        }

        public CastType CastType { get; set;}

        public RestType RestType { get; set; }

        public double MinDistance { get; set; }

        public double MaxDistance { get; set; }
    }
}
