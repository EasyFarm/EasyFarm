using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class Sequence : Behavior
    {
        Queue<Behavior> Behaviors;

        public Sequence()
        {
            Behaviors = new Queue<Behavior>();
        }

        protected override TerminationStatus Execute()
        {
            throw new NotImplementedException();
        }

        protected override bool CanExecute()
        {
            throw new NotImplementedException();
        }
    }
}
