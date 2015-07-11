using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EasyFarm.DataContexts
{
    public class Battle
    {
        public int Id { get; set; }

        public double Distance { get; set; }
        public double Height { get; set; }
        public double Wander { get; set; }

        public Player Player { get; set; }
    }
}
