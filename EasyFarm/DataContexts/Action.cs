using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.DataContexts
{
    public class Action
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Player Player { get; set; }
    }
}
