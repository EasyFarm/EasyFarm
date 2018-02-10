using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class AgentState : BaseState
    {
        protected AgentState(StateMemory stateMemory)
        {
            Memory = stateMemory;
        }

        /// <summary>
        ///     Retrieves aggroing creature.
        /// </summary>
        protected IUnitService UnitService
        {
            get => Memory.UnitService;
            set => Memory.UnitService = value;
        }

        public StateMemory Memory { get; }

        public IMemoryAPI EliteApi
        {
            get => Memory.EliteApi;
            set => Memory.EliteApi = value;
        }

        public bool IsFighting
        {
            get => Memory.IsFighting;
            set => Memory.IsFighting = value;
        }

        public IUnit Target
        {
            get => Memory.Target;
            set => Memory.Target = value;
        }

        public Executor Executor
        {
            get => Memory.Executor;
            set => Memory.Executor = value;
        }
    }
}