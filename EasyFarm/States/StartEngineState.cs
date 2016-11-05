using System;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    /// <summary>
    /// Sets up state before other states start firing. 
    /// </summary>
    public class StartEngineState : BaseState
    {
        public StartEngineState(IMemoryAPI fface) : base(fface) { }

        /// <summary>
        /// Setup any state before other states start firing. 
        /// </summary>
        /// <returns></returns>
        public override bool Check()
        {
            /// Reset all action's last cast times on FSM start. 
            foreach (var action in Config.Instance.BattleLists.Actions) action.LastCast = DateTime.Now;
            
            // Only run once at the FSM start. 
            Enabled = false;

            // No need to run body. 
            return false;
        }
    }
}