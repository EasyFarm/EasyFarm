using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace EasyFarm.DataContexts
{
    public class Targeted
    {
        public int Id { get; set; }
                
        public string Name { get; set; }

        public Player Player { get; set;}
    }
}
