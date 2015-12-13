using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryAPI
{
    /// <summary>
    /// Status of OpenTradeMenu
    /// </summary>
    public enum ThreadStatus : byte
    {
        /// <summary>
        /// Default for when the constructor starts, but method was never run.
        /// </summary>
        NotStarted,
        /// <summary>
        /// Method just started the most recent run.
        /// </summary>
        Running,
        /// <summary>
        /// Method failed on last run.
        /// </summary>
        Failed,
        /// <summary>
        /// Method succeeded on last run.
        /// </summary>
        Succeeded
    }
}
