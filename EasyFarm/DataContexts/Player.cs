using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EasyFarm.DataContexts
{
    public class Player
    {
        public Player()
        {
            this.Health = new Health();
            this.Magic = new Magic();
            this.Battle = new Battle();
            this.Targeted = new List<Targeted>();
            this.Ignored = new List<Ignored>();
            this.Actions = new List<Action>();
        }
        
        public int Id { get; set; }

        public Health Health { get; set; }
        public Magic Magic { get; set; }
        public Battle Battle { get; set; }

        public List<Targeted> Targeted { get; set; }
        public List<Ignored> Ignored { get; set; }
        public List<Action> Actions { get; set; }
    }
}
