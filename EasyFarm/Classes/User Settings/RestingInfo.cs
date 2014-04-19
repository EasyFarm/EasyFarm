using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class RestingInfo
    {
        public RestingInfo()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            LowHP = 0;
            LowMP = 0;
            HighHP = 0;
            HighMP = 0;
            IsRestingHPEnabled = false;
            IsRestingMPEnabled = false;
        }

        /// <summary>
        /// The value we should rest mp at
        /// </summary>
        public int LowMP { get; set; }

        /// <summary>
        /// The value we can get up from resting mp
        /// </summary>
        public int HighMP { get; set; }

        /// <summary>
        /// The value we can get up from resting hp
        /// </summary>
        public int HighHP { get; set; }

        /// <summary>
        /// The value we should rest hp at.
        /// </summary>
        public int LowHP { get; set; }

        /// <summary>
        /// Should we rest hp at all?
        /// </summary>
        public bool IsRestingHPEnabled { get; set; }

        /// <summary>
        /// Should we rest mp at all?
        /// </summary>
        public bool IsRestingMPEnabled { get; set; }
    }
}
