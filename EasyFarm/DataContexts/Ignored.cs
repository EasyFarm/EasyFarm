using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EasyFarm.DataContexts
{
    public class Ignored
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public Player Player { get; set; }
    }
}
