using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class WeaponInfo
    {
        public WeaponInfo()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            this.WeaponSkill = new WeaponAbility("");
            HealthThreshold = 0;
            Name = "";
            Distance = 0;
        }

        /// <summary>
        /// The weaponskill that should be used when we reach 100% tp
        /// </summary>
        public WeaponAbility WeaponSkill { get; set; }

        /// <summary>
        /// Tells us when to use the weaponskill when the mob's hp reaches this level
        /// </summary>
        public int HealthThreshold { get; set; }

        /// <summary>
        /// The name of the weaponskill that will be created
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The max distance the weaponskill should be used at
        /// </summary>
        public double Distance { get; set; }
    }
}
