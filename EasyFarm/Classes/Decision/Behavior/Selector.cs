using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    public abstract class Selector : Behavior
    {
        public List<Behavior> _behaviors;

        public Selector() { _behaviors = new List<Behavior>(); }

        public override TerminationStatus Execute()
        {
            foreach (var child in _behaviors)
            {
                if (child.CanExecute())
                {
                    var status = child.Execute();
                    if (!status.Equals(TerminationStatus.Equals(TerminationStatus.Failed)))
                    {
                        return status;
                    }
                }
            }

            return TerminationStatus.Failed;
        }

        public override bool CanExecute() { return true; }
    }
}
