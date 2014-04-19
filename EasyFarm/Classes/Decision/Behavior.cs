using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public abstract class Behavior
    {
        protected ProgressStatus ProgressStatus { get; set; }
        protected TerminationStatus TerminationStatus { get; set; }
        protected abstract TerminationStatus Execute();
        protected abstract bool CanExecute();
    }

    public enum ProgressStatus
    {
        Ready,
        Running,
        Finished
    }

    public enum TerminationStatus
    {
        Success,
        Failed,
        Error
    }
}
