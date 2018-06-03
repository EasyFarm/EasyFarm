using EasyFarm.Classes;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.States
{
    public class AgentState : BaseState
    {
        protected AgentState(StateMemory stateMemory)
        {
            Memory = stateMemory;
        }

        public IConfig Config
        {
            get { return Memory.Config;}
            set { Memory.Config = value; }
        }

        /// <summary>
        ///     Retrieves aggroing creature.
        /// </summary>
        protected IUnitService UnitService
        {
            get { return Memory.UnitService; }
            set { Memory.UnitService = value; }
        }

        public IUnitFilters UnitFilters
        {
            get { return Memory.UnitFilters; }
            set { Memory.UnitFilters = value; }
        }

        public StateMemory Memory { get; }

        public IMemoryAPI EliteApi
        {
            get { return Memory.EliteApi; }
            set { Memory.EliteApi = value; }
        }

        public bool IsFighting
        {
            get { return Memory.IsFighting; }
            set { Memory.IsFighting = value; }
        }

        public IUnit Target
        {
            get { return Memory.Target; }
            set { Memory.Target = value; }
        }

        public Executor Executor
        {
            get { return Memory.Executor; }
            set { Memory.Executor = value; }
        }
    }
}