using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Interfaces
{
    public interface IProfile
    {
        CastType CastType { get; set; }
        double MinDistance { get; set; }
        double MaxDistance { get; set; }
    }

    public enum CastType
    {
        Mobile,
        Stationary
    }
}
