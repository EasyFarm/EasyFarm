using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.DataContexts
{
    public class Health
    {
        public int Id { get; set; }

        public bool Enabled { get; set; }
        public int High { get; set; }
        public int Low { get; set; }

        public Player Player { get; set; }
    }
}
